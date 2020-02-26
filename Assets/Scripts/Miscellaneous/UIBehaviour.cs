using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text currentClipText;
    [SerializeField] private TMP_Text remainingClipsText;
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private TMP_Text oxygenPercentage;
    [SerializeField] private TMP_Text objectiveText;
    
    public void UpdateUI(PlayerAttack attackScript = null)
    {
        if (!attackScript.photonView.IsMine)
        {
            return;
        }
        
        remainingClipsText.text = "Remaining clips: " + attackScript.resourcesScript.currentWeapon.magsLeft;
        currentClipText.text = "Current clip: " + attackScript.resourcesScript.currentWeapon.bulletsInCurrentMag + "/" + attackScript.resourcesScript.currentWeapon.magSize;
        healthText.text = "Health: " + attackScript.resourcesScript.currentHealth;
        oxygenSlider.value = attackScript.resourcesScript.oxygenAmountSeconds / attackScript.resourcesScript.maxOxygenAmountSeconds * 100; // Gets the percentage
        oxygenPercentage.text = Mathf.Floor(oxygenSlider.value).ToString();

        //Layer 9 is AlienCharacter. Alien's don't breathe :/
        if (attackScript.gameObject.layer == 9 && oxygenSlider.IsActive())
        {
            oxygenSlider.gameObject.SetActive(false);
            objectiveText.gameObject.SetActive(false);
        }
    }
}
