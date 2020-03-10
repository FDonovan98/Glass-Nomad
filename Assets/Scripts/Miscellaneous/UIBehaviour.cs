<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
=======
﻿using UnityEngine;
using UnityEngine.UI;
>>>>>>> master
using TMPro;

public class UIBehaviour : MonoBehaviour
{
<<<<<<< HEAD
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI currentClipText;
    public TextMeshProUGUI remainingClipsText;

    public void UpdateUI(PlayerAttack attackScript = null)
    {
        remainingClipsText.text = "Remaining clips: " + attackScript.currentWeapon.magsLeft.ToString();
        currentClipText.text = "Current clip: " + attackScript.currentWeapon.bulletsInCurrentMag.ToString() + "/" + attackScript.currentWeapon.magSize.ToString();
        healthText.text = "Health: " + attackScript.healthScript.currentHealth.ToString();
=======
    [SerializeField] private TMP_Text healthText = null;
    [SerializeField] private TMP_Text currentClipText = null;
    [SerializeField] private TMP_Text remainingClipsText = null;
    [SerializeField] private Slider oxygenSlider = null;
    [SerializeField] private TMP_Text oxygenPercentage = null;
    [SerializeField] private TMP_Text objectiveText = null;
    
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
>>>>>>> master
    }
}
