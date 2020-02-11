using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System;

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

    // Used to control the rate of fire, reload time, audio clip, etc.
    public Weapon currentWeapon;

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

    #endregion

    /// <summary>
    /// Initialises the players resources. 
    /// </summary>
    private new void OnEnable()
    {
        resourcesScript = new PlayerResources(this.gameObject, maxHealth);
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
        weaponAudio.clip = currentWeapon.weaponSound;
        currentWeapon.magsLeft = currentWeapon.magCount;
        currTimeBetweenFiring = currentWeapon.fireRate;

        resourcesScript.hudCanvas.UpdateUI(GetComponent<PlayerAttack>());
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (currTimeBetweenFiring <= currentWeapon.fireRate) currTimeBetweenFiring += Time.deltaTime;

        if (currentWeapon.CanFire(currTimeBetweenFiring))
        {
            if (currentWeapon.fireMode == Weapon.FireType.Single)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else if (currentWeapon.fireMode == Weapon.FireType.FullAuto)
            {
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                }
            }

            currTimeBetweenFiring = 0;
        }

        if (recoil > 0) RecoilWeapon();

        if (Input.GetKeyDown(KeyCode.R)) currentWeapon.Reload();

        ReduceOxygen();
    }

    /// <summary>
    /// Calls the PunRPC 'FireWeapon' so that every clients registers the fire, and takes damage accordingly.
    /// This also reduces the players ammo count and adds recoil, as well as spawning the muzzle flash.
    /// </summary>
    private void Shoot()
    {
        // Calls the 'FireWeapon' method on all clients, meaning that the health and gun shot will be synced across all clients.
        photonView.RPC("FireWeapon", RpcTarget.All, cameraGO.transform.position, cameraGO.transform.forward,
                    currentWeapon.range, currentWeapon.damage);

        recoil += currentWeapon.recoilForce;
        currentWeapon.bulletsInCurrentMag--;

        if (muzzleFlash != null)
        {
            StartCoroutine(muzzleFlash.Flash(flashlight.gameObject.transform.position, flashlight.gameObject.transform.rotation));
        }
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
    protected void FireWeapon(Vector3 cameraPos, Vector3 cameraForward, float range, int damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraForward, out hit, range))
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
                BulletHole(hit);                
            }
        }

        // Play a gunshot sound.
        if (weaponAudio.clip != null) weaponAudio.Play();
    }

    private void BulletHole(RaycastHit hit)
    {
        if (GetComponent<MarineController>() != null) // If this is the marine shooting...
        {
            int temp = hit.normal.z == -1 ? 2 : 0;
            int temp1 = hit.normal.x != 0 ? 2 : 0;
            Vector3 holeSpawn = new Vector3(-1 + temp + hit.normal.y, temp1 + hit.normal.x, 0) * -90;
            GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + (hit.normal * 0.001f), Quaternion.Euler(holeSpawn));
            StartCoroutine(FadeBulletOut(bulletHole, 1f));
            Destroy(bulletHole, 1f);
        }
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
