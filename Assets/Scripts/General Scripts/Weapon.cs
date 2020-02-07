using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Create New Weapon")]
public class Weapon : ScriptableObject
{
    // Used to control whether you can hold down mouse to fire or you have to click.
    public enum FireType
    {
        Single,
        Burst,
        FullAuto
    }

    #region variable-declaration

    // Refer to FireType enum (above).
    public FireType fireMode = FireType.Single;

    // How fast the gun shows (bullets per second).
    public float fireRate = 0f;

    // The number of bullets per magazine.
    public int magSize = 8;

    // The initial number of magazines in the weapon.
    public int magCount = 3;

    // How far the weapon can fire.
    public float range = 10f;

    // How much damage the weapon does.
    public int damage = 10;

    // How forceful the recoil of the weapon is.
    public float recoilForce = 5f;

    // The sound the weapon makes when you fire.
    public AudioClip weaponSound = null;

    // How many bullets you currently have in your magazine.
    [HideInInspector] public int bulletsInCurrentMag;

    // The number of magazines left in your weapon.
    [HideInInspector] public int magsLeft;

    #endregion

    /// <summary>
    /// Determines whether you can fire, using the current bullets in your gun,
    /// the fire rate and the magazine size to do so.
    /// </summary>
    /// <param name="currentTime"></param>
    /// <returns>True if you can fire, and false if you cannot.</returns>
    public bool CanFire(float currentTime)
    {
        if (currentTime > fireRate)
        {
            if (magSize > 0)
            {
                if (bulletsInCurrentMag > 0)
                {
                    return true;
                }

                Debug.Log("You need to reload.");
            }
        }

        return false;
    }

    /// <summary>
    /// Deducts the amount of magazines you have left, and refills the bullets in your
    /// current magazine. Prints a message if you have no more magazines.
    /// </summary>
    public void Reload()
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
