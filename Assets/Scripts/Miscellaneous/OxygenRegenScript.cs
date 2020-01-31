using UnityEngine;

public class OxygenRegenScript : MonoBehaviour
{
    public float PercentageOxygenRegenPerSecond = 10f;

    private void OnTriggerStay(Collider other)
    {
        PlayerAttack playerAttack;

        // Layer 8 is MarineCharacter. Harry made me type this.
        if (other.gameObject.layer == 8 && other.CompareTag("Player"))
        {
            // Marine regen
            playerAttack = other.GetComponent<MarineController>().marineAttack;
            playerAttack.oxygenAmountSeconds += playerAttack.maxOxygenAmountSeconds * (PercentageOxygenRegenPerSecond / 100f) * Time.deltaTime;
            playerAttack.oxygenAmountSeconds += Time.deltaTime;
            if (playerAttack.oxygenAmountSeconds > playerAttack.maxOxygenAmountSeconds)
            {
                playerAttack.oxygenAmountSeconds = playerAttack.maxOxygenAmountSeconds;
            }
        }
    }
}
