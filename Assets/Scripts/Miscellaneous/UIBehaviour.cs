using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class UIBehaviour : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text currentClipText;
    public TMP_Text remainingClipsText;
    public Slider oxygenSlider;
    public TMP_Text oxygenPercentage;
    
    public void UpdateUI(PlayerAttack attackScript = null)
    {
        remainingClipsText.text = "Remaining clips: " + attackScript.resourcesScript.magsLeft.ToString();
        currentClipText.text = "Current clip: " + attackScript.resourcesScript.bulletsInCurrentMag.ToString() + "/" + attackScript.currentWeapon.magSize.ToString();
        healthText.text = "Health: " + attackScript.resourcesScript.currentHealth.ToString();
        oxygenSlider.value = attackScript.resourcesScript.oxygenAmountSeconds / attackScript.resourcesScript.maxOxygenAmountSeconds * 100; // Gets the percentage
        oxygenPercentage.text = Mathf.Floor(oxygenSlider.value).ToString();

        //Layer 9 is AlienCharacter. Alien's don't breathe :/
        if (attackScript.gameObject.layer == 9 && oxygenSlider.IsActive() && attackScript.photonView.IsMine)
        {
            oxygenSlider.gameObject.SetActive(false);
        }
    }
}
