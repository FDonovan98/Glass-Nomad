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
    private float fireRate;
    // Max number of bullets stored in a magazine.
    private int magSize;
    private float range;
    private int damage;

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

    [PunRPC]
    protected void FireWeapon(ref float deltaTime, GameObject camera)
    {
        if (canFire(deltaTime))
        {
            deltaTime -= fireRate;

            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
            {
                PlayerAttack hitPlayer = hit.transform.gameObject.GetComponent<PlayerAttack>();
                if (hitPlayer != null)
                {
                    PlayerHealth hitPlayerHealth = hitPlayer.healthScript;

                    hitPlayerHealth.PlayerHit(damage);
                    hitPlayer.healthSlider.fillAmount = hitPlayerHealth.fillAmount;
                }
            }
        }
    }

    private bool canFire(float deltaTime)
    {
        if (deltaTime > fireRate)
        {
            return true;
        }

        return false;
    }
}
