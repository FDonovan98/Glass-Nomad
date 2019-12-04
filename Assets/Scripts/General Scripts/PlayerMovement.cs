using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int speed = 10; // Used to control the movement speed of the player.
    [SerializeField] protected int mouseSensitivity = 1; // Used to control the sensitivity of the mouse.
    [SerializeField] protected float jumpSpeed = 10; // Used to control the jumping force of the player.

    protected Rigidbody rigidBody; // Used to apply physics to the player, e.g. movement.
    protected float playerHeight; // Used for the ground raycast.
    protected Camera cameraGO; // Used to disable/enable the camera so that we only control our local player's camera.
    protected Vector3 playerMovementInput; // Used to store the players movement input.
    protected Vector3 mouseRotationInput; // Used to store rotation of the player and the camera.

    protected void Start()
    {
        cameraGO = this.GetComponentInChildren<Camera>(); // Gets the camera child on the player.
        playerHeight = GetComponent<Collider>().bounds.extents.y; // Gets the players height using collider bounds.
        rigidBody = this.GetComponent<Rigidbody>(); // Gets the rigidbody component of the player.

        if (!photonView.IsMine)
        {
            cameraGO.GetComponent<Camera>().enabled = false; // Disables the camera on every client that isn't our own.
        }

    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        PlayerInput(); // Gets player movement
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // Player movement
        Vector3 dir = transform.TransformDirection(playerMovementInput * speed);
        rigidBody.velocity = dir;

        // Player rotation
        Vector3 playerRotation = new Vector3(0, mouseRotationInput.x, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        // Camera rotation
        Vector3 cameraRotation = new Vector3(-mouseRotationInput.y, 0, 0) * mouseSensitivity;
        cameraGO.transform.Rotate(cameraRotation);
    }

    protected virtual void PlayerInput()
    {
        float x, y, z; // Declare x, y and z axis variables for player movement.

        // Jump and ground detection
        if (IsGrounded(-Vector3.up) && Input.GetKeyDown(KeyCode.Space))
        {
            y = jumpSpeed;
        }
        else
        {
            y = rigidBody.velocity.y;
        }

        // Player movement
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        playerMovementInput = new Vector3(x, y, z);

        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        mouseRotationInput = new Vector3(mouseX, mouseY, 0);
    }

    protected bool IsGrounded(Vector3 dirOfRay)
    {
        // Sends a raycast directing down, checking for a floor.
        return Physics.Raycast(transform.position, dirOfRay, playerHeight + 0.1f);
    }
}
