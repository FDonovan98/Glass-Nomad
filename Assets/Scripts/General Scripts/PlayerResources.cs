using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerResources
{
    public enum PlayerResource
    {
        Health,
        Ammo,
        Magazines,
        OxygenLevel
    }

    // Used to initialise the players health and ensure that we don't over-regen.
    public int maxHealth;

    // Used to keep track of the player's current health.
    public int currentHealth;

    // Used to fill the health slider.
    public float fillAmount;

    // Used to ragdoll upon death.
    public GameObject player;

    // Used to update the player's HUD when we get shot.
    public UIBehaviour hudCanvas;

    // Oxygen shenanigans
    public float maxOxygenAmountSeconds = 300f;
    public float oxygenAmountSeconds;

    [HideInInspector] public int magsLeft;
    [HideInInspector] public int bulletsInCurrentMag;

    private MonoBehaviour monoBehaviour;

    public PlayerResources(GameObject attachedPlayer, MonoBehaviour instance, int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
        player = attachedPlayer;
        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
        oxygenAmountSeconds = maxOxygenAmountSeconds;
        monoBehaviour = instance;
    }

    private void UpdateFillAmount()
    {
        fillAmount = (float)currentHealth / maxHealth;
    }

    public void UpdatePlayerResource(PlayerResource playerResource, float value)
    {
        if (playerResource == PlayerResource.OxygenLevel)
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
        else if (playerResource == PlayerResource.Ammo)
        {
            bulletsInCurrentMag += (int)value;
            if (bulletsInCurrentMag > player.GetComponent<PlayerAttack>().currentWeapon.magSize)
            {
                bulletsInCurrentMag = player.GetComponent<PlayerAttack>().currentWeapon.magSize;
            }
            if (bulletsInCurrentMag < 0)
            {
                bulletsInCurrentMag = 0;
            }
            Debug.Log(bulletsInCurrentMag);
        }
        else if (playerResource == PlayerResource.Magazines)
        {
            magsLeft += (int)value;
            if (magsLeft < 0)
            {
                magsLeft = 0;
            }
        }
        else if (playerResource == PlayerResource.Health)
        {
            if (currentHealth < -(int)value)
            {
                if (player.GetComponent<PlayerMovement>() != null)
                {
                    player.GetComponent<PlayerMovement>().Ragdoll();
                }
            }
            else
            {
                currentHealth += (int)value;
            }

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            UpdateFillAmount();
        }
        hudCanvas.UpdateUI(player.GetComponent<PlayerAttack>());
    }
}
