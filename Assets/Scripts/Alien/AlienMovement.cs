using Photon.Pun;
using UnityEngine;

// Code initially based on code from here:
// https://answers.unity.com/questions/155907/basic-movement-walking-on-walls.html

[RequireComponent(typeof(Collider))]
public class AlienMovement : PlayerMovement
{
<<<<<<< HEAD
    // Smoothing speed.
    public float lerpSpeed = 1;
    public float gravConstant = 10;
    private float gravity;
    // Char counts as grounded up to this distance from the ground.
    public float deltaGround = 0.1f;
=======
    #region variable-declaration

    // Smoothing speed of rotating to wall.
    public float lerpSpeed = 1;

    // Char counts as grounded up to this distance from the ground.
    public float deltaGround = 0.1f;

>>>>>>> master
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

<<<<<<< HEAD
    // The normal of the current surface.
    private Vector3 surfaceNormal;
    // The characters normal.
    private Vector3 charNormal;
=======
    #endregion
>>>>>>> master

    [SerializeField]
    public float maxLength = 0;

    protected new void Start()
    {
        base.Start();
<<<<<<< HEAD
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
=======
>>>>>>> master
    }

    protected new void Update()
    {
        base.Update();
        if (!photonView.IsMine && !PhotonNetwork.PhotonServerSettings.StartInOfflineMode) return;
        if (!inputEnabled || Cursor.lockState == CursorLockMode.None) return;

<<<<<<< HEAD
        if (Input.GetButtonDown("Jump"))
        {
            // Creates a ray from the current position in the direction the char is facing.
            ray = new Ray(transform.position, charCamera.transform.forward);

            // If there is a wall ahead then trigger JumpToWall script.
            if (Physics.Raycast(ray, out hit, jumpRange) && hit.normal != this.transform.up)
            {
                StartCoroutine(JumpToWall(hit.point, hit.normal));
            }
        }

        // When the jump key is pressed activate either a normal jump or a jump to a wall.
        if (Input.GetButton("Jump"))
        {
            jumpCharge += Time.deltaTime;
            Debug.Log("Jump key pressed");
        }


        if (Input.GetButtonUp("Jump"))
        {
            // If the player is on the ground then jump up.
            // Jump speed is multiplied by jump charge.
            if (isGrounded)
            {
                // Limits the jump multiplier.
                jumpCharge = Mathf.Min(jumpCharge, jumpChargeTime);
                Debug.Log("Applying Jump Force");
                float jumpForce = jumpSpeed * jumpCharge;
                charRigidbody.velocity += horizontalJumpMod * jumpForce * charCamera.transform.forward;
                charRigidbody.velocity += verticalJumpMod * jumpForce * charNormal;

                jumpCharge = 0.0f;
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
=======
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
>>>>>>> master
        }
    }

<<<<<<< HEAD
        // Interpolate between the characters current normal and the surface normal.
        charNormal = Vector3.Lerp(charNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // Get the direction the character faces.
        Vector3 charForward = Vector3.Cross(transform.right, charNormal);
        // Align the character to the surface normal while still looking forward.
        Quaternion targetRotation = Quaternion.LookRotation(charForward, charNormal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
        

        // Gets the horz and vert movement for char.
        float deltaX = Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime;
        float deltaZ = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;
=======
    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if (!photonView.IsMine && !PhotonNetwork.PhotonServerSettings.StartInOfflineMode) return;
        if (!inputEnabled || Cursor.lockState == CursorLockMode.None) return;
        
        RotateTransformToSurfaceNormal();
>>>>>>> master

        charRigidbody.AddForce(transform.TransformDirection(GetPlayerInput()), ForceMode.Acceleration);
    }



    /// <summary>
    /// Rotates the alien to the normal of the surface which the alien in on.
    /// </summary>
    private void RotateTransformToSurfaceNormal()
    {
        Vector3 surfaceNormal = CalculateSurfaceNormal();

        float dotProduct = Vector3.Dot(charNormal, surfaceNormal);
        float cosTheta = dotProduct / (charNormal.magnitude * surfaceNormal.magnitude);
        float theta = Mathf.Acos(cosTheta);

        if (debug) Debug.Log(Mathf.Rad2Deg * theta);
        // charNormal is used when applying gravity so needs to be set.
        charNormal = surfaceNormal;

        Quaternion targetRotation;
        Vector3 charForward;

        // I can't fix the view snapping it's beyond me - Harry.
        // Needs to check if character velocity vector is <= 45 degree of the surface normal. 
        // If it is then movement needs to be handled differently.

        charForward = Vector3.Cross(transform.right, charNormal);
        
        targetRotation = Quaternion.LookRotation(charForward, charNormal);  
        
        if (transform.rotation != targetRotation)
        {
            if (debug) Debug.Log("Lerping");
            charRigidbody.velocity += charNormal;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed);
        }
        
    }

    private Vector3 CalculateSurfaceNormal()
    {
        // Vectors needed to cast rays in six directions around the alien.
        // -charNormal needs to be last for movement to work well within vents.
        // Vector3[] testVectors = new Vector3 [6] 
        // {
        //     transform.right,
        //     -transform.right, 
        //     transform.forward,
        //     -transform.forward,
        //     charNormal,
        //     -charNormal,
        // };

        Vector3[] testVectors = new Vector3[2]
        {
            -charNormal,
            charRigidbody.velocity.normalized
        };

        Vector3 averageRayDirection = new Vector3(0, 0, 0);
        int ventCount = 0;

        RaycastHit hit;

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

                if(debug) Debug.DrawRay(transform.position, element * (distGround + deltaGround), Color.red);
            }
        }

        // Magnitude is only zero if the alien isn't close to any surface.
        // In this case it falls towards the ground.
        if (averageRayDirection.magnitude > 0)
        {
            isGrounded = true;

            if(debug) Debug.DrawRay(transform.position, averageRayDirection.normalized * (distGround + deltaGround), Color.green);
            
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
