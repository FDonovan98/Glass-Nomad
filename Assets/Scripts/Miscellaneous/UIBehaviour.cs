using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI currentClipText;
    public TextMeshProUGUI remainingClipsText;

    public void UpdateUI(PlayerAttack attackScript = null)
    {
        remainingClipsText.text = "Remaining clips: " + attackScript.currentWeapon.magsLeft.ToString();
        currentClipText.text = "Current clip: " + attackScript.currentWeapon.bulletsInCurrentMag.ToString() + "/" + attackScript.currentWeapon.magSize.ToString();
        healthText.text = "Health: " + attackScript.healthScript.currentHealth.ToString();
    }
}
