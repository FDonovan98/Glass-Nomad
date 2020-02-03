using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System;

public class PlayerAttack : MonoBehaviourPunCallbacks
{
    // Used to change the health bar slider above the player.
    [SerializeField]
    public Image healthSlider = null;

    // Used to set the player's health the max, on initialisation.
    [SerializeField]
    private int maxHealth = 100;

    // Used to control the health of this player.
    public PlayerHealth healthScript;
    // Used to disable/enable the camera so that we only control our local player's camera.
    private GameObject cameraGO;

    private float deltaTime = 0.0f;
    public Weapon currentWeapon;
    private float recoil = 0f;
    private float recoil_rotation = 0f;

    private MuzzleFlashScript muzzleFlash;
    private Vector3 muzzleFlashPosition;
    private Light flashlight;
	private UIBehaviour hudCanvas;

    public GameObject bulletHolePrefab; // Spawned when a bullet hits a wall.

    // Oxygen shenanigans
    public float maxOxygenAmountSeconds = 300f;
    public float oxygenAmountSeconds;
    private float oxygenDamageTime = 0f;

    private new void OnEnable()
    {
        healthScript = new PlayerHealth(this.gameObject, maxHealth);
    }
    private void Start()
    {
        // The muzzle flash will appear at the same spot as the flashlight
        flashlight = gameObject.GetComponentInChildren<Light>();
        if (flashlight != null)
        {
            muzzleFlash = new MuzzleFlashScript();
        }

        // Gets the camera child on the player.
        cameraGO = this.GetComponentInChildren<Camera>().gameObject;
        deltaTime = currentWeapon.fireRate;

        oxygenAmountSeconds = maxOxygenAmountSeconds;

        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
        hudCanvas.UpdateUI(gameObject.GetComponent<PlayerAttack>());
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        deltaTime += Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            if (canFire(deltaTime, currentWeapon))
            {
                // Calls the 'Attack' method on all clients, meaning that the health will be synced across all clients.
                photonView.RPC("FireWeapon", RpcTarget.All, cameraGO.transform.position, cameraGO.transform.forward, currentWeapon.range, currentWeapon.damage);

                // If magSize is zero then it is a melee attack.
                if (currentWeapon.magSize > 0)
                {
                    currentWeapon.bulletsInCurrentMag--;
                    recoil += currentWeapon.recoilForce;

                    if (flashlight != null)
                    {
                        muzzleFlashPosition = flashlight.gameObject.transform.position;
                    }
                }

                if (muzzleFlash != null)
                {
                    StartCoroutine(muzzleFlash.Flash(muzzleFlashPosition, flashlight.gameObject.transform.rotation));
                }

                deltaTime = 0;
            }
        }

        if (recoil > 0)
        {
            RecoilWeapon();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadWeapon(currentWeapon);
        }

        // Reduce oxygen
        if (oxygenAmountSeconds > 0)
        {
            oxygenAmountSeconds -= Time.fixedDeltaTime;
        }
        if (oxygenAmountSeconds == 0)
        {
            if (oxygenDamageTime >= 0.2f)
            {
                healthScript.PlayerHit(1);
                oxygenDamageTime = 0f;
            }
            else
            {
                oxygenDamageTime += Time.fixedDeltaTime;
            }
        }

        hudCanvas.UpdateUI(gameObject.GetComponent<PlayerAttack>());
    }

    private void ReloadWeapon(Weapon weapon)
    {
        if (weapon.magsLeft > 0)
        {
            weapon.bulletsInCurrentMag = weapon.magSize;
            weapon.magsLeft--;
        }
        else
        {
            Debug.Log("You are out of magazines for this weapon. Find more ammo.");
        }
    }

    private void RecoilWeapon()
    {
        float xRotation = cameraGO.transform.localEulerAngles.x;
        recoil *= 10 * Time.deltaTime; // this dampens the recoil until it is (almost) zero.
        recoil_rotation += recoil;
        recoil_rotation *= 0.95f; // get smaller every frame.
        cameraGO.transform.localEulerAngles = new Vector3(xRotation - recoil_rotation, cameraGO.transform.localEulerAngles.y, cameraGO.transform.localEulerAngles.z);
    }

    private bool canFire(float deltaTime, Weapon weapon)
    {
        if (deltaTime > weapon.fireRate)
        {
            if (weapon.magSize > 0)
            {
                if (weapon.bulletsInCurrentMag > 0)
                {
                    return true;
                }
                else
                {
                    Debug.Log("You are out of bullets in your magazine.");
                }
                return false;
            }  
            return true; 
        }
        
        return false;
        
    }

    [PunRPC]
    protected void FireWeapon(Vector3 cameraPos, Vector3 cameraForward, float range, int damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraForward, out hit, range))
        {
            PlayerAttack hitPlayer = hit.transform.gameObject.GetComponent<PlayerAttack>();
            if (hitPlayer != null) // A player was hit
            {
                PlayerHealth hitPlayerHealth = hitPlayer.healthScript;

                hitPlayerHealth.PlayerHit(damage);
                hitPlayer.healthSlider.fillAmount = hitPlayerHealth.fillAmount;
            }
            else // A wall was hit.
            {
                // It works somehow... don't ask.
                int temp = hit.normal.z == -1 ? 2 : 0;
                int temp1 = hit.normal.x != 0 ? 2 : 0;
                Vector3 holeSpawn = new Vector3(-1 + temp + hit.normal.y, temp1 + hit.normal.x, 0) * -90;
                GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + (hit.normal * 0.001f), Quaternion.Euler(holeSpawn));
                StartCoroutine(FadeBulletOut(bulletHole, 1f));
                Destroy(bulletHole, 1f);
            }
        }
    }

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
