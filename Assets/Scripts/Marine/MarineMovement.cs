using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MarineMovement : PlayerMovement
{
    // How much force should be applied randomly to player upon death.
    [SerializeField] private float deathForce = 150f;

    // The scale of gravity to be applied downwards onto the player.
    [SerializeField] private float gravity = -10;

    // The distance that the step is detected by the player.
    [SerializeField] private float distanceBetweenStep = 2f;

    // The upward force applied to the player when on stairs.
    [SerializeField] private float upForce = 1.5f;

    // Should the debug rays and console messages be shown.
    [SerializeField] private bool debug;

    // Used to store the players movement input.
    private Vector3 playerMovementInput;

    protected new void Update()
    {
        base.Update();
        if (!inputEnabled) { return; }

        CheckIfStep();
        if (debug) Debugging();
        GetPlayerInput();

        // Player movement
        Vector3 dir = transform.TransformDirection(playerMovementInput);
        charRigidbody.velocity = dir;
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        // Calculate and apply force of gravity to char.
        Vector3 gravForce = gravity * charRigidbody.mass * Vector3.up;
        charRigidbody.AddForce(gravForce);
    }

    private void GetPlayerInput()
    {
        // Declare x, y and z axis variables for player movement.
        float x, y, z;

        // Jump and ground detection
        if (IsGrounded(-Vector3.up) && Input.GetKeyDown(KeyCode.Space))
        {
            charRigidbody.velocity += new Vector3(0, jumpSpeed, 0);
        }
        else
        {
            y = charRigidbody.velocity.y;
        }

        // Player movement
        x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        z = Input.GetAxisRaw("Vertical") * movementSpeed;

        if (Input.GetAxis("Sprint") >= 1)
        {
            x *= sprintSpeedMultiplier;
            z *= sprintSpeedMultiplier;
        }   

        playerMovementInput = new Vector3(x, charRigidbody.velocity.y, z);
    }

    public void Ragdoll()
    {
        // Disable input
        inputEnabled = false;

        // Enable rotation constraints
        charRigidbody.constraints = RigidbodyConstraints.None;

        // Apply force
        charRigidbody.AddForceAtPosition(RandomForce(deathForce), transform.position);

        // Start death (a.k.a delete the player gameobject)
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Death());
        }
    }

    private Vector3 RandomForce(float velocity)
    {
        return new Vector3(Random.Range(0, velocity), Random.Range(0, velocity), Random.Range(0, velocity));
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.Destroy(this.gameObject);
        PhotonNetwork.LeaveRoom();
    }

    private bool IsGrounded()
    {
        // Sends a raycast directing down, checking for a floor.
        Vector3 frontOfPlayer = transform.position;
        frontOfPlayer.z += charCollider.bounds.extents.z;
        return Physics.Raycast(frontOfPlayer, -Vector3.up, charCollider.bounds.extents.y + 0.5f);
    }

    #region stairs

    private void CheckIfStep()
    {
        if (!IsGrounded()) return;
        Vector3 playerFeet = transform.position;
        playerFeet.y -= charCollider.bounds.extents.y;
        if (Physics.Raycast(playerFeet, transform.forward, distanceBetweenStep)) CheckStepHeight();
    }

    private void CheckStepHeight()
    {
        Vector3 startDir = transform.position;
        startDir.z += charCollider.bounds.extents.z;

        Vector3 endDir = transform.position;
        endDir.y -= charCollider.bounds.extents.y;
        endDir.z += charCollider.bounds.extents.z + 0.1f;
        // NEED TO CHECK IF THE HIT.NORMAL IS VECTOR3.UP
        if (Physics.Raycast(startDir, endDir, distanceBetweenStep)) ApplyUpwardsForce();
    }

    private void ApplyUpwardsForce()
    {
        if (charRigidbody.velocity.y < upForce)
        {
            if (playerMovementInput.z > 0)
            {
                charRigidbody.velocity += transform.up * upForce;
            }
        }
    }

    private void Debugging()
    {
        // Used to check the distance betweent the players feet and the step.
        Vector3 playerFeet = transform.position;
        playerFeet.y -= charCollider.bounds.extents.y;
        Debug.DrawRay(playerFeet, transform.forward * (charCollider.bounds.extents.z + distanceBetweenStep), Color.magenta);

        // Used to check how steep the step is, and its height.
        Vector3 startDir = transform.position;
        startDir.z += charCollider.bounds.extents.z;
        Vector3 endDir = transform.position;
        endDir.y -= charCollider.bounds.extents.y;
        endDir.z += charCollider.bounds.extents.z + 0.1f;
        Debug.DrawRay(startDir, endDir - startDir, Color.red);

        // Used to check is the player is on the ground.
        Vector3 frontOfPlayer = transform.position;
        frontOfPlayer.z += charCollider.bounds.extents.z;
        Debug.DrawRay(frontOfPlayer, -Vector3.up * (charCollider.bounds.extents.y + 0.5f), Color.green);

        Debug.Log("IS GROUNDED: " + IsGrounded());
        Debug.Log("IS THERE A STEP: " + Physics.Raycast(playerFeet, transform.forward, distanceBetweenStep));
        Debug.Log("STEP HEIGHT LOW ENOUGH: " + Physics.Raycast(startDir, endDir, distanceBetweenStep));
    }

    #endregion
}
