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
        remainingClipsText.text = "Remaining clips: " + attackScript.currentWeapon.magsLeft.ToString();
        currentClipText.text = "Current clip: " + attackScript.currentWeapon.bulletsInCurrentMag.ToString() + "/" + attackScript.currentWeapon.magSize.ToString();
        healthText.text = "Health: " + attackScript.healthScript.currentHealth.ToString();
        oxygenSlider.value = attackScript.oxygenAmountSeconds / attackScript.maxOxygenAmountSeconds * 100; // Gets the percentage
        oxygenPercentage.text = Mathf.Floor(oxygenSlider.value).ToString();
    }
}
