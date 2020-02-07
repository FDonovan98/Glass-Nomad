using UnityEngine;

using System.Collections.Generic;

// Code initially based on code from here:
// https://answers.unity.com/questions/155907/basic-movement-walking-on-walls.html

[RequireComponent(typeof(Collider))]
public class AlienMovement : PlayerMovement
{
    // Smoothing speed.
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

    [SerializeField]
    public float maxLength = 0;

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();

        RotateTransformToSurfaceNormal();

        AlienJump();
    }

    private void AlienJump()
    {
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
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        XYMovement();
    }

    private void XYMovement()
    {
        // Gets the horz and vert movement for char.
        float deltaX = Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime;
        float deltaZ = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;

        if (Input.GetAxisRaw("Sprint") != 0)
        {
            deltaX *= sprintSpeedMultiplier;
            deltaZ *= sprintSpeedMultiplier;
        }

        transform.Translate(new Vector3(deltaX, 0.0f, deltaZ));
    }

    private void RotateTransformToSurfaceNormal()
    {
        Vector3 surfaceNormal = CalculateSurfaceNormal();
        // Interpolate between the characters current normal and the surface normal.
        //charNormal = Vector3.Lerp(charNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // Debug.Log("Surface Normal: " + surfaceNormal);

        // Vector2 startArc = new Vector2(charNormal.x, charNormal.y);
        // Debug.Log("Start arc: " + startArc);

        // Vector2 endArc = new Vector2(surfaceNormal.x, surfaceNormal.y);
        // Debug.Log("End arc: " + endArc);

        // Vector2 arcPoint = ArcLerp(startArc, endArc, lerpSpeed * Time.deltaTime);
        // Debug.Log("Arc point: " + arcPoint);

        // charNormal = new Vector3(arcPoint.x, arcPoint.y, charNormal.z);
        // Debug.Log("Character normal: " + charNormal);

        charNormal = surfaceNormal;

        Quaternion targetRotation;

        // // Get the direction the character faces.
        // Vector3 charForward = Vector3.Cross(transform.InverseTransformDirection(transform.right), charNormal);
        // Align the character to the surface normal while still looking forward.
        
        targetRotation = Quaternion.LookRotation(transform.forward, charNormal);    
        
        transform.localRotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
    }

    private Vector2 ArcLerp(Vector2 startVector, Vector2 endVector, float angleStep)
    {
        startVector = startVector.normalized;
        endVector = endVector.normalized;
        float startX = startVector.x;
        float startY  = startVector.y;
        float targetX;
        float targetY;
        
        float angle = Mathf.Acos(Vector2.Dot(startVector, endVector));
        if (angle <= angleStep)
        {
            return endVector;
        }

        targetX = startX + startVector.magnitude * Mathf.Cos(angleStep);
        targetY = startY + startVector.magnitude * Mathf.Sin(angleStep);

        return new Vector2(-targetX, -targetY);
    }


    private Vector3 CalculateSurfaceNormal()
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
            -charNormal
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
