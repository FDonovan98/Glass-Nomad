using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth
{
    public int maxHealth;
    public int currentHealth;
    public float fillAmount;
    public GameObject player;

    public PlayerHealth(GameObject attachedPlayer, int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
        player = attachedPlayer;
    }

    public void PlayerHit(int damage)
    {
        if (currentHealth < damage)
        {
            player.GetComponent<MarineMovement>().Ragdoll();
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        fillAmount = (float)currentHealth / maxHealth;
    }
}
