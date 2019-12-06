using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int movementSpeed = 10; // Used to control the movement movementSpeed of the player.
    [SerializeField] protected int mouseSensitivity = 1; // Used to control the sensitivity of the mouse.
    [SerializeField] protected float jumpSpeed = 10; // Used to control the jumping force of the player.

    protected Rigidbody charRigidbody; // Used to apply physics to the player, e.g. movement.
    protected float distGround; // Used for the ground raycast.
    protected Collider charCollider;
    protected Camera charCamera; // Used to disable/enable the camera so that we only control our local player's camera.
    protected Vector3 mouseRotationInput; // Used to store rotation of the player and the camera.
    protected float groundDelta = 1.0f;

    protected void Start()
    {
        gameObject.name = photonView.Owner.NickName; // Sets the gameobject name to the player's username.
        charCamera = gameObject.GetComponentInChildren<Camera>(); // Gets the camera child on the player.
        charCollider = gameObject.GetComponent<CapsuleCollider>();
        distGround =  charCollider.bounds.extents.y;
        Debug.Log("distGround: " + distGround);
        charRigidbody = gameObject.GetComponent<Rigidbody>(); // Gets the rigidbody component of the player.

        if (!photonView.IsMine)
        {
            charCamera.GetComponent<Camera>().enabled = false; // Disables the camera on every client that isn't our own.
        }

    }

    protected void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        PlayerInput(); // Gets player movement

        // Player rotation
        Vector3 playerRotation = new Vector3(0, mouseRotationInput.x, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        // Camera rotation
        Vector3 cameraRotation = new Vector3(-mouseRotationInput.y, 0, 0) * mouseSensitivity;
        charCamera.transform.Rotate(cameraRotation);
    }

    protected virtual void PlayerInput()
    {
        float x, y, z; // Declare x, y and z axis variables for player movement.

        // // Jump and ground detection
        // if (IsGrounded(-Vector3.up) && Input.GetKeyDown(KeyCode.Space))
        // {
        //     y = jumpSpeed;
        // }
        // else
        // {
        //     y = charRigidbody.velocity.y;
        // }

        // // Player movement
        // x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        // z = Input.GetAxisRaw("Vertical") * movementSpeed;
        // playerMovementInput = new Vector3(x, charRigidbody.velocity.y, z);

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
}
