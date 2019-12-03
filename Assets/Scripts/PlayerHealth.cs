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

    public int PlayerHit(int damage = 10)
    {
        if (health < damage)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }
        return health;
    }
}
