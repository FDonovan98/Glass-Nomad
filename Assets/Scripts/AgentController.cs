using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
<<<<<<< HEAD
using System.Collections;

using UnityEngine.UI;

public enum ResourceType
{
    Health,
    MagazineAmmo,
    ExtraAmmo,
    Oxygen,
    WallClimbing,
    EmergencyRegen
}
=======
>>>>>>> c55a8081b03de99dd0b8146e4eeda3bb35f2e01e

public class AgentController : AgentInputHandler
{
    public GameObject deathScreen;
    public Color alienVision;
    public bool specialVision = false;

    [Header("UI")]
    public TextMeshProUGUI healthUIText;
    public TextMeshProUGUI ammoUIText;
    public GameObject oxygenDisplay;
    public GameObject wallClimbingSymbol;

    [Header("Current Stats")]
    [ReadOnly]
    public float currentHealth = 0.0f;
    [ReadOnly]
    public float currentOxygen = 0.0f;
    [Range(0, 100)]
    public int oxygenWarningAmount = 30;
    public GameObject oxygenWarning = null;
    [ReadOnly]
    public int currentExtraAmmo = 0;
    [ReadOnly]
    public int currentBulletsInMag = 0;
    [ReadOnly]
    public bool emergencyRegenActive = false;
    public int emergencyRegenUsesRemaining = 0;
    [ReadOnly]
    public bool isWallClimbing = false;
    
    public GameObject[] gameObjectsToDisableForPhoton;
    public Behaviour[] componentsToDisableForPhoton;

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

        UpdateUI();
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
            UpdateWallClimbingUI();
        }
    }

    public void ChangeStat(ResourceType resourceType, int value)
    {
        if (resourceType == ResourceType.MagazineAmmo)
        {
            currentBulletsInMag = (int)Mathf.Clamp(currentBulletsInMag + value, 0.0f, currentWeapon.magSize);

            if (ammoUIText != null)
            {
                UpdateUI(ResourceType.MagazineAmmo);
            }
        }

        if (resourceType == ResourceType.ExtraAmmo)
        {
            currentExtraAmmo = (int)Mathf.Max(currentExtraAmmo + value, 0.0f);

            if (ammoUIText != null)
            {
                UpdateUI(ResourceType.MagazineAmmo);
            }
        }
    }

    public void ChangeStat(ResourceType resourceType, float value)
    {
        if (resourceType == ResourceType.Health)
        {
            currentHealth += value;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                AgentHasDied();
            }

            UpdateUI(ResourceType.Health);
        }
        else if (resourceType == ResourceType.Oxygen)
        {
            currentOxygen = Mathf.Clamp(currentOxygen + value, 0.0f, agentValues.maxOxygen);

            if (currentOxygen == 0.0f)
            {
                ChangeStat(ResourceType.Health, -(agentValues.suffocationDamage * Time.deltaTime));
            }
            else if (currentOxygen <= oxygenWarningAmount)
            {
                // Display warning
                DisplayOxygenWarning(true);
            }
            else
            {
                if (oxygenWarning != null && oxygenWarning.activeInHierarchy)
                {
                    DisplayOxygenWarning(false);
                }
            }

            if (oxygenDisplay != null)
            {
                UpdateUI(ResourceType.Oxygen);
            }
        }
    }

    private void UpdateUI()
    {
        UpdateAmmoUI();
        UpdateHealthUI();
        UpdateOxygenUI();
        UpdateWallClimbingUI();
    }

    private void UpdateUI(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.MagazineAmmo:
            case ResourceType.ExtraAmmo:
                UpdateAmmoUI();
                break;
            case ResourceType.Health:
                UpdateHealthUI();
                break;
            case ResourceType.Oxygen:
                UpdateOxygenUI();
                break;
            case ResourceType.WallClimbing:
                UpdateWallClimbingUI();
                break;
            default:
                Debug.LogWarning(gameObject.name + " tried to update UI of unrecognized type.");
                break;
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoUIText != null)
        {
            ammoUIText.text = currentBulletsInMag + " / " + currentExtraAmmo;
        }
    }

	void UpdateWallClimbingUI()
    {
        if (wallClimbingSymbol != null)
        {
            wallClimbingSymbol.SetActive(isWallClimbing);
        }
    }

    private void UpdateHealthUI()
    {
        if (healthUIText != null)
        {
            healthUIText.text = "Health: " + Mathf.RoundToInt(currentHealth / agentValues.maxHealth * 100);
        }
    }

    private void UpdateOxygenUI()
    {
        if (oxygenDisplay != null)
        {
            Slider oxygenSlider = oxygenDisplay.GetComponentInChildren<Slider>();
            TextMeshProUGUI oxygenText = oxygenDisplay.GetComponentInChildren<TextMeshProUGUI>();
            oxygenSlider.value = currentOxygen / agentValues.maxOxygen * 100;
            oxygenText.text = (Mathf.Round(currentOxygen / agentValues.maxOxygen * 100)).ToString();
        }
    }

    private void DisplayOxygenWarning(bool shouldEnable)
    {
        if (oxygenWarning != null)
        {
            oxygenWarning.SetActive(shouldEnable);
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
