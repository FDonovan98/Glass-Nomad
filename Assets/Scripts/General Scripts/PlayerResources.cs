using System.Collections;
using UnityEngine;

public class PlayerResources
{
    public enum PlayerResource
    {
        Health,
        OxygenLevel
    }

    // Initialises the players health and ensure that we don't over-regen.
    public int maxHealth;

    // Keeps track of the player's current health.
    public int currentHealth;

    // Fills the health slider.
    public float fillAmount;

    // Ragdolls upon death.
    public GameObject player;

    // Updates the player's HUD when we get shot.
    public UIBehaviour hudCanvas;

    public Weapon currentWeapon;

    // Oxygen shenanigans
    public float maxOxygenAmountSeconds = 300f;
    public float oxygenAmountSeconds;

    public PlayerResources(GameObject attachedPlayer, int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
        player = attachedPlayer;
        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
        oxygenAmountSeconds = maxOxygenAmountSeconds;
    }

    public void UpdatePlayerResource(PlayerResource playerResource, float value)
    {
        if (playerResource == PlayerResource.OxygenLevel)
        {
            UpdateOxygen((int)value);
        }
        else if (playerResource == PlayerResource.Health)
        {
            UpdateHealth((int)value);
        }
        hudCanvas.UpdateUI(player.GetComponent<PlayerAttack>());
    }

    private void UpdateOxygen(int value)
    {
        oxygenAmountSeconds += value;
        if (oxygenAmountSeconds < 0)
        {
            oxygenAmountSeconds = 0;
        }
        if (oxygenAmountSeconds > maxOxygenAmountSeconds)
        {
            oxygenAmountSeconds = maxOxygenAmountSeconds;
        }
    }

    private void UpdateHealth(int value)
    {
        if (currentHealth < -value)
        {
            if (player.GetComponent<PlayerMovement>() != null)
            {
                player.GetComponent<PlayerMovement>().Ragdoll();
            }
        }
        else
        {
            currentHealth += value;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Update the health slider.
        fillAmount = (float)currentHealth / maxHealth;
    }
}
