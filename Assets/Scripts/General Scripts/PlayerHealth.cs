using UnityEngine;

public class PlayerHealth
{
    // Used to initialise the players health and ensure that we don't over-regen.
    public int maxHealth;

    // Used to keep track of the player's current health.
    public int currentHealth;

    // Used to fill the health slider.
    public float fillAmount;

    // Used to ragdoll upon death.
    public GameObject player;

    // Used to update the player's HUD when we get shot.
    private UIBehaviour hudCanvas;

    /// <summary>
    /// Constructor method for PlayerHealth.
    /// Assigns the variables using passed variables.
    /// </summary>
    /// <param name="attachedPlayer"></param>
    /// <param name="playerMaxHealth"></param>
    public PlayerHealth(GameObject attachedPlayer, int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
        player = attachedPlayer;
        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
    }

    /// <summary>
    /// Called when a player is hit. Deducts the player's health, and kills them if
    /// they're health drops below 0. Also used to regen players and update the health
    /// slider fill amount. The UI is updated afterwards.
    /// </summary>
    /// <param name="damage"></param>
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

        fillAmount = (float)currentHealth / maxHealth;

        //This is required to update player health not only when they shoot but more importantly when they get shot.
        hudCanvas.UpdateUI(player.GetComponent<PlayerAttack>());
    }
}
