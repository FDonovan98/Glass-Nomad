using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    // The movement of the player.
    [SerializeField] 
    public float movementSpeed = 10;     
    
    // The sensitivity of the mouse.
    [SerializeField] 
    protected int mouseSensitivity = 1; 

    // The jumping force of the player.
    [SerializeField] 
    protected float jumpSpeed = 10; 

    // Stops the player looking 'underneath' themselves.
    [SerializeField] 
    protected float yRotationClamp = 30; 

    // Multiplies the players current speed, when sprinting.
    [SerializeField] 
    protected float sprintSpeedMultiplier = 1.5f;

    // Hides and shows the menu options.
    [SerializeField] 
    private GameObject menu = null; 

    // Applies physics to the player, e.g. movement.
    protected Rigidbody charRigidbody; 

    // Used for the ground raycast.
    protected float distGround; 

    // The characters collider.
    protected Collider charCollider;

    // Disables/enables the camera so that we only control our local player's camera.
    protected Camera charCamera; 
    
    // Stores rotation of the player and the camera.
    protected Vector3 mouseRotationInput; 

    // How far off the ground counts as 'grounded'.
    protected float groundDelta = 1.0f;

    // The x-axis rotation of the players camera.
    protected Quaternion charCameraTargetRotation;

    // Enables/disables the players input.
    protected bool inputEnabled = true;

    // Toggles the menu and cursor visibilty.
    private bool turnMenuOn = false;

    // The characters normal.
    public Vector3 charNormal;

    public float gravConstant = 10;
    public float gravity;


    protected void Start()
    {
        if (!photonView.IsMine)
        {
            // Disables the camera on every client that isn't our own.
            charCamera.GetComponent<Camera>().enabled = false; 
        }

        InitialiseGlobals();

        // Forces every player's mouse to the center of the window and hides it when the player is created.
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    private void InitialiseGlobals()
    {
        // Sets the gameobject name to the player's username.
        gameObject.name = photonView.Owner.NickName; 

        charCamera = gameObject.GetComponentInChildren<Camera>(); 
        charCollider = gameObject.GetComponent<Collider>();
        distGround =  charCollider.bounds.extents.y;
        charRigidbody = gameObject.GetComponent<Rigidbody>(); 

        menu = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().pauseMenu;

        charCameraTargetRotation = charCamera.transform.localRotation;

        // Initialises the charNormal to the world normal.
        charNormal = Vector3.up;
        gravity = gravConstant;
    }

    protected void Update()
    {
        HandlePauseMenu();

        // If input is enabled, ignore player and camera rotation.
        if (!inputEnabled) return;

        HandlePlayerRotation();
    }

    /// <summary>
    /// Rotates the players gameobject around the y-axis, and the players camera
    /// around the x-axis.
    /// </summary>

    protected void FixedUpdate()
    {
        // Calculate and apply force of gravity to char.
        Vector3 gravForce = -gravity * charRigidbody.mass * charNormal;
        charRigidbody.AddForce(gravForce);
    }

    private void HandlePlayerRotation()
    {
        Vector3 mouseRotationInput = GetMouseInput(); 

        // Player rotation
        Vector3 playerRotation = new Vector3(0, mouseRotationInput.x, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        // Camera rotation
        float cameraRotation = -mouseRotationInput.y * mouseSensitivity;
        charCameraTargetRotation *= Quaternion.Euler(cameraRotation, 0.0f, 0.0f);
        charCameraTargetRotation = ClampRotationAroundXAxis(charCameraTargetRotation);

        // Use of localRotation allows movement around y axis.
        charCamera.transform.localRotation = charCameraTargetRotation;
    }

    /// <summary>
    /// Retrieves the platform specific input for toggling the pause menu.
    /// </summary>
    private void HandlePauseMenu()
    {
        #if UNITY_EDITOR
            //Press the Comma key (,) to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                ToggleCursorAndMenu();
            } 
        #elif UNITY_STANDALONE_WIN
            //Press the Escape key to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCursorAndMenu();
            } 
        #endif
    }
    
    private void ToggleCursorAndMenu()
    {
        turnMenuOn = !turnMenuOn;
        Cursor.lockState = turnMenuOn ? CursorLockMode.None : CursorLockMode.Locked;
        ToggleMenu(turnMenuOn);
    }

    /// <summary>
    /// Retrieves the x and y input of the mouse and returns it as a Vector3.
    /// </summary>
    /// <returns>The mouse input as a Vector3.</returns>
    protected Vector3 GetMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        return new Vector3(mouseX, mouseY, 0);
    }

    /// <summary>
    /// Sends a raycast from 'origin' in the direction of 'dirOfRay'.
    /// Predominantly used to check if the player is on the floor.
    /// </summary>
    /// <param name="origin">The starting position for the raycast.</param>
    /// <param name="dirOfRay">The direction of the raycast.</param>
    /// <returns>True if the player is grounded, false if not.</returns>
    protected bool IsGrounded(Vector3 origin, Vector3 dirOfRay)
    {
        return Physics.Raycast(origin, dirOfRay, distGround + groundDelta);
    }

    /// <summary>
    /// Clamps the given quaternion within the value of the 'yRotationClamp' variable.
    /// </summary>
    /// <param name="q">The quaternion to clamp.</param>
    /// <returns>The clamped quaternion.</returns>
    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        // Quaternion is 4x4 matrix.
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -yRotationClamp, yRotationClamp);

        // Updates x.
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    /// <summary>
    /// Toggles the visibility of the pause menu.
    /// </summary>
    /// <param name="toggle">Whether to turn the pause menu on or off.</param>
    private void ToggleMenu(bool toggle)
    {
        menu.SetActive(toggle);
        Cursor.visible = toggle;
        inputEnabled = !toggle;
    }
}