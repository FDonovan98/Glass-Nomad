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
    
    public int maxHealth;
    public int currentHealth;
    public float fillAmount;
    public GameObject player;
    private UIBehaviour hudCanvas;

    // Oxygen shenanigans
    public float maxOxygenAmountSeconds = 300f;
    public float oxygenAmountSeconds;

    [HideInInspector] public int magsLeft;
    [HideInInspector] public int bulletsInCurrentMag;

    public PlayerResources(GameObject attachedPlayer, int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
        player = attachedPlayer;
        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
        oxygenAmountSeconds = maxOxygenAmountSeconds;
    }

    public void PlayerHit(int damage)
    {
       
    }

    private void UpdateFillAmount()
    {
        fillAmount = (float)currentHealth / maxHealth;
    }

    public void UpdatePlayerResource(PlayerResource type, float value)
    {
        if (type == PlayerResource.OxygenLevel)
        {
            oxygenAmountSeconds -= value;
            if (oxygenAmountSeconds < 0)
            {
                oxygenAmountSeconds = 0;
            }
        }
        else if (type == PlayerResource.Ammo)
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
        }
        else if (type == PlayerResource.Magazines)
        {
            if (magsLeft > 0)
            {
                magsLeft -= (int)value;
            }
            if (magsLeft < 0)
            {
                magsLeft = 0;
            }
        }
        else if (type == PlayerResource.Health)
        {
            if (currentHealth < (int)value)
            {
                if (player.GetComponent<MarineMovement>() != null)
                    player.GetComponent<MarineMovement>().Ragdoll();
            }
            else
            {
                currentHealth -= (int)value;
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
