using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Create New Weapon")]
public class Weapon : ScriptableObject
{
    public enum FireType
    {
        Single,
        Burst,
        FullAuto
    }

    public FireType fireMode;
    public int magCount;
    public float fireRate = 0f;
    public int magSize;
    public float range;
    public int damage;

    // These are hidden in the inspector as they don't need to be set.
    [HideInInspector] public int magsLeft;
    [HideInInspector] public int bulletsInCurrentMag;
}
