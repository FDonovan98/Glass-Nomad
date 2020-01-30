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

    public FireType fireMode = FireType.Single;
    public int magCount = 3;
    public float fireRate = 0f;
    public int magSize = 8;
    public float range = 10f;
    public int damage = 10;
    public float recoilForce = 5f;
    public AudioClip weaponSound = null;

    // These are hidden in the inspector as they don't need to be set.
    [HideInInspector] public int magsLeft;
    [HideInInspector] public int bulletsInCurrentMag;

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
