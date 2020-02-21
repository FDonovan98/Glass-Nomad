using UnityEngine;

// Code initially based on code from here:
// https://answers.unity.com/questions/155907/basic-movement-walking-on-walls.html

[RequireComponent(typeof(Collider))]
public class AlienMovement : PlayerMovement
{
    #region variable-declaration

    // Smoothing speed of rotating to wall.
    public float lerpSpeed = 1;

    // Char counts as grounded up to this distance from the ground.
    public float deltaGround = 0.1f;

    // Is the alien in contact with the ground.
    public bool isGrounded = false;

    // The range at which to detect a wall to stick to.
    public float jumpRange = 10;

    // Time it takes to transfer between two surfaces.
    public float transferTime = 1;

    // Variables used for adjusting jump charge.
    private float jumpCharge = 0.0f;
    public float jumpChargeTime = 1.0f;
    public float horizontalJumpMod = 1.0f;
    public float verticalJumpMod = 1.0f;

    // Should the debug messages be displayed.
    public bool debug = false;

    #endregion

    [SerializeField]
    public float maxLength = 0;

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        if (!photonView.IsMine) return;
        if (!inputEnabled || Cursor.lockState == CursorLockMode.None) return;

        AlienJump();
    }

    /// <summary>
    /// Retrieves the jump input and determines whether to perform a normal
    /// jump or a jump to a wall.
    /// </summary>
    private void AlienJump()
    {
        if (Input.GetButton("Jump"))
        {
            jumpCharge += Time.deltaTime;
            if (debug) Debug.Log("Jump key pressed");
        }


        if (Input.GetButtonUp("Jump"))
        {
            // If the player is on the ground then jump up.
            // Jump speed is multiplied by jump charge.
            if (isGrounded)
            {
                // Limits the jump multiplier.
                jumpCharge = Mathf.Min(jumpCharge, jumpChargeTime);
                if (debug) Debug.Log("Applying Jump Force");
                float jumpForce = jumpSpeed * jumpCharge;
                charRigidbody.velocity += horizontalJumpMod * jumpForce * charCamera.transform.forward;
                charRigidbody.velocity += verticalJumpMod * jumpForce * charNormal;

                jumpCharge = 0.0f;
            }
        }
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if (!photonView.IsMine) return;
        if (!inputEnabled || Cursor.lockState == CursorLockMode.None) return;
        
        RotateTransformToSurfaceNormal();

        charRigidbody.AddForce(transform.TransformDirection(GetPlayerInput()), ForceMode.Acceleration);
    }



    /// <summary>
    /// Rotates the alien to the normal of the surface which the alien in on.
    /// </summary>
    private void RotateTransformToSurfaceNormal()
    {
        bool zAxisRayHit = false;
        Vector3 surfaceNormal = CalculateSurfaceNormal(ref zAxisRayHit);

        charNormal = surfaceNormal;

        Quaternion targetRotation;
        Vector3 charForward;

        // Align the character to the surface normal while still looking forward.

        if (zAxisRayHit)
        {
            charForward = Vector3.Cross(transform.right, charNormal);
        }
        else
        {
            charForward = transform.forward;
        }
        
        targetRotation = Quaternion.LookRotation(charForward, charNormal);  
        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
    }

    private Vector3 CalculateSurfaceNormal(ref bool zAxisRayHit)
    {
        // Vectors needed to cast rays in six directions around the alien.
        // -charNormal needs to be last for movement to work well within vents.
        Vector3[] testVectors = new Vector3 [6] 
        {
            transform.right,
            -transform.right, 
            transform.forward,
            -transform.forward,
            charNormal,
            -charNormal,
        };

        Vector3 averageRayDirection = new Vector3(0, 0, 0);
        int ventCount = 0;

        RaycastHit hit;

        foreach (Vector3 element in testVectors)
        {
            if (Physics.Raycast(transform.position, element, out hit, distGround + deltaGround))
            {
                if (element == transform.forward || element == -transform.forward)
                {
                    zAxisRayHit = true;
                }

                if (hit.transform.gameObject.tag == "Vent")
                {
                    ventCount++;
                }

                // If there are more than two vents surrounding the alien wall running mechanic changes.
                // Gravity is disabled and alien just sticks to the surface below it.
                if (ventCount <= 2)
                {
                    averageRayDirection += hit.normal * ((distGround + deltaGround) - hit.distance);
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
            return averageRayDirection.normalized;
        }    
        else
        {
            // If the character isn't grounded resets surface normal.
            isGrounded = false;
            return Vector3.up;
        }

    }
}
