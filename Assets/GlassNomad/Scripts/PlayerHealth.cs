using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth
{
    public int health;

    public PlayerHealth(int maxHealth = 100)
    {
        health = maxHealth;
    }

    public void PlayerHit(int damage = 10)
    {
        health -= damage;
    }
}
