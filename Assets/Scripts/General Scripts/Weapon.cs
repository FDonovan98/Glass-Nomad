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
}
