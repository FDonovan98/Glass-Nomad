using UnityEngine;


using TMPro;
using UnityEngine.UI;

public class AgentUIController : MonoBehaviour
{
    public AgentController agentController;

    [Header("Health UI")]
    public TextMeshProUGUI healthUIText;
    public Image healthUIImage;

    [Header("Ammo UI")]
    public TextMeshProUGUI ammoUIText;

    [Header("Oxygen UI")]
    public TextMeshProUGUI oxygenUIText;
    public Image oxygenUIImage;
    public GameObject LowOxygenUIObject;
    public GameObject oxyIsRegeningObject;

    [Header("Wall Climbing UI")]
    public GameObject wallClimbingActiveUISymbol;
    public GameObject wallClimbingInactiveUISymbol;

    [Header("Emergency Regeneration UI")]
    public GameObject emergencyRegenUnusedUISymbol;
    public GameObject emergencyRegenUsedUISymbol;

    [Header("Alien Vision UI")]
    public GameObject alienVisionActiveUI;
    public GameObject alienVisionInactiveUI;

    [Header("Objective UI")]
    public Image outerReticule;
    public TMP_Text interactionText;

    private void OnEnable()
    {
        agentController.updateUI += UpdateUI;
    }

    private void UpdateUI(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.None:
                UpdateUI();
                break;

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
            
            case ResourceType.LowOxygen:
                UpdateLowOxygenUI();
                break;

            case ResourceType.EmergencyRegen:
                UpdateEmergencyRegenUI();
                break;

            case ResourceType.OxygenRegen:
                UpdateOxygenRegenUI();
                break;

            case ResourceType.AlienVision:
                UpdateAlienVisionUI();
                break;

            default:
                Debug.LogWarning(gameObject.name + " tried to update UI of unrecognized type.");
                break;
        }
    }

    void UpdateUI()
    {
        UpdateAmmoUI();
        UpdateHealthUI();
        UpdateOxygenUI();
        UpdateWallClimbingUI();
        UpdateEmergencyRegenUI();
        UpdateLowOxygenUI();
        UpdateOxygenRegenUI();
        UpdateAlienVisionUI();
    }

    void UpdateAlienVisionUI()
    {
        if (alienVisionActiveUI != null)
        {
            alienVisionActiveUI.SetActive(agentController.alienVisionIsActive);
        }

        if (alienVisionInactiveUI != null)
        {
            alienVisionInactiveUI.SetActive(!agentController.alienVisionIsActive);
        }
    }

    void UpdateOxygenRegenUI()
    {
        oxyIsRegeningObject.SetActive(agentController.oxygenIsRegening);
    }

    void UpdateLowOxygenUI()
    {
        LowOxygenUIObject.SetActive(agentController.lowOxygen);
    }

    void UpdateEmergencyRegenUI()
    {
        if (emergencyRegenUsedUISymbol != null && emergencyRegenUnusedUISymbol != null)
        {
            if (agentController.emergencyRegenUsesRemaining <= 0)
            {
                emergencyRegenUsedUISymbol.SetActive(true);
                emergencyRegenUnusedUISymbol.SetActive(false);
            }
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoUIText != null)
        {
            ammoUIText.text = agentController.currentBulletsInMag + " / " + agentController.currentExtraAmmo;
        }
    }

	void UpdateWallClimbingUI()
    {
        if (wallClimbingActiveUISymbol != null)
        {
            wallClimbingActiveUISymbol.SetActive(agentController.isWallClimbing);
        }
        
        if (wallClimbingInactiveUISymbol != null)
        {
            wallClimbingInactiveUISymbol.SetActive(!agentController.isWallClimbing);
        }
    }

    private void UpdateHealthUI()
    {
        if (healthUIText != null)
        {
            healthUIText.text = Mathf.RoundToInt(agentController.currentHealth / agentController.agentValues.maxHealth * 100).ToString() + "%";
        }

        if (healthUIImage != null)
        {
            healthUIImage.fillAmount = agentController.currentHealth / agentController.agentValues.maxHealth;
        }
    }

    private void UpdateOxygenUI()
    {
        if (oxygenUIText != null)
        {
            oxygenUIText.text = Mathf.RoundToInt(agentController.currentOxygen / agentController.agentValues.maxOxygen * 100).ToString() + "%";
        }

        if (oxygenUIImage != null)
        {
            oxygenUIImage.fillAmount = agentController.currentOxygen / agentController.agentValues.maxOxygen;
        }
    }
}
