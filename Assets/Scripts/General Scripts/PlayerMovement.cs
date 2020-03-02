using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    // The distance that the step is detected by the player.
    [SerializeField]
    private float distanceBetweenStep = 2f;

    // The upward force applied to the player when on stairs.
    [SerializeField]
    private float upForce = 1.5f;

    // Should the debug rays and console messages be shown.
    [SerializeField]
    protected bool debug = false;

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

    // How much force should be applied randomly to player upon death.
    [SerializeField]
    private float deathForce = 150f;

    // The gravity scale that's applied to the player.
    [SerializeField]
    protected float gravity = -100;

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

    // The characters normal.
    protected Vector3 charNormal;

    // Enables/disables the players input.
    public bool inputEnabled = true;

    private float distanceBetweenPlayerAndStep = 0;

    protected void Start()
    {
        InitialiseGlobals();

        if (!photonView.IsMine)
        {
            // Disables the camera on every client that isn't our own.
            charCamera.GetComponent<AudioListener>().enabled = false; // Disables the audio listener on every client that isn't our own.
            charCamera.GetComponent<Camera>().enabled = false; 
        }

        // Forces every player's mouse to the center of the window and hides it when the player is created.
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    private void InitialiseGlobals()
    {
        charNormal = Vector3.up;
        if (debug) Debug.Log(charNormal);
        // Sets the gameobject name to the player's username.
        gameObject.name = photonView.Owner.NickName; 

        charCamera = gameObject.GetComponentInChildren<Camera>(); 
        charCollider = gameObject.GetComponent<Collider>();
        distGround =  charCollider.bounds.extents.y;
        charRigidbody = gameObject.GetComponent<Rigidbody>(); 

        menu = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().pauseMenu;

        charCameraTargetRotation = charCamera.transform.localRotation;
    }

    protected void Update()
    {
        if (!photonView.IsMine) return;

        HandlePauseMenu();

        // If input is enabled, ignore player and camera rotation.
        if (!inputEnabled || Cursor.lockState == CursorLockMode.None) return;

        HandlePlayerRotation();

        // If there is a step, and its height is correct, then try and apply force.
        if (CheckStepHeight())
        {
            ApplyUpwardsForce();
        }

        if (debug) Debugging();
    }

    /// <summary>
    /// Rotates the players gameobject around the y-axis, and the players camera
    /// around the x-axis.
    /// </summary>
    protected void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        charRigidbody.velocity += gravity * charNormal * Time.fixedDeltaTime;
    }

    private void HandlePlayerRotation()
    {
        Vector3 mouseRotationInput = GetMouseInput(); 

        // Player rotation
        Vector3 playerRotation = new Vector3(0, mouseRotationInput.x, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        // Camera rotation
        float cameraRotation = -mouseRotationInput.y * mouseSensitivity;
        charCameraTargetRotation = charCamera.transform.localRotation;
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
                if (Cursor.lockState == CursorLockMode.Locked) ToggleCursorAndMenu(true);
                else ToggleCursorAndMenu(false);
            } 
        #elif UNITY_STANDALONE_WIN
            //Press the Escape key to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.lockState == CursorLockMode.Locked) ToggleCursorAndMenu(true);
                else ToggleCursorAndMenu(false);
            } 
        #endif
    }
    
    private void ToggleCursorAndMenu(bool turnOn)
    {
        Cursor.lockState = turnOn ? CursorLockMode.None : CursorLockMode.Locked;
        ToggleMenu(turnOn);
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
    }

    /// <summary>
    /// Disables the player's input, enables rotations in the rigidbody, adds a random force to the
    /// rigidbody, and starts the 'Death' coroutine.
    /// </summary>
    public void Ragdoll()
    {
        inputEnabled = false;
        charRigidbody.constraints = RigidbodyConstraints.None;
        charRigidbody.AddForceAtPosition(RandomForce(deathForce), transform.position);
        StartCoroutine(Death(gameObject));
    }

    /// <summary>
    /// Returns a vector with all axes having a random value between 0 and the 'velocity' parameter.
    /// </summary>
    /// <param name="velocity">The maximum random force.</param>
    /// <returns>Returns a vector with all axes having a random value between 0 and the 'velocity' parameter.</returns>
    private Vector3 RandomForce(float velocity)
    {
        return new Vector3(Random.Range(0, velocity), Random.Range(0, velocity), Random.Range(0, velocity));
    }

    private IEnumerator Death(GameObject player)
    {
        yield return new WaitForSeconds(3f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.Destroy(player);
        if (photonView.IsMine) PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// Retrieves the player's WASD input, translating the transform of the player.
    /// Also multiplies the speed if the player is sprinting.
    /// </summary>
    public virtual Vector3 GetPlayerInput()
    {
        float x, z;

        // Player movement
        x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        z = Input.GetAxisRaw("Vertical") * movementSpeed;

        if (Input.GetAxis("Sprint") >= 1)
        {
            x *= sprintSpeedMultiplier;
            z *= sprintSpeedMultiplier;
        }   

        return new Vector3(x, charRigidbody.velocity.y, z);
    }

    #region stairs

    /// <summary>
    /// Casts a ray from the middle of the player, downwards, to check for the step height,
    /// and for the step's normal.
    /// </summary>
    /// <returns>True if the player can walk the step's height, false if not.</returns>
    private bool CheckStepHeight()
    {
        // If the player isn't grounded, then force has (presumably) already been applied.
        Vector3 frontOfPlayer = transform.position;
        frontOfPlayer += transform.forward * charCollider.bounds.extents.z;
        if (!IsGrounded(frontOfPlayer, -Vector3.up)) return false;

        // End the ray on the floor, ahead of the player.
        Vector3 endDir = transform.position;
        endDir.y -= charCollider.bounds.extents.y;
        endDir += transform.forward * (charCollider.bounds.extents.z + (distanceBetweenStep / 2f));

        // Cast the ray and output it to the hitInfo.
        RaycastHit hitInfo;
        bool isThereAStep = Physics.Raycast(frontOfPlayer, endDir - frontOfPlayer, out hitInfo, Vector3.Distance(frontOfPlayer, endDir) - 0.1f);
        if (debug) Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.cyan);
        if (debug) Debug.Log("IS THERE A STEP: " + isThereAStep);

        distanceBetweenPlayerAndStep = Vector3.Distance(frontOfPlayer, hitInfo.point) < 1.5f ? 1.5f : Vector3.Distance(frontOfPlayer, hitInfo.point);

        // If the step height is correct and the step's normal is the worlds up axis then return true.
        return isThereAStep && hitInfo.normal == Vector3.up; // ** THIS LINE MAY HAVE BROKEN IT **
    }

    /// <summary>
    /// Applies a force upwards, to make the player be able to traverse steps.
    /// </summary>
    private void ApplyUpwardsForce()
    {
        // If there isn't a force already being applied upwards on the player, then...
        if (charRigidbody.velocity.y < upForce)
        {
            // If the player is pressing forward key ('W')...
            if (GetPlayerInput().z > 0)
            {
                // Apply an upwards force onto the player's rigidbody.
                charRigidbody.velocity += transform.up * upForce * charRigidbody.mass * (1 / distanceBetweenPlayerAndStep);
            }
        }
    }

    /// <summary>
    /// Provides debugging rays and logs for the stair mechanics.
    /// </summary>
    private void Debugging()
    {
        // Used to check the distance betweent the players feet and the step.
        Vector3 playerFeet = transform.position;
        playerFeet.y -= charCollider.bounds.extents.y;
        Debug.DrawRay(playerFeet, transform.forward * (charCollider.bounds.extents.z + distanceBetweenStep), Color.magenta);

        // Used to check how steep the step is, and its height.
        Vector3 startDir = transform.position;
        startDir += transform.forward * charCollider.bounds.extents.z;
        Vector3 endDir = transform.position;
        endDir.y -= charCollider.bounds.extents.y;
        endDir += transform.forward * (charCollider.bounds.extents.z + (distanceBetweenStep / 2f));
        Debug.DrawRay(startDir, endDir - startDir, Color.red);

        // Used to check is the player is on the ground.
        Vector3 frontOfPlayer = transform.position;
        frontOfPlayer += transform.forward * charCollider.bounds.extents.z;
        Debug.DrawRay(frontOfPlayer, -Vector3.up * (charCollider.bounds.extents.y + 0.5f), Color.green);

        Debug.Log("IS GROUNDED: " + IsGrounded(frontOfPlayer, -Vector3.up));
        Debug.Log("STEP HEIGHT LOW ENOUGH: " + CheckStepHeight() + ", " + distanceBetweenPlayerAndStep);
    }

    #endregion
}