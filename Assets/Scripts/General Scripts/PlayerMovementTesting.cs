using UnityEngine;

public class stairtest : MonoBehaviour
{
    public float distanceBetweenStep;
    public float movementSpeed = 10;     
    public bool debug;
    public float gravity;
    public float mouseSensitivity;
    public Camera charCamera;
    public float yRotationClamp;
    public float upForce = 1.5f;
    public float jumpSpeed;
    public bool useStairMechanic;
    private Collider charCollider;
    private Rigidbody charRigidbody;
    private Quaternion charCameraTargetRotation;

    private void Start()
    {
        charCollider = GetComponent<Collider>();
        charRigidbody = GetComponent<Rigidbody>();
        charCameraTargetRotation = charCamera.transform.localRotation;
    }

    private void Update()
    {
        HandlePlayerRotation();

        if (useStairMechanic)
        {
            // If there is a step, and its height is correct, then try and apply force.
            if (CheckIfStep() && CheckStepHeight())
            {
                ApplyUpwardsForce();
            }

            if (debug) Debugging();
        }
    }

    private void FixedUpdate()
    {
        charRigidbody.velocity += gravity * Vector3.up * Time.fixedDeltaTime;
        charRigidbody.AddForce(transform.TransformDirection(GetPlayerInput()), ForceMode.Acceleration);
    }

    private void HandlePlayerRotation()
    {
        Vector3 mouseRotationInput = GetMouseInput(); 

        // Player rotation
        Vector3 playerRotation = new Vector3(0, mouseRotationInput.x, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        // Camera rotation
        float cameraRotation = -mouseRotationInput.y * mouseSensitivity;
        charCameraTargetRotation *= Quaternion.Euler(cameraRotation, 0.0f, 0.0f);
        charCameraTargetRotation = ClampRotationAroundXAxis(charCameraTargetRotation);

        // Use of localRotation allows movement around y axis.
        charCamera.transform.localRotation = charCameraTargetRotation;
    }

    private Vector3 GetPlayerInput()
    {
        float x, y, z;

        // Player movement
        x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        z = Input.GetAxisRaw("Vertical") * movementSpeed; 

        // Jump and ground detection
        if (IsGrounded(transform.position, -Vector3.up) && Input.GetKeyDown(KeyCode.Space))
        {
            y = jumpSpeed;
        }
        else
        {
            y = charRigidbody.velocity.y;
        }

        return new Vector3(x, y, z);
    }

    private Vector3 GetMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        return new Vector3(mouseX, mouseY, 0);
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

    private bool IsGrounded(Vector3 origin, Vector3 dirOfRay)
    {
        return Physics.Raycast(origin, dirOfRay, charCollider.bounds.extents.y + 0.1f);
    }

    #region stairs

    private void ApplyUpwardsForce()
    {
        // If there isn't a force already being applied upwards on the player, then...
        if (charRigidbody.velocity.y < upForce)
        {
            // If the player is pressing forward key ('W')...
            if (GetPlayerInput().z > 0)
            {
                // Apply an upwards force onto the player's rigidbody.
                charRigidbody.velocity += transform.up * upForce;
            }
        }
    }

    private bool CheckIfStep()
    {
        // If the player isn't grounded, then force has (presumably) already been applied.
        Vector3 frontOfPlayer = transform.position;
        frontOfPlayer += transform.forward * charCollider.bounds.extents.z;
        if (!IsGrounded(frontOfPlayer, -Vector3.up)) return false;

        // Start the ray at the bottom center of the player.
        Vector3 playerFeet = transform.position;
        playerFeet.y -= charCollider.bounds.extents.y;

        return Physics.Raycast(playerFeet, transform.forward, charCollider.bounds.extents.z + distanceBetweenStep);
    }

    private bool CheckStepHeight()
    {
        // Start the ray half way up the player, at the front.
        Vector3 startDir = transform.position;
        startDir += transform.forward * charCollider.bounds.extents.z;

        // End the ray on the floor, ahead of the player.
        Vector3 endDir = transform.position;
        endDir.y -= charCollider.bounds.extents.y;
        endDir += transform.forward * (charCollider.bounds.extents.z + (distanceBetweenStep / 2f));

        // Cast the ray and output it to the hitInfo.
        RaycastHit hitInfo;
        bool stepHeight = Physics.Raycast(startDir, endDir - startDir, out hitInfo, distanceBetweenStep);
        if (debug) Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.cyan);

        // If the step height is correct and the step's normal is the worlds up axis then return true.
        return stepHeight /*&& hitInfo.normal == Vector3.up*/; // ** THIS LINE MAY HAVE BROKEN IT **
    }

    private void Debugging()
    {
        // Used to check the distance betweent the players feet and the step.
        Vector3 playerFeet = transform.position;
        playerFeet.y -= charCollider.bounds.extents.y;
        Debug.DrawRay(playerFeet, transform.forward * (charCollider.bounds.extents.z + distanceBetweenStep), Color.magenta);

        // Used to check how steep the step is, and its height.
        Vector3 startDir = transform.position;
        startDir += transform.forward * charCollider.bounds.extents.z; // Box colliders has slightly larger extents.
        Vector3 endDir = transform.position;
        endDir.y -= charCollider.bounds.extents.y;
        endDir += transform.forward * (charCollider.bounds.extents.z + (distanceBetweenStep / 2f));
        Debug.DrawRay(startDir, endDir - startDir, Color.red);

        // Used to check is the player is on the ground.
        Vector3 frontOfPlayer = transform.position;
        frontOfPlayer += transform.forward * charCollider.bounds.extents.z;
        Debug.DrawRay(frontOfPlayer, -Vector3.up * (charCollider.bounds.extents.y + 0.5f), Color.green);

        Debug.Log("IS GROUNDED: " + IsGrounded(frontOfPlayer, -Vector3.up));
        Debug.Log("IS THERE A STEP: " + CheckIfStep());
        Debug.Log("STEP HEIGHT LOW ENOUGH: " + CheckStepHeight());
    }
    
    #endregion
}
