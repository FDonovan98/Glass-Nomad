using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    // Used to control the movement movementSpeed of the player.
    [SerializeField] 
    public float movementSpeed = 10;     
    
    // Used to control the sensitivity of the mouse.
    [SerializeField] 
    protected int mouseSensitivity = 1; 

    // Used to control the jumping force of the player.
    [SerializeField] 
    protected float jumpSpeed = 10; 

    // Used to stop the player looking 'underneath' themselves.
    [SerializeField] 
    protected float yRotationClamp = 30; 

    [SerializeField] 
    protected float sprintSpeedMultiplier = 1.5f;

    // Used to hide and show the menu options.
    [SerializeField] 
    private GameObject menu = null; 

    // Used to apply physics to the player, e.g. movement.
    protected Rigidbody charRigidbody; 

    // Used for the ground raycast.
    protected float distGround; 
    protected Collider charCollider;

    // Used to disable/enable the camera so that we only control our local player's camera.
    protected Camera charCamera; 
    
    // Used to store rotation of the player and the camera.
    protected Vector3 mouseRotationInput; 

    protected float groundDelta = 1.0f;
    protected float cameraRotation = 0f;
    protected Quaternion charCamTarRot;
    protected bool inputEnabled = true;

    protected void Start()
    {
        // Sets the gameobject name to the player's username.
        gameObject.name = photonView.Owner.NickName; 

        // Gets the camera child on the player.
        charCamera = gameObject.GetComponentInChildren<Camera>(); 
        charCollider = gameObject.GetComponent<CapsuleCollider>();
        distGround =  charCollider.bounds.extents.y;
        Debug.Log("distGround: " + distGround);

        // Gets the rigidbody component of the player.
        charRigidbody = gameObject.GetComponent<Rigidbody>(); 

        //Cursor starts off locked to the center of the game window and invisible
        Cursor.lockState = CursorLockMode.Locked;   

        if (!photonView.IsMine)
        {
            // Disables the camera on every client that isn't our own.
            charCamera.GetComponent<Camera>().enabled = false; 
        }

        charCamTarRot = charCamera.transform.localRotation;

        menu = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().pauseMenu;

        // Forces every player's mouse to the center of the window and hides it when the player is created.
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    protected void Update()
    {
        HandlePauseMenu();

        // If input is enabled, ignore all of the below.
        if (!inputEnabled) 
        { 
            return; 
        } 
        
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

    protected virtual void MouseInput()
    {        
        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        mouseRotationInput = new Vector3(mouseX, mouseY, 0);
    }

    protected bool IsGrounded(Vector3 dirOfRay)
    {
        // Sends a raycast directing down, checking for a floor.
        return Physics.Raycast(transform.position, dirOfRay, distGround + groundDelta);
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
