public class WeaponClass
{
    // Number of magazines for the weapon.
    public int magCount;

    protected int bulletsInCurrentMag;

    // Rate of fire in rounds per second.
    private float fireRate;
    // Max number of bullets stored in a magazine.
    private int magSize;
    private float range;
    private float damage;

    protected WeaponClass(int magazineCount, float rateOfFire, int magazingSize, float weaponRange, float weaponDamage)
    {
        magCount = magazineCount;
        fireRate = rateOfFire;
        magSize = magazingSize;
        range = weaponRange;
        damage = weaponDamage;

        // Starts the weapon with a full magazine.
        bulletsInCurrentMag = magSize;
    }
}
