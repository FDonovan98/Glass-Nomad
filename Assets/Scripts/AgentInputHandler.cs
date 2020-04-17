using UnityEngine;
using Photon.Pun;
using TMPro;

using System.Collections.Generic;

public class AgentInputHandler : MonoBehaviourPunCallbacks
{
    public GameObject pauseMenu;
    public Behaviour behaviourToToggle;
    public AgentValues agentValues;
    public ActiveCommandObject[] activeCommands;
    public PassiveCommandObject[] passiveCommands;

    [Header("Check If Grounded")]
    public bool isGrounded = true;
    public ContactPoint groundContactPoint = new ContactPoint();

    [Header("Movement")]
    public bool isSprinting = false;
    public Vector3 gravityDirection = Vector3.down;
    public bool allowInput = true;
    [ReadOnly]
    public float currentLeapCharge = 0.0f;
    [ReadOnly]
    public bool isJumping = false;

    [Header("Stairs")]
    [ReadOnly]
    public Vector3 lastVelocity = Vector3.zero;

    [Header("Oxygen")]
    [ReadOnly]
    public float currentOxygen = 0.0f;
    public GameObject oxygenDisplay;

    [Header("Weapons")]
    public Weapon currentWeapon;
    [ReadOnly]
    public int currentBulletsInMag = 0;
    [ReadOnly]
    public float timeSinceLastShot = 0.0f;
    [ReadOnly]
    public int currentTotalAmmo = 0;
    [ReadOnly]
    public float currentRecoilValue = 0.0f;
    public GameObject weaponObject;
    public ParticleSystem weaponMuzzleFlash;
    public Weapon[] equipedWeapons = new Weapon[2];
    public int currentWeaponID = 0;

    [Header("Camera")]
    public Camera agentCamera;

    [Header("ADS")]
    public Camera mainCamera;
    public Camera aDSCamera;
    public bool isADS = false;

    [Header("Health")]
    [ReadOnly]
    public float currentHealth = 0.0f;

    [Header("UI")]
    public Canvas HUDCanvas;
    public GameObject ADSReticule;
    public TextMeshProUGUI healthUIText;
    public TextMeshProUGUI ammoUIText;

    [Header("UI Offset")]
    public GameObject HUD;
    [ReadOnly]
    public Vector3 UIOffset = Vector3.zero;

    [Header("Agent Hit Feedback")]
    public AudioClip agentHitSound;
    public GameObject agentHitParticles;

    [Header("PUN")]
    public PunRPCs punRPCs;
    [ReadOnly]
    public bool isLocalAgent = true;

    protected GameObject agent;

    // Delegates used by commands.
    // Should add a delegate for UpdateUI(GameObject UIToUpdate, float newValue = 0.0f, int newIntValue = 0), maybe.
    public delegate void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues);
    public RunCommandOnUpdate runCommandOnUpdate;
    public delegate void RunCommandOnFixedUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues);
    public RunCommandOnFixedUpdate runCommandOnFixedUpdate;
    public delegate void RunCommandOnCollisionEnter(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collision other);
    public RunCommandOnCollisionStay runCommandOnCollisionEnter;
    public delegate void RunCommandOnCollisionStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collision other);
    public RunCommandOnCollisionStay runCommandOnCollisionStay;
    public delegate void RunCommandOnCollisionExit(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collision other);
    public RunCommandOnCollisionExit runCommandOnCollisionExit;
    public delegate void RunCommandOnTriggerStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other);
    public RunCommandOnTriggerStay runCommandOnTriggerStay;


    public delegate void RunCommandOnWeaponFired(AgentInputHandler agentInputHandler);
    public RunCommandOnWeaponFired runCommandOnWeaponFired;
    public delegate void RunCommandOnAgentHasBeenHit(AgentInputHandler agentInputHandler, Vector3 position, Vector3 normal, float value);
    public RunCommandOnAgentHasBeenHit runCommandOnAgentHasBeenHit;
    public delegate void RunCommandOnCameraMovement(Vector3 cameraMovement, AgentInputHandler agentInputHandler, AgentValues agentValues);
    public RunCommandOnCameraMovement runCommandOnCameraMovement;


    private void Start()
    {
        InitiliseVariable();

        InitiliseUI();

        foreach (ActiveCommandObject element in activeCommands)
        {
            element.RunCommandOnStart(this);
        }
        foreach (PassiveCommandObject element in passiveCommands)
        {
            element.RunCommandOnStart(this);
        }
    }

    private void Update()
    {
        if (runCommandOnUpdate != null)
        {
            runCommandOnUpdate(agent, this, agentValues);
        }
    }

    private void FixedUpdate()
    {
        if (runCommandOnFixedUpdate != null)
        {
            runCommandOnFixedUpdate(agent, this, agentValues);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (runCommandOnCollisionEnter != null)
        {
            runCommandOnCollisionEnter(agent, this, agentValues, other);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (runCommandOnCollisionStay != null)
        {
            runCommandOnCollisionStay(agent, this, agentValues, other);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (runCommandOnCollisionExit != null)
        {
            runCommandOnCollisionExit(agent, this, agentValues, other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (runCommandOnTriggerStay != null)
        {
            runCommandOnTriggerStay(agent, this, agentValues, other);
        }
    }

    void InitiliseVariable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        agent = this.gameObject;

        if (agentValues != null)
        {
            currentOxygen = agentValues.maxOxygen;
            currentHealth = agentValues.maxHealth;
        }

        if (currentWeapon != null)
        {
            currentBulletsInMag = currentWeapon.bulletsInCurrentMag;
            currentTotalAmmo = currentWeapon.magSize * 3;
            timeSinceLastShot = currentWeapon.fireRate;
        }
    }

    void InitiliseUI()
    {
        if (healthUIText != null)
        {
            healthUIText.text = "Health: " + Mathf.RoundToInt(currentHealth / agentValues.maxHealth * 100);
        }

        if (ammoUIText != null)
        {
            ammoUIText.text =  currentBulletsInMag + " / " + currentTotalAmmo;
        }
    }
}