using UnityEngine;


using TMPro;
using UnityEngine.UI;

public class AgentUIController : MonoBehaviour
{
    public AgentController agentController;
    public TextMeshProUGUI healthUIText;
    public Image healthUIImage;
    public TextMeshProUGUI ammoUIText;
    public TextMeshProUGUI oxygenUIText;
    public Image oxygenUIImage;
    public GameObject wallClimbingUISymbol;
    public GameObject emergencyRegenUISymbol;
    public GameObject LowOxygenUIObject;
    public GameObject oxyIsRegeningObject;

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
        if (emergencyRegenUISymbol != null)
        {
            emergencyRegenUISymbol.SetActive(agentController.emergencyRegenActive);
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
        if (wallClimbingUISymbol != null)
        {
            wallClimbingUISymbol.SetActive(agentController.isWallClimbing);
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
