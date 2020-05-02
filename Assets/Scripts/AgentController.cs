using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public enum ResourceType
{
    None,
    Health,
    MagazineAmmo,
    ExtraAmmo,
    Oxygen,
    WallClimbing,
    EmergencyRegen,
    LowOxygen,
    OxygenRegen, 
    AlienVision
}

public class AgentController : AgentInputHandler
{
    public GameObject deathScreen;
    public Color alienVision;
    public bool specialVision = false;
    public GameObject emergencyRegenParticleSystem;
    public GameObject emergencyRegenParticleSystems;
    public AudioClip oxygenWarningAudio = null;
    public float oxygenWarningDingStartRate = 2.0f;
    private float timeInLowOxygen = float.MaxValue;

    [Header("Current Stats")]
    [ReadOnly]
    public float currentHealth = 0.0f;
    [ReadOnly]
    public float currentOxygen = 0.0f;
    [Range(0, 100)]
    public int oxygenWarningAmount = 30;
    public bool lowOxygen = false;
    [ReadOnly]
    public int currentExtraAmmo = 0;
    [ReadOnly]
    public int currentBulletsInMag = 0;
    [ReadOnly]
    public bool emergencyRegenActive = false;
    public int emergencyRegenUsesRemaining = 0;
    [ReadOnly]
    public bool isWallClimbing = false;
    [ReadOnly]
    public bool oxygenIsRegening = false;
    [ReadOnly]
    public bool alienVisionIsActive = false;
    
    public GameObject[] gameObjectsToToggleForPhoton;
    public Behaviour[] componentsToToggleForPhoton;

    public delegate void UpdateUI(ResourceType resourceType = ResourceType.None);
    public UpdateUI updateUI;

    private void Awake()
    { 
        updateUI += TriggerEffectsOnStatChange;

        if (agentValues != null)
        {
            currentOxygen = agentValues.maxOxygen;
            currentHealth = agentValues.maxHealth;
            emergencyRegenUsesRemaining = agentValues.emergencyRegenUses;
            Debug.Log("awake");
        }

        if (currentWeapon != null)
        {
            currentBulletsInMag = currentWeapon.bulletsInCurrentMag;
            currentExtraAmmo = currentWeapon.magSize * 2;
            timeSinceLastShot = currentWeapon.fireRate;
        }

        if (specialVision && photonView.IsMine)
        {
            SpawnFadeFromBlack.Fade(Color.black, alienVision, 3, this);
            RenderSettings.fog = false;
        }

        if (photonView != null)
        {
            if (!photonView.IsMine && !PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
            {
                DisableObjectsForPhoton();
                isLocalAgent = false;
            }
        }

        if (updateUI != null)
        {
            updateUI();
        }
    }

    void TriggerEffectsOnStatChange(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.EmergencyRegen:
                if (emergencyRegenParticleSystem != null)
                {
                    photonView.RPC("EmergencyRegenSmoke", RpcTarget.All, photonView.ViewID);
                }
                break;

            case ResourceType.Oxygen:
                if (lowOxygen && oxygenWarningAudio != null)
                {
                    timeInLowOxygen += Time.deltaTime;
                    float boundryTime = currentOxygen / agentValues.maxOxygen;
                    boundryTime /= (oxygenWarningAmount / agentValues.maxOxygen);
                    boundryTime *= oxygenWarningDingStartRate + oxygenWarningAudio.length;

                    if (timeInLowOxygen > boundryTime)
                    {
                        mainAudioSource.PlayOneShot(oxygenWarningAudio);
                        timeInLowOxygen = 0.0f;
                    }
                }
                break;

            default:
                break;
        }
    }

    public override void ChangeWeapon(Weapon weapon)
    {
        base.ChangeWeapon(weapon);
        
        currentBulletsInMag = currentWeapon.bulletsInCurrentMag;
        currentExtraAmmo = currentWeapon.magSize * 2;

        if (updateUI != null)
        {
            updateUI(ResourceType.ExtraAmmo);
        }

    }

    private void DisableObjectsForPhoton()
    {
        foreach (GameObject element in gameObjectsToToggleForPhoton)
        {
            element.SetActive(!element.activeSelf);   
        }
        foreach (Behaviour element in componentsToToggleForPhoton)
        {
            element.enabled = !element.isActiveAndEnabled;   
        }
    }

    public void ChangeStat(ResourceType resourceType, bool value)
    {
        if (resourceType == ResourceType.WallClimbing)
        {
            isWallClimbing = value;

            if (updateUI != null)
            {
                updateUI(ResourceType.WallClimbing);
            }
        }

        if (resourceType == ResourceType.EmergencyRegen)
        {
            emergencyRegenActive = value;

            if (updateUI != null)
            {
                updateUI(ResourceType.EmergencyRegen);
            }
        }

        if (resourceType == ResourceType.AlienVision)
        {
            alienVisionIsActive = value;
            updateUI(ResourceType.AlienVision);
        }
    }

    public void ChangeStat(ResourceType resourceType, int value)
    {
        if (resourceType == ResourceType.MagazineAmmo)
        {
            currentBulletsInMag = (int)Mathf.Clamp(currentBulletsInMag + value, 0.0f, currentWeapon.magSize);

            if (updateUI != null)
            {
                updateUI(ResourceType.ExtraAmmo);
            }
        }

        if (resourceType == ResourceType.ExtraAmmo)
        {
            currentExtraAmmo = (int)Mathf.Max(currentExtraAmmo + value, 0.0f);

            if (updateUI != null)
            {
                updateUI(ResourceType.ExtraAmmo);
            }
        }
    }

    public void ChangeStat(ResourceType resourceType, float value)
    {
        if (resourceType == ResourceType.Health)
        {
            currentHealth += value * equippedArmour.damageMultiplier;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                AgentHasDied();
            }

            if (updateUI != null)
            {
                updateUI(ResourceType.Health);
            }
        }
        else if (resourceType == ResourceType.Oxygen)
        {
            currentOxygen = Mathf.Clamp(currentOxygen + value, 0.0f, agentValues.maxOxygen);

            if (value > 0)
            {
                if (lowOxygen && currentOxygen > oxygenWarningAmount)
                {
                    lowOxygen = false;
                    timeInLowOxygen = float.MaxValue;

                    if (updateUI != null)
                    {
                        updateUI(ResourceType.LowOxygen);
                    }
                }
            }
            else
            {
                if (currentOxygen == 0.0f)
                {
                    ChangeStat(ResourceType.Health, -(agentValues.suffocationDamage * Time.deltaTime));
                }
                else if (!lowOxygen && currentOxygen <= oxygenWarningAmount)
                {
                    lowOxygen = true;

                    if (updateUI != null)
                    {
                        updateUI(ResourceType.LowOxygen);
                    }
                }
            }

            if (updateUI != null)
            {
                updateUI(ResourceType.Oxygen);
            }
        }
    }

    /// <summary>
    /// Disables the player's input, enables rotations in the rigidbody, adds a random force to the
    /// rigidbody, and starts the 'Death' coroutine.
    /// </summary>
    public void AgentHasDied()
    {
        allowInput = false;
        agentRigidbody.constraints = RigidbodyConstraints.None;
        agentRigidbody.AddForceAtPosition(RandomForce(150f), transform.position);

        if (agentValues.deathNoise != null)
        {
            mainAudioSource.PlayOneShot(agentValues.deathNoise);
        }

        StartCoroutine(Death(agent));
    }
    
    /// <summary>
    /// Returns a vector with all axes having a random value between 0 and the 'velocity' parameter.
    /// </summary>
    /// <param name="velocity">The maximum random force.</param>
    /// <returns>Returns a vector with all axes having a random value between 0 and the 'velocity' parameter.</returns>
    private Vector3 RandomForce(float velocity)
    {
        return new Vector3(Random.Range(0, velocity), Random.Range(0, velocity), Random.Range(0, velocity));
    }
    
    private IEnumerator Death(GameObject player)
    {
        yield return new WaitForSeconds(3f);
        
        deathScreen.GetComponentInChildren<TMP_Text>().text = "You Died";
        deathScreen.GetComponent<Image>().enabled = true;
        deathScreen.GetComponentInChildren<Button>().gameObject.SetActive(true);
        deathScreen.SetActive(true);

        yield return new WaitForSeconds(3f);

        deathScreen.SetActive(false);
        agent.GetComponent<PhotonView>().ObservedComponents.Clear();
        agent.GetComponent<Spectator>().enabled = true;
    }

}
