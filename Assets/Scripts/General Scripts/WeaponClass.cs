using UnityEngine;
using Photon.Pun;

public class WeaponClass
{
    public enum FireType
    {
        Single,
        Burst,
        FullAuto
    }
    
    // Number of magazines for the weapon.
    public int magCount;
    public FireType fireMode;

    public int magsLeft;

    public int bulletsInCurrentMag;

    // Minimum time delay between each shot.
    public float fireRate = 0f;
    // Max number of bullets stored in a magazine.
    // 0 If the weapon should never reload (melee).
    public int magSize;
    public float range;
    public int damage;

    public WeaponClass(int magazineCount, float shotsPerSecond, int magazingSize, float weaponRange, int weaponDamage)
    {
        magCount = magazineCount;
        fireRate = 1 / shotsPerSecond;
        magSize = magazingSize;
        range = weaponRange;
        damage = weaponDamage;

        // Tracks how many usable magazines you still have left.
        magsLeft = magCount;

        // Starts the weapon with a full magazine.
        bulletsInCurrentMag = magSize;
    }

    public void ReloadWeapon()
    {
        if (magsLeft > 0)
        {
            bulletsInCurrentMag = magSize;
            magsLeft--;
        }
        else
        {
            Debug.Log("You are out of magazines for this weapon. Find more ammo.");
        }
    }
}
