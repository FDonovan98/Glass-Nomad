using UnityEngine;

using System.Collections;

// Code initially based on code from here:
// https://answers.unity.com/questions/155907/basic-movement-walking-on-walls.html

[RequireComponent(typeof(Collider))]
public class AlienMovement : PlayerMovement
{
    // Turn speed is in degrees per second.
    public float turnSpeed = 90; 
    // Smoothing speed.
    public float lerpSpeed = 10;
    public float gravity = 10;
    // Char counts as grounded up to this distance from the ground.
    public float deltaGround = 1.0f;
    // Is the alien in contact with the ground.
    public bool isGrounded;
    // The range at which to detect a wall to stick to.
    public float jumpRange = 10;
    // Time it takes to transfer between two surfaces.
    public float transferTime = 1;

    // The normal of the current surface.
    private Vector3 surfaceNormal;
    // The characters normal.
    private Vector3 charNormal;
    // Flag for if the alien is currently jumping.
    private bool jumping;
    // Current vertical speed.
    private float verticalSpeed;

    protected new void Start()
    {
        base.Start();
        // Initialises the charNormal to the world normal.
        charNormal = transform.up;
        Debug.Log(distGround);
    }

    protected void FixedUpdate()
    {
        // Calculate and apply force of gravity to char.
        Vector3 gravForce = -gravity * charRigidbody.mass * charNormal;
        charRigidbody.AddForce(gravForce);
    }

    protected new void Update()
    {   
        base.Update();
        Ray ray;
        RaycastHit hit;

        // // Exits Update if the character is mid-jump.
        // if (jumping)
        // {
        //     return;
        // }

        // When the jump key is pressed activate either a normal jump or a jump to a wall.
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump key pressed");
            // Creates a ray from the current position in the direction the char is facing.
            ray = new Ray(transform.position, charCamera.transform.forward);

            // If there is a wall ahead then trigger JumpToWall script.
            if (Physics.Raycast(ray, out hit, jumpRange) && hit.normal != this.transform.up)
            {
                StartCoroutine(JumpToWall(hit.point, hit.normal));
            }
            // If the player is on the ground then jump up.
            else if (isGrounded)
            {
                Debug.Log("Applying Jump Force");
                charRigidbody.velocity += jumpSpeed * charNormal;
            }
        }

        // Update surfaceNormal.
        // Casts ray downwards.
        ray = new Ray(transform.position, -charNormal);

        if (Physics.Raycast(ray, out hit))
        {
            // If the character is touching the ground.
            if (hit.distance <= (distGround + deltaGround))
            {
                isGrounded = true;
                surfaceNormal = hit.normal;
            }
            else
            {
                // If the character isn't grounded resets surface normal.
                isGrounded = false;
                surfaceNormal = Vector3.up;
            }

            // Interpolate between the characters current normal and the surface normal.
            charNormal = Vector3.Lerp(charNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
            // Get the direction the character faces.
            Vector3 charForward = Vector3.Cross(transform.right, charNormal);
            // Align the character to the surface normal while still looking forward.
            Quaternion targetRotation = Quaternion.LookRotation(charForward, charNormal);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
        }

        // Gets the horz and vert movement for char.
        float deltaX = Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime;
        float deltaZ = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;

        transform.Translate(new Vector3(deltaX, 0.0f, deltaZ));
    }

    IEnumerator JumpToWall(Vector3 point, Vector3 normal)
    {
        Debug.Log("JumpToWall");
        // Enables the flag saying the char is jumping.
        jumping = true;

        // Disables physics while jumping.
        charRigidbody.isKinematic = true;
        
        // Gets the original position and rotation of char.
        Vector3 originalPos = transform.position;
        Quaternion originalRotation = charCamera.transform.rotation;

        // Gets the point at which the function should give up control.
        float finalGroundOffset = 0.5f;
        Vector3 farPos = point + normal * (distGround + finalGroundOffset);

        // Gets the char forward facing and the rotation at the far point
        Vector3 charForward = charCamera.transform.forward;
        Quaternion farRotation = Quaternion.LookRotation(charForward, normal);

        // Interpolates between current position and target position for a second.
        float timeElapsed = 0.0f;

        do
        {
            timeElapsed += Time.deltaTime / transferTime;

            transform.position = Vector3.Lerp(originalPos, farPos, timeElapsed);
            transform.rotation = Quaternion.Slerp(originalRotation, farRotation, timeElapsed);
            
            yield return null; 

        } while (timeElapsed < 1.0f);

        // Update charNormal.
        charNormal = normal;
        // Re-enables physics.
        charRigidbody.isKinematic = false;
        // Signals the jump to the wall has finished.
        jumping = false;
    }
}
