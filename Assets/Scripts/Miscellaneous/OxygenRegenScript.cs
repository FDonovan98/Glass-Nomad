<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> master

public class OxygenRegenScript : MonoBehaviour
{
    public int PercentageOxygenRegenPerSecond = 10;

    private void OnTriggerStay(Collider other)
    {
<<<<<<< HEAD
        // Layer 8 is MarineCharacter. Harry made me type this.
        if (other.gameObject.layer == 8 && other.CompareTag("Player"))
        {
            MarineController marineController = other.GetComponent<MarineController>();
            marineController.oxygenAmountSeconds += marineController.maxOxygenAmountSeconds * PercentageOxygenRegenPerSecond * Time.deltaTime;
            marineController.oxygenAmountSeconds += Time.deltaTime;
            if (marineController.oxygenAmountSeconds > marineController.maxOxygenAmountSeconds)
            {
                marineController.oxygenAmountSeconds = marineController.maxOxygenAmountSeconds;
            }
=======
        PlayerAttack playerAttack = null;

        // Layer 8 is MarineCharacter. Harry made me type this.
        if (other.gameObject.layer == 8 && other.CompareTag("Player")) // Marine regen
        {
            playerAttack = other.GetComponent<MarineController>().marineAttack;
        }
        //I have no idea how many times I've had to fix this.
        else if (other.gameObject.layer == 9) // Alien regen
        {
            playerAttack = other.GetComponent<AlienController>().alienAttack;
        }

        if (playerAttack != null)
        {
            float oxygenIncrease = (playerAttack.resourcesScript.maxOxygenAmountSeconds * PercentageOxygenRegenPerSecond * Time.deltaTime) + Time.deltaTime;
            playerAttack.resourcesScript.UpdatePlayerResource(PlayerResources.PlayerResource.OxygenLevel, oxygenIncrease);
>>>>>>> master
        }
    }
}
