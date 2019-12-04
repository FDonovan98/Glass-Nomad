using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : PlayerMovement
{
    [SerializeField] private float lerpSpeed = 10; // Smoothing speed.
    [SerializeField] private float jumpRange = 10;// The range at which to detect a wall to stick to
    [SerializeField] private bool isPlayerGrounded = true; // Is the alien in contact with the ground.

    private const float gravity = 10; // Used as a const for the gravity scale.
    private Vector3 surfaceNormal = Vector3.zero;// The normal of the current surface.
    private Vector3 charNormal = Vector3.zero;// The characters normal
    private float verticalSpeed; // Current vertical speed.

    private new void Start()
    {
        base.Start();
        charNormal = transform.up; // Initialises the charNormal to the world normal.
    }

    private void Update()
    {
        if (!photonView.IsMine) // If we are not the local client then don't compute any of this.
            return;

        if (!isPlayerGrounded) // If the player is in the air then don't worry about any of this.
            return;

        PlayerInput(); // Process the player's input.

        Ray ray = new Ray(transform.position, -charNormal);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // If the character is touching the ground.
            if (hit.distance <= (playerHeight + 1))
            {
                // then, update the grounded variable and the surface normal.
                isPlayerGrounded = true;
                surfaceNormal = hit.normal;
            }
            else
            {
                // otherwise, reset the surface normal.
                isPlayerGrounded = false;
                surfaceNormal = Vector3.up; // Just completely breaks it.
            }

            // Interpolate between the characters current normal and the surface normal.
            charNormal = Vector3.Lerp(charNormal, surfaceNormal, lerpSpeed * Time.deltaTime);

            // Get the direction the character faces.
            Vector3 charForward = Vector3.Cross(transform.right, charNormal);

            // Align the character to the surface normal while still looking forward.
            Quaternion targetRotation = Quaternion.LookRotation(charForward, charNormal);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        // Camera rotation
        Vector3 cameraRotation = new Vector3(-mouseRotationInput.y, 0, 0) * mouseSensitivity;
        cameraGO.transform.Rotate(cameraRotation);

        // Player rotation
        Vector3 playerRotation = new Vector3(0, mouseRotationInput.x, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        // Calculate and apply force of gravity to char.
        Vector3 gravForce = -gravity * rigidBody.mass * charNormal;
        rigidBody.AddForce(gravForce);

        if (!isPlayerGrounded) // We can't move if the player is still in the air.
            return;


        // Player movement
        transform.Translate(playerMovementInput);
    }

    protected override void PlayerInput()
    {
        // If we are on the ground (or a wall?) and we pressed the jump key...
        if (IsGrounded(-charNormal) && Input.GetButtonDown("Jump"))
        {
            // If we can jump to a wall then do so...
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, jumpRange))
            {
                // Start the jump transition to the wall.
                StartCoroutine(JumpToWall(hit.point, hit.normal));
            }
            else
            {
                // otherwise apply a jump force to our movement variable.
                isPlayerGrounded = false;
                playerMovementInput += jumpSpeed * charNormal;
            }
        }

        // Player movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        playerMovementInput = new Vector3(x, 0, z) * speed * Time.deltaTime;

        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        mouseRotationInput = new Vector3(mouseX, mouseY, 0);
    }

    private IEnumerator JumpToWall(Vector3 point, Vector3 normal)
    {
        Debug.Log("JumpToWall");
        // Enables the flag saying the char is jumping.
        isPlayerGrounded = false;

        // Disables physics while jumping.
        rigidBody.isKinematic = true;

        // Gets the original position and rotation of char.
        Vector3 originalPos = transform.position;
        Quaternion originalRotation = transform.rotation;

        // Gets the point at which the function should give up control.
        float finalGroundOffset = 0.5f;
        Vector3 farPos = point + normal * (playerHeight + finalGroundOffset);

        // Gets the char forward facing and the rotation at the far point
        Vector3 charForward = Vector3.Cross(transform.right, normal);
        Quaternion farRotation = Quaternion.LookRotation(charForward, normal);

        // Interpolates between current position and target position for a second.
        float timeElapsed = 0.0f;
        do
        {
            timeElapsed += Time.deltaTime;

            transform.position = Vector3.Lerp(originalPos, farPos, timeElapsed);
            transform.rotation = Quaternion.Slerp(originalRotation, farRotation, timeElapsed);
            yield return null;

        } while (timeElapsed < 1.0f);

        // Update charNormal.
        charNormal = normal;
        // Re-enables physics.
        rigidBody.isKinematic = false;
        // Signals the jump to the wall has finished.
        isPlayerGrounded = true;
    }
}