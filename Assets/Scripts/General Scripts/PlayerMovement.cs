using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] private int speed = 10; // Used to control the movement speed of the player.
    [SerializeField] private int mouseSensitivity = 1; // Used to control the sensitivity of the mouse.
    [SerializeField] private float jumpThrust = 10; // Used to control the jumping force of the player.

    private Rigidbody rigidBody; // Used to apply physics to the player, e.g. movement.
    private float playerHeight; // Used for the ground raycast.
    private Camera cameraGO; // Used to disable/enable the camera so that we only control our local player's camera.

    private void Start()
    {
        cameraGO = this.GetComponentInChildren<Camera>(); // Gets the camera child on the player.
        playerHeight = GetComponent<Collider>().bounds.extents.y; // Gets the players height using collider bounds.
        rigidBody = this.GetComponent<Rigidbody>(); // Gets the rigidbody component of the player.

        if (!photonView.IsMine)
        {
            cameraGO.GetComponent<Camera>().enabled = false; // Disables the camera on every client that isn't our own.
        }

    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // Player movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = transform.TransformDirection(new Vector3(x, 0, z) * speed);
        rigidBody.velocity = dir;

        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 playerRotation = new Vector3(0, mouseX, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        Vector3 cameraRotation = new Vector3(-mouseY, 0, 0) * mouseSensitivity;
        cameraGO.transform.Rotate(cameraRotation);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.velocity = Vector2.up * jumpThrust;
        }
    }

    private bool IsGrounded()
    {
        // Sends a raycast directing down, checking for a floor.
        return Physics.Raycast(transform.position, -Vector3.up, playerHeight + 0.1f);
    }
}
