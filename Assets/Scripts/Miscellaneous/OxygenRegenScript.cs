using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenRegenScript : MonoBehaviour
{
    public int PercentageOxygenRegenPerSecond = 10;

    private void OnTriggerStay(Collider other)
    {
        PlayerAttack playerAttack;

        // Layer 8 is MarineCharacter. Harry made me type this.
        if (other.gameObject.layer == 8 && other.CompareTag("Player")) // Marine regen
        {
            playerAttack = other.GetComponent<MarineController>().marineAttack;
        }
        else // Alien regen
        {
            playerAttack = other.GetComponent<AlienController>().alienAttack;
        }

        playerAttack.oxygenAmountSeconds += playerAttack.maxOxygenAmountSeconds * PercentageOxygenRegenPerSecond * Time.deltaTime;
        playerAttack.oxygenAmountSeconds += Time.deltaTime;
        if (playerAttack.oxygenAmountSeconds > playerAttack.maxOxygenAmountSeconds)
        {
            playerAttack.oxygenAmountSeconds = playerAttack.maxOxygenAmountSeconds;
        }
    }
}
