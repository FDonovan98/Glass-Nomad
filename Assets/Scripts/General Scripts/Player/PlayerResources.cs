using System.Collections;
using UnityEngine;

public class PlayerResources
{
    public enum PlayerResource
    {
        Health,
        OxygenLevel,
        Ammo,
        Magazines
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
    public float maxOxygenAmountSeconds = 500f;
    public float oxygenAmountSeconds;
    public float sprintOxygenMultiplier = 1.3f;

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
            UpdateOxygen(value);
        }
        else if (playerResource == PlayerResource.Health)
        {
            UpdateHealth((int)value);
        }
        else if (playerResource == PlayerResource.Ammo)
        {
            UpdateAmmo((int)value);
        }
        else if (playerResource == PlayerResource.Magazines)
        {
            UpdateMagazines((int)value);
        }
        hudCanvas.UpdateUI(player.GetComponent<PlayerAttack>());
    }

    private void UpdateOxygen(float value)
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

    private void UpdateAmmo(int value)
    {
        currentWeapon.bulletsInCurrentMag += value;
        if (currentWeapon.bulletsInCurrentMag > currentWeapon.magSize)
        {
            currentWeapon.bulletsInCurrentMag = currentWeapon.magSize;
        }
        if (currentWeapon.bulletsInCurrentMag < 0)
        {
            currentWeapon.bulletsInCurrentMag = 0;
        }
    }

    private void UpdateMagazines(int value)
    {
        currentWeapon.magsLeft += value;
        if (currentWeapon.magsLeft < 0)
        {
            currentWeapon.magsLeft = 0;
        }
    }

    /// <summary>
    /// Deducts the amount of magazines you have left, and refills the bullets in your
    /// current magazine. Prints a message if you have no more magazines.
    /// </summary>
    public void Reload()
    {
        if (currentWeapon.magsLeft > 0)
        {
            UpdatePlayerResource(PlayerResource.Ammo, currentWeapon.magSize);
            UpdatePlayerResource(PlayerResource.Magazines, -1);
        }
        else if (currentWeapon.magsLeft == -1)
        {
            Debug.Log("MELEE");
        }
        else
        {
            Debug.Log("You are out of magazines for this weapon. Find more ammo.");
        }
    }
}
