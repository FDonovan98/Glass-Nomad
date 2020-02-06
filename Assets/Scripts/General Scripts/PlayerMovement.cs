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
    protected Collider charCollider;

    // Disables/enables the camera so that we only control our local player's camera.
    protected Camera charCamera; 
    
    // Stores rotation of the player and the camera.
    protected Vector3 mouseRotationInput; 

    protected float groundDelta = 1.0f;
    protected float cameraRotation = 0f;
    protected Quaternion charCameraTargetRotation;
    protected bool inputEnabled = true;

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

        // Gets the camera child on the player.
        charCamera = gameObject.GetComponentInChildren<Camera>(); 
        charCollider = gameObject.GetComponent<Collider>();
        distGround =  charCollider.bounds.extents.y;

        // Gets the rigidbody component of the player.
        charRigidbody = gameObject.GetComponent<Rigidbody>(); 

        menu = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().pauseMenu;

        charCameraTargetRotation = charCamera.transform.localRotation;
    }

    protected void Update()
    {
        HandlePauseMenu();

        // If input is enabled, ignore player and camera rotation.
        if (!inputEnabled) return;

        HandlePlayerRotation();
    }

    private void HandlePlayerRotation()
    {
        Vector3 mouseRotationInput = GetMouseInput(); 

        // Player rotation
        Vector3 playerRotation = new Vector3(0, mouseRotationInput.x, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        // Camera rotation
        cameraRotation = -mouseRotationInput.y * mouseSensitivity;
        charCameraTargetRotation *= Quaternion.Euler(cameraRotation, 0.0f, 0.0f);
        charCameraTargetRotation = ClampRotationAroundXAxis(charCameraTargetRotation);

        // Use of localRotation allows movement around y axis.
        charCamera.transform.localRotation = charCameraTargetRotation;
    }

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

    protected Vector3 GetMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        return new Vector3(mouseX, mouseY, 0);
    }

    protected bool IsGrounded(Vector3 origin, Vector3 dirOfRay)
    {
        return Physics.Raycast(origin, dirOfRay, distGround + groundDelta);
    }

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

    private void ToggleMenu(bool toggle)
    {
        menu.SetActive(toggle);
        Cursor.visible = toggle;
        inputEnabled = !toggle;
    }
}
