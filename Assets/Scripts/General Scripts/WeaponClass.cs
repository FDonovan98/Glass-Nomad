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

    protected int bulletsInCurrentMag;

    // Minimum time delay between each shot.
    public float fireRate;
    // Max number of bullets stored in a magazine.
    private int magSize;
    public float range;
    public int damage;

    public WeaponClass(int magazineCount, float shotsPerSecond, int magazingSize, float weaponRange, int weaponDamage)
    {
        magCount = magazineCount;
        fireRate = 1 / shotsPerSecond;
        magSize = magazingSize;
        range = weaponRange;
        damage = weaponDamage;

        // Starts the weapon with a full magazine.
        bulletsInCurrentMag = magSize;
    }
}
