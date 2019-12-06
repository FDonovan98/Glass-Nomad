public class PlayerHealth
{
    public int maxHealth;
    public int currentHealth;
    public float fillAmount;

    public PlayerHealth(int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
    }

    public void PlayerHit(int damage)
    {
        if (currentHealth < damage)
        {
            currentHealth = 0;
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
