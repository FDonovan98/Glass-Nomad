using UnityEngine;

using System.Collections;

// Code initially based on code from here:
// https://answers.unity.com/questions/155907/basic-movement-walking-on-walls.html

[RequireComponent(typeof(Collider))]
public class AlienMovement : PlayerMovement
{
    // Smoothing speed.
    public float lerpSpeed = 1;
    public float gravConstant = 10;
    private float gravity;
    // Char counts as grounded up to this distance from the ground.
    public float deltaGround = 0.1f;
    // Is the alien in contact with the ground.
    public bool isGrounded = false;
    // The range at which to detect a wall to stick to.
    public float jumpRange = 10;
    // Time it takes to transfer between two surfaces.
    public float transferTime = 1;

    // The normal of the current surface.
    private Vector3 surfaceNormal;
    // The characters normal.
    private Vector3 charNormal;

    // Flag for if the alien is currently jumping.
    //private bool jumping;
    // Current vertical speed.
    //private float verticalSpeed;

    protected new void Start()
    {
        base.Start();
        // Initialises the charNormal to the world normal.
        charNormal = transform.up;
        Debug.Log(distGround);
        gravity = gravConstant;
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
                charRigidbody.velocity += jumpSpeed * charCamera.transform.forward;
            }
        }

        // Vectors needed to cast rays in six directions around the alien.
        // -charNormal needs to be last for movement to work well within vents.
        Vector3[] testVectors = new Vector3 [6] 
        {
            transform.right,
            -transform.right, 
            transform.forward,
            -transform.forward,
            charNormal,
            -charNormal
        };

        Vector3 averageRayDirection = new Vector3(0, 0, 0);
        int ventCount = 0;
        gravity = gravConstant;

        foreach (Vector3 element in testVectors)
        {
            if (Physics.Raycast(transform.position, element, out hit, distGround + deltaGround))
            {
                if (hit.transform.gameObject.tag == "Vent")
                {
                    ventCount++;
                }

                // If there are more than two vents surrounding the alien wall running mechanic changes.
                // Gravity is disabled and alien just sticks to the surface below it.
                if (ventCount <= 2)
                {
                    averageRayDirection += hit.normal;
                }
                else
                {
                    gravity = 0;
                    averageRayDirection = hit.normal;
                }
            }
        }

        // Magnitude is only zero if the alien isn't close to any surface.
        // In this case it falls towards the ground.
        if (averageRayDirection.magnitude > 0)
        {
            isGrounded = true;
            surfaceNormal = averageRayDirection.normalized;
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
        Vector3 charForward = Vector3.Cross(charCamera.transform.right, charNormal);
        // Align the character to the surface normal while still looking forward.
        Quaternion targetRotation = Quaternion.LookRotation(charForward, charNormal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
        

        // Gets the horz and vert movement for char.
        float deltaX = Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime;
        float deltaZ = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;

        transform.Translate(new Vector3(deltaX, 0.0f, deltaZ));
    }

    IEnumerator JumpToWall(Vector3 point, Vector3 normal)
    {
        Debug.Log("JumpToWall");
        // Enables the flag saying the char is jumping.
        //jumping = true;

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
        //jumping = false;
    }
}
