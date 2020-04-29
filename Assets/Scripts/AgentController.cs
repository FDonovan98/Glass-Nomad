using System.Collections;
using UnityEngine;
using Photon.Pun;

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
    OxygenRegen
}

public class AgentController : AgentInputHandler
{
    public GameObject deathScreen;
    public Color alienVision;
    public bool specialVision = false;

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
    
    public GameObject[] gameObjectsToDisableForPhoton;
    public Behaviour[] componentsToDisableForPhoton;

    public delegate void UpdateUI(ResourceType resourceType = ResourceType.None);
    public UpdateUI updateUI;

    private void Awake()
    { 

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
        foreach (GameObject element in gameObjectsToDisableForPhoton)
        {
            element.SetActive(false);   
        }
        foreach (Behaviour element in componentsToDisableForPhoton)
        {
            element.enabled = false;   
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
                oxygenIsRegening = true;
                updateUI(ResourceType.OxygenRegen);

                if (lowOxygen && currentOxygen > oxygenWarningAmount)
                {
                    lowOxygen = false;

                    if (updateUI != null)
                    {
                        updateUI(ResourceType.LowOxygen);
                    }
                }
            }
            else
            {
                oxygenIsRegening = false;
                updateUI(ResourceType.OxygenRegen);

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
        agent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        agent.GetComponent<Rigidbody>().AddForceAtPosition(RandomForce(150f), transform.position);
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
        
        deathScreen.SetActive(true);

        yield return new WaitForSeconds(3f);

        deathScreen.SetActive(false);
        agent.GetComponent<PhotonView>().ObservedComponents.Clear();
        agent.GetComponent<Spectator>().enabled = true;
    }

}
