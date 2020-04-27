using UnityEngine;
using Photon.Pun;
using TMPro;

using UnityEngine.UI;

public class AgentInputHandler : MonoBehaviourPunCallbacks
{
    public AgentController agentController;
    private AgentInputHandler attachedScript;
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
    [ReadOnly]
    public float moveSpeedMultiplier = 1.0f;
    public Rigidbody agentRigidbody;

    [Header("Stairs")]
    [ReadOnly]
    public Vector3 lastVelocity = Vector3.zero;

    [Header("Weapons")]
    public Weapon currentWeapon;
    [ReadOnly]
    public float timeSinceLastShot = 0.0f;
    [ReadOnly]
    public float currentRecoilValue = 0.0f;
    public GameObject weaponObject;
    public ParticleSystem weaponMuzzleFlash;
    public Weapon[] equipedWeapons = new Weapon[2];
    public int currentWeaponID = 0;

    [Header("Reloading")]
    public bool isReloading = false;

    [Header("Camera")]
    public Camera agentCamera;

    [Header("ADS")]
    public Camera mainCamera;
    public Camera aDSCamera;
    public bool isADS = false;
    public Canvas HUDCanvas;

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

    [Header("ObjectInteraction")]
    public TMP_Text interactionPromptText = null;
    public Image progressBar = null;

    public GameObject agent;

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
    public delegate void RunCommandOnTriggerEnter(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other);
    public RunCommandOnTriggerEnter runCommandOnTriggerEnter;
    public delegate void RunCommandOnTriggerStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other);
    public RunCommandOnTriggerStay runCommandOnTriggerStay;
    public delegate void RunCommandOnTriggerExit(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other);
    public RunCommandOnTriggerExit runCommandOnTriggerExit;


    public delegate void RunCommandOnWeaponFired(AgentInputHandler agentInputHandler);
    public RunCommandOnWeaponFired runCommandOnWeaponFired;
    public delegate void RunCommandOnAgentHasBeenHit(AgentInputHandler agentInputHandler, Vector3 position, Vector3 normal, float value);
    public RunCommandOnAgentHasBeenHit runCommandOnAgentHasBeenHit;
    public delegate void RunCommandOnCameraMovement(Vector3 cameraMovement, AgentInputHandler agentInputHandler, AgentValues agentValues);
    public RunCommandOnCameraMovement runCommandOnCameraMovement;


    private void Start()
    {
        if (agentController != null)
        {
            attachedScript = agentController;
        }
        else
        {
            attachedScript = this;
        }

        InitiliseVariable();

        foreach (ActiveCommandObject element in activeCommands)
        {
            element.RunCommandOnStart(attachedScript);
        }
        foreach (PassiveCommandObject element in passiveCommands)
        {
            element.RunCommandOnStart(attachedScript);
        }
    }

    private void Update()
    {
        if (runCommandOnUpdate != null)
        {
            runCommandOnUpdate(agent, attachedScript, agentValues);
        }
    }

    private void FixedUpdate()
    {
        if (runCommandOnFixedUpdate != null)
        {
            runCommandOnFixedUpdate(agent, attachedScript, agentValues);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (runCommandOnCollisionEnter != null)
        {
            runCommandOnCollisionEnter(agent, attachedScript, agentValues, other);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (runCommandOnCollisionStay != null)
        {
            runCommandOnCollisionStay(agent, attachedScript, agentValues, other);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (runCommandOnCollisionExit != null)
        {
            runCommandOnCollisionExit(agent, attachedScript, agentValues, other);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (runCommandOnTriggerEnter != null)
        {
            runCommandOnTriggerEnter(agent, attachedScript, agentValues, other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (runCommandOnTriggerStay != null)
        {
            runCommandOnTriggerStay(agent, attachedScript, agentValues, other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (runCommandOnTriggerExit != null)
        {
            runCommandOnTriggerExit(agent, attachedScript, agentValues, other);
        }
    }

    void InitiliseVariable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ChangeMovementSpeedModifier(float value, bool increase)
    {
        if (increase)
        {
            moveSpeedMultiplier *= value;
        }
        else
        {
            moveSpeedMultiplier /= value;
        }
    }
}