using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenRegenScript : MonoBehaviour
{
    public int PercentageOxygenRegenPerSecond = 10;

    private void OnTriggerStay(Collider other)
    {
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
        }
    }
}
