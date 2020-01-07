using UnityEngine;
using Photon.Pun;

public class PlayerHealth
{
    public int maxHealth;
    public int currentHealth;
    public float fillAmount;

    public PlayerHealth(GameObject attachedPlayer, int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
    }

    public void PlayerHit(int damage, GameObject player)
    {
        if (currentHealth < damage)
        {
            PhotonNetwork.Destroy(player);
        }
        else
        {
            currentHealth -= damage;
        }

        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        fillAmount = (float)currentHealth / maxHealth;
    }
}
