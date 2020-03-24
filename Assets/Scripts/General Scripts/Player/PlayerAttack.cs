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
    [SerializeField] private GameObject cameraGO;

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

    private float currentRecoilTimeStamp = 0.0f;

    private Animator anim;
    public AudioClip hitSound;


    #endregion

    /// <summary>
    /// Initialises the players resources. 
    /// </summary>
    private new void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
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
        bool recoilUp = false;
        if (!photonView.IsMine) return;

        //Reducing oxygen has to happen regardless of input being enabled otherwise oxygen will not go down when interacting with objects.
        ReduceOxygen();

        if (!gameObject.GetComponent<PlayerMovement>().inputEnabled) return;

        if (currTimeBetweenFiring < resourcesScript.currentWeapon.fireRate) currTimeBetweenFiring += Time.deltaTime;

        if (resourcesScript.currentWeapon.CanFire(currTimeBetweenFiring))
        {
            if (resourcesScript.currentWeapon.fireMode == Weapon.FireType.Single)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (resourcesScript.currentWeapon.name == "Shotgun")
                    {
                        int numberOfBulletsToFire = 6;
                        for (int i = 0; i < numberOfBulletsToFire; i++)
                        {
                            Shoot();
                            resourcesScript.currentWeapon.bulletsInCurrentMag++;
                        }
                        recoilUp = true;
                        resourcesScript.currentWeapon.bulletsInCurrentMag--;
                    }
                    else
                    {
                        Shoot();
                        recoilUp = true;
                    }
                    if (anim != null)
                    {
                        anim.SetTrigger("isShooting");
                    }
                }
            }
            else if (resourcesScript.currentWeapon.fireMode == Weapon.FireType.FullAuto)
            {
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                    recoilUp = true;
                    if (anim != null)
                    {
                        anim.SetTrigger("isShooting");
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            resourcesScript.Reload();
            if (anim != null)
            {
                anim.SetTrigger("isReloading");
            }
        }

        RecoilWeapon(recoilUp);
    }

    private void RecoilWeapon(bool forceWeaponUp)
    {
        AnimationCurve weaponRecoilCurveUp = resourcesScript.currentWeapon.recoilCurveUp;
        AnimationCurve weaponRecoilCurveDown = resourcesScript.currentWeapon.recoilCurveDown;
        
        float timeDelta;
        float valueDelta;

        if (forceWeaponUp)
        {
            timeDelta = resourcesScript.currentWeapon.upForceStep;
            valueDelta = weaponRecoilCurveUp.Evaluate(currentRecoilTimeStamp + timeDelta) - weaponRecoilCurveUp.Evaluate(currentRecoilTimeStamp);
        }
        else
        {
            timeDelta = -Time.deltaTime / resourcesScript.currentWeapon.downForceDuration;
            valueDelta = weaponRecoilCurveDown.Evaluate(currentRecoilTimeStamp + timeDelta) - weaponRecoilCurveDown.Evaluate(currentRecoilTimeStamp);
        }


        valueDelta *= -resourcesScript.currentWeapon.recoilForce;

        cameraGO.transform.Rotate(valueDelta, 0.0f, 0.0f);

        // Prevents index errors.
        currentRecoilTimeStamp += timeDelta;
        currentRecoilTimeStamp = Mathf.Clamp(currentRecoilTimeStamp, 0.0f, 1.0f);

        //Debug.Log(currentRecoilTimeStamp);
    }

    /// <summary>
    /// Calls the PunRPC 'FireWeapon' so that every clients registers the fire, and takes damage accordingly.
    /// This also reduces the players ammo count and adds recoil, as well as spawning the muzzle flash.
    /// </summary>
    private void Shoot()
    {     
        Vector3 bulletDir = RandomBulletSpread(cameraGO.transform.rotation);

        RaycastHit hit;
        // The shoot hit something
        if (Physics.Raycast(cameraGO.transform.position, bulletDir, out hit, resourcesScript.currentWeapon.range))
        {
            Vector3[] effectSpawnPos = CalculateEffectSpawnPos(hit);
            PlayerAttack hitPlayer = hit.collider.gameObject.GetComponent<PlayerAttack>();
            if (hitPlayer != null && hitPlayer.gameObject != this.gameObject) // A player was hit
            {
                // Calls the 'PlayerWasHit' method on all clients, meaning that the hit player's health will be updated on all clients.
                photonView.RPC("PlayerWasHit", RpcTarget.All, hit.collider.gameObject.GetPhotonView().ViewID, resourcesScript.currentWeapon.damage, effectSpawnPos);
                weaponAudio.clip = hitSound;
                weaponAudio.Play();
                Debug.Log("Player was hit");
            }
            else // A wall was hit
            {
                bool shouldRicochet = (hit.transform.gameObject.tag != "Not Metal") ? true : false;
                photonView.RPC("WallWasHit", RpcTarget.All, effectSpawnPos, shouldRicochet);
                Debug.Log("Wall was hit");
            }
        }
        
        Debug.DrawRay(cameraGO.transform.position, bulletDir * resourcesScript.currentWeapon.range, Color.cyan, 2f);

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

    private void ReduceOxygen()
    {
        //Layer 8 is MarineCharacter. This time Harry didn't have to tell me to put this here. Oh how far I've come.
        if (gameObject.layer == 8)
        {
            if (resourcesScript.oxygenAmountSeconds > 0)
            {
                if (Input.GetAxis("Sprint") >= 1)
                {
                    resourcesScript.UpdatePlayerResource(PlayerResources.PlayerResource.OxygenLevel, -Time.fixedDeltaTime * resourcesScript.sprintOxygenMultiplier);
                }
                else
                {
                    resourcesScript.UpdatePlayerResource(PlayerResources.PlayerResource.OxygenLevel, -Time.fixedDeltaTime);
                }
            }
        }
    }

    /// <summary>
    /// Plays a gun shot sound, so that all clients hear it. If the bullet hits a player,
    /// then the hit player's health is reduced, otherwise, if the bullet hit a wall, then
    /// a bullet hole is instantiated and faded out.
    /// </summary>
    /// <param name="hitPlayerViewID"></param>
    /// <param name="damage"></param>
    [PunRPC]
    protected void PlayerWasHit(int hitPlayerViewID, int damage, Vector3[] ricochetPos)
    {
        PlayerAttack hitPlayer = PhotonNetwork.GetPhotonView(hitPlayerViewID).GetComponent<PlayerAttack>();
        PlayerResources hitPlayerResources = hitPlayer.resourcesScript;

        hitPlayerResources.UpdatePlayerResource(PlayerResources.PlayerResource.Health, -damage);
        hitPlayer.healthSlider.fillAmount = hitPlayerResources.fillAmount;

        RicochetVisual(ricochetPos);

        // Play a gunshot sound.
        if (weaponAudio.clip != null) weaponAudio.Play();
    }

    [PunRPC]
    protected void WallWasHit(Vector3[] ricochetPos, bool ricochet)
    {
        BulletHole(ricochetPos); 

        if (ricochet) RicochetVisual(ricochetPos);

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
