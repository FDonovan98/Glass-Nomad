using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth
{
    public int maxHealth;
    public int currentHealth;
    public float fillAmount;
    public GameObject player;
    private UIBehaviour hudCanvas;

    public PlayerHealth(GameObject attachedPlayer, int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
        player = attachedPlayer;
        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
    }

    public void PlayerHit(int damage)
    {
        if (currentHealth < damage)
        {
            if (player.GetComponent<MarineMovement>() != null)
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
        //This is required to update player health not only when they shoot but more importantly when they get shot.
        hudCanvas.UpdateUI(player.GetComponent<PlayerAttack>());
    }

    private void UpdateFillAmount()
    {
        fillAmount = (float)currentHealth / maxHealth;
    }
}
