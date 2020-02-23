using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text currentClipText;
    public TMP_Text remainingClipsText;
    public Slider oxygenSlider;
    public TMP_Text oxygenPercentage;
    
    public void UpdateUI(PlayerAttack attackScript = null)
    {
        remainingClipsText.text = "Remaining clips: " + attackScript.resourcesScript.currentWeapon.magsLeft;
        currentClipText.text = "Current clip: " + attackScript.resourcesScript.currentWeapon.bulletsInCurrentMag + "/" + attackScript.resourcesScript.currentWeapon.magSize;
        healthText.text = "Health: " + attackScript.resourcesScript.currentHealth;
        oxygenSlider.value = attackScript.resourcesScript.oxygenAmountSeconds / attackScript.resourcesScript.maxOxygenAmountSeconds * 100; // Gets the percentage
        oxygenPercentage.text = Mathf.Floor(oxygenSlider.value).ToString();

        //Layer 9 is AlienCharacter. Alien's don't breathe :/
        if (attackScript.gameObject.layer == 9 && oxygenSlider.IsActive() && attackScript.photonView.IsMine)
        {
            oxygenSlider.gameObject.SetActive(false);
        }
    }
}
