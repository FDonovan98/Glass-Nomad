using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System;
using System.Linq;

public class PlayerAttack : MonoBehaviourPunCallbacks
{
    #region variable-declaration

    // Used to set the player's health the max, on initialisation.
    [SerializeField] private int maxHealth = 100;

    // Used to change the health bar slider above the player.
    public Image healthSlider = null;

    // Used to control the health of this player.
    public PlayerResources resourcesScript;

    // Used to disable/enable the camera so that we only control our local player's camera.
    private GameObject cameraGO;

    // Used to play the current weapons audio clip.
    private AudioSource weaponAudio;

    // Both used to recoil the players weapon.
    private float recoil = 0f;
    private float recoilRotation = 0f;

    // Used to keep track of how long it has been since the weapon was last fired.
    private float currTimeBetweenFiring = 0.0f;

    // Used to spawn a muzzle flash when a player shoots.
    private MuzzleFlashScript muzzleFlash = null;

    // Used to position the muzzle flash.
    private Light flashlight;

    // Used to display the players current ammo, mag count, and oxygen.
    private UIBehaviour hudCanvas;

    // Spawned when a bullet hits a wall.
    public GameObject bulletHolePrefab;
    public GameObject bulletRicochetSpark;

    // Name for the weapon of the alien.
    private string alienWeapon = "Claws";

    #endregion

    /// <summary>
    /// Initialises the players resources. 
    /// </summary>
    private new void OnEnable()
    {
        resourcesScript = new PlayerResources(this.gameObject, maxHealth);
        AllocatePlayersItems();
    }

    /// <summary>
    /// Assigns the flashlight, camera, weapon audio and HUD canvas.
    /// </summary>
    private void Start()
    {        
        // The muzzle flash will appear at the same spot as the flashlight
        flashlight = gameObject.GetComponentInChildren<Light>();


        // Gets the camera child on the player.
        cameraGO = this.GetComponentInChildren<Camera>().gameObject;

        weaponAudio = cameraGO.GetComponentInChildren<AudioSource>();
        weaponAudio.clip = resourcesScript.currentWeapon.weaponSound;
        resourcesScript.currentWeapon.bulletsInCurrentMag = resourcesScript.currentWeapon.magSize;
        resourcesScript.currentWeapon.magsLeft = resourcesScript.currentWeapon.magCount;
        currTimeBetweenFiring = resourcesScript.currentWeapon.fireRate;

        resourcesScript.hudCanvas.UpdateUI(GetComponent<PlayerAttack>());
    }

    private void AllocatePlayersItems()
    {
        BaseObject[] baseObjects = Resources.LoadAll("Items", typeof(BaseObject)).Cast<BaseObject>().ToArray();
        string primary = PlayerPrefs.GetString("Primary");
        string secondary = PlayerPrefs.GetString("Secondary");
        string armour = PlayerPrefs.GetString("Armour");

        //Layer 8 is MarineCharacter
        if (gameObject.layer == 8)
        {
            BaseObject prim = baseObjects.Where(a => a.name == primary).FirstOrDefault();
            resourcesScript.currentWeapon = (Weapon)prim;
        }
        //Layer 9 is AlienCharacter
        //Currently bandage fix because couldn't immediately think of a better solution
        else if (gameObject.layer == 9)
        {
            BaseObject prim = baseObjects.Where(a => a.name == alienWeapon).FirstOrDefault();
            resourcesScript.currentWeapon = (Weapon)prim;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        if (!gameObject.GetComponent<PlayerMovement>().inputEnabled) return;

        if (currTimeBetweenFiring < resourcesScript.currentWeapon.fireRate) currTimeBetweenFiring += Time.deltaTime;

        if (resourcesScript.currentWeapon.CanFire(currTimeBetweenFiring))
        {
            if (resourcesScript.currentWeapon.fireMode == Weapon.FireType.Single)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else if (resourcesScript.currentWeapon.fireMode == Weapon.FireType.FullAuto)
            {
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                }
            }
        }

        if (recoil > 0) RecoilWeapon();

        if (Input.GetKeyDown(KeyCode.R)) resourcesScript.Reload();

        ReduceOxygen();
    }

    /// <summary>
    /// Calls the PunRPC 'FireWeapon' so that every clients registers the fire, and takes damage accordingly.
    /// This also reduces the players ammo count and adds recoil, as well as spawning the muzzle flash.
    /// </summary>
    private void Shoot()
    {
        // Calls the 'FireWeapon' method on all clients, meaning that the health and gun shot will be synced across all clients.
        photonView.RPC("FireWeapon", RpcTarget.All, cameraGO.transform.position, RandomBulletSpread(cameraGO.transform.rotation),
                    resourcesScript.currentWeapon.range, resourcesScript.currentWeapon.damage);

        recoil += resourcesScript.currentWeapon.recoilForce;
        resourcesScript.currentWeapon.bulletsInCurrentMag--;
        currTimeBetweenFiring = 0;

        if (muzzleFlash != null)
        {
            StartCoroutine(muzzleFlash.Flash(flashlight.gameObject.transform.position, flashlight.gameObject.transform.rotation));
        }
    }

    private Vector3 RandomBulletSpread(Quaternion cameraRot)
    {
        Vector3 deviation3D = UnityEngine.Random.insideUnitCircle * resourcesScript.currentWeapon.maxBulletSpread;
        Quaternion rot = Quaternion.LookRotation(Vector3.forward * resourcesScript.currentWeapon.range + deviation3D);
        return cameraRot * rot * Vector3.forward;
    }

    /// <summary>
    /// Recoils the players camera when they shoot.
    /// </summary>
    private void RecoilWeapon()
    {
        float xRotation = cameraGO.transform.localEulerAngles.x;
        recoil *= 10 * Time.deltaTime; // This dampens the recoil until it is (almost) zero.
        recoilRotation += recoil;
        recoilRotation *= 0.95f; // Gets smaller every frame.
        cameraGO.transform.localEulerAngles = new Vector3(xRotation - recoilRotation, cameraGO.transform.localEulerAngles.y, cameraGO.transform.localEulerAngles.z);
    }

    private void ReduceOxygen()
    {
        //Layer 8 is MarineCharacter. This time Harry didn't have to tell me to put this here. Oh how far I've come.
        if (gameObject.layer == 8)
        {
            if (resourcesScript.oxygenAmountSeconds > 0)
            {
                resourcesScript.UpdatePlayerResource(PlayerResources.PlayerResource.OxygenLevel, -Time.fixedDeltaTime);
            }
        }
    }

    /// <summary>
    /// Plays a gun shot sound, so that all clients hear it. If the bullet hits a player,
    /// then the hit player's health is reduced, otherwise, if the bullet hit a wall, then
    /// a bullet hole is instantiated and faded out.
    /// </summary>
    /// <param name="cameraPos"></param>
    /// <param name="cameraForward"></param>
    /// <param name="range"></param>
    /// <param name="damage"></param>
    [PunRPC]
    protected void FireWeapon(Vector3 cameraPos, Vector3 bulletDir, float range, int damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, bulletDir, out hit, range))
        {
            PlayerAttack hitPlayer = hit.transform.gameObject.GetComponent<PlayerAttack>();
            if (hitPlayer != null && hitPlayer.gameObject != this.gameObject) // A player was hit
            {
                PlayerResources hitPlayerResources = hitPlayer.resourcesScript;

                hitPlayerResources.UpdatePlayerResource(PlayerResources.PlayerResource.Health, -damage);
                hitPlayer.healthSlider.fillAmount = hitPlayerResources.fillAmount;
            }
            else // A wall was hit.
            {
                Vector3[] effectSpawnPos = CalculateEffectSpawnPos(hit);
                BulletHole(effectSpawnPos); 

                if (hit.transform.gameObject.tag != "Not Metal")
                {
                    RicochetVisual(effectSpawnPos);     
                }          
            }
        }

        // Play a gunshot sound.
        if (weaponAudio.clip != null) weaponAudio.Play();
    }

    private Vector3[] CalculateEffectSpawnPos(RaycastHit hit)
    {
        int temp = hit.normal.z == -1 ? 2 : 0;
        int temp1 = hit.normal.x != 0 ? 2 : 0;
        Vector3 spawnRotation = new Vector3(-1 + temp + hit.normal.y, temp1 + hit.normal.x, 0) * -90;
        Vector3 spawnPosition = hit.point + (hit.normal * 0.001f);
        return new Vector3[] {spawnPosition, spawnRotation};
    }

    private void BulletHole(Vector3[] spawnPos)
    {
        if (GetComponent<MarineController>() != null) // If this is the marine shooting...
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab, spawnPos[0], Quaternion.Euler(spawnPos[1]));
            StartCoroutine(FadeBulletOut(bulletHole, 1f));
            Destroy(bulletHole, 1f);
        }
    }

    private void RicochetVisual(Vector3[] spawnPos)
    {
        GameObject bulletSpark = Instantiate(bulletRicochetSpark, spawnPos[0], Quaternion.Euler(spawnPos[1]));
        Destroy(bulletSpark, 0.1f);
    }

    /// <summary>
    /// Used to fade out the bullet if it hits a wall.
    /// </summary>
    /// <param name="bullet"></param>
    /// <param name="fadeDuration"></param>
    /// <returns></returns>
    IEnumerator FadeBulletOut(GameObject bullet, float fadeDuration)
    {
        Color col = bullet.GetComponent<MeshRenderer>().material.color;
        float step = 1 / fadeDuration;

        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            bullet.GetComponent<MeshRenderer>().material.color = new Color(col.r, col.g, col.b, col.a * ((fadeDuration - t) / fadeDuration));
            yield return null;
        }
    }
}
