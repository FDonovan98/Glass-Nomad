using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    #region variable-declaration

    // Used to control the movement movementSpeed of the player.
    [SerializeField] public float movementSpeed = 10;

    // Used to control the sensitivity of the mouse.
    [SerializeField] protected int mouseSensitivity = 1;

    // Used to control the jumping force of the player.
    [SerializeField] protected float jumpSpeed = 10;

    // Used to stop the player looking 'underneath' themselves.
    [SerializeField] protected float yRotationClamp = 30;

    // Multiplies the player's current speed, when sprinting.
    [SerializeField] protected float sprintSpeedMultiplier = 1.5f;

    // Hide and shows the menu options.
    [SerializeField] private GameObject menu = null;

    // Applies physics to the player, e.g. movement.
    protected Rigidbody charRigidbody;

    // Used for the ground raycast.
    protected float distGround;

    // Used to calculate the distance to the ground.
    protected Collider charCollider;

    // Used to disable/enable the camera so that we only control our local player's camera.
    protected Camera charCamera;

    // Stores the rotation of the player and the camera.
    protected Vector3 mouseRotationInput;

    // Ensures the ground raycast is in contact with the ground.
    protected float groundDelta = 1.0f;

    // Both used to rotate the camera.
    protected float cameraRotation = 0f;
    protected Quaternion charCamTarRot;

    // Used to ignore the player's movement and mouse look.
    protected bool inputEnabled = true;

    #endregion

    /// <summary>
    /// Assigns variables and alters the state of the cursor, when the player is spawned.
    /// Disables the audio listener and camera of all other players.
    /// </summary>
    protected void Start()
    {
        // Sets the gameobject name to the player's username.
        gameObject.name = photonView.Owner.NickName;
        charCamera = gameObject.GetComponentInChildren<Camera>();
        charCollider = gameObject.GetComponent<Collider>();
        distGround =  charCollider.bounds.extents.y;
        charRigidbody = gameObject.GetComponent<Rigidbody>();
        menu = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().pauseMenu;

        //Cursor starts off locked to the center of the game window and invisible.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!photonView.IsMine)
        {
            charCamera.GetComponent<AudioListener>().enabled = false; // Disables the audio listener on every client that isn't our own.
            charCamera.GetComponent<Camera>().enabled = false; // Disables the camera on every client that isn't our own.
        }

        charCamTarRot = charCamera.transform.localRotation;
    }

    /// <summary>
    /// Retrieves the comma and escape input, and toggles the menu is one of the buttons
    /// is pressed. The comma is used for in-editor playing, and the escape key is used
    /// in built versions of the game.
    /// Mouse input is also retrieved, which rotates the player and their camera.
    /// </summary>
    protected void Update()
    {
#if UNITY_EDITOR
        // Press the Comma key (,) to unlock the cursor. If it's unlocked, lock it again
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                ToggleMenu(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.lockState == CursorLockMode.None)
            {
                ToggleMenu(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
#elif UNITY_STANDALONE_WIN
        // Press the Escape key to unlock the cursor. If it's unlocked, lock it again
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeSelf) // Menu is open, so close it.
            {
                ToggleMenu(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else // Menu is closed, so open it.
            {
                ToggleMenu(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
#endif

        // If input is enabled, ignore all of the below.
        if (!inputEnabled) { return; }
        
        // Gets player movement
        MouseInput();

        // Player rotation
        Vector3 playerRotation = new Vector3(0, mouseRotationInput.x, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        // Camera rotation - means that the player can't look underneath themself.
        cameraRotation = -mouseRotationInput.y * mouseSensitivity;

        // Modifies target from current direction to desired direction.
        charCamTarRot *= Quaternion.Euler(cameraRotation, 0.0f, 0.0f);

        charCamTarRot = ClampRotationAroundXAxis(charCamTarRot);

        // Use of localRotation allows movement around y axis.
        charCamera.transform.localRotation = charCamTarRot;
    }

    /// <summary>
    /// Retrieves the mouse input position and assigns it as a vector 3.
    /// </summary>
    protected virtual void MouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        mouseRotationInput = new Vector3(mouseX, mouseY, 0);
    }

    /// <summary>
    /// Sends a raycast directing down, checking for a floor.
    /// </summary>
    /// <param name="dirOfRay"></param>
    /// <returns>True if the player is grounded, false if not.</returns>
    protected bool IsGrounded(Vector3 dirOfRay)
    {
        return Physics.Raycast(transform.position, dirOfRay, distGround + groundDelta);
    }

    /// <summary>
    /// Takes a quaternion and clamps it using the yRotationClamp variable.
    /// </summary>
    /// <param name="q"></param>
    /// <returns>Returns the clamped rotation</returns>
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
    /// Toggles the pause menu, as well as the cursor and inputEnabled variable.
    /// </summary>
    /// <param name="toggle"></param>
    private void ToggleMenu(bool toggle)
    {
        menu.SetActive(toggle);
        Cursor.visible = toggle;
        inputEnabled = !toggle;
    }
}
