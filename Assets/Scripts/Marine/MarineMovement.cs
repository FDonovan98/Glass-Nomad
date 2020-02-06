using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MarineMovement : PlayerMovement
{
    [SerializeField] private float deathForce = 150f;
    [SerializeField] private float gravConstant = -10;
    [SerializeField] private float distanceBetweenStep = 2f;
    [SerializeField] private float upForce = 1.5f;
    [SerializeField] private bool debug;

    private Vector3 gravityVector;
    private Vector3 playerMovementInput; // Used to store the players movement input.
    private float gravity;

    protected new void Start()
    {
        base.Start();
        gravity = gravConstant;
    }

    protected new void Update()
    {
        base.Update();
        if (!inputEnabled) { return; }

        CheckIfStep();
        ApplyGravity();
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
        Vector3 gravForce = -gravity * charRigidbody.mass * Vector3.up;
        charRigidbody.AddForce(gravForce);
    }

    private void GetPlayerInput()
    {
        float x, y, z; // Declare x, y and z axis variables for player movement.

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

        if (Input.GetAxis("Sprint") == 1)
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

    private void CheckIfStep()
    {

    }

    private void CheckStepHeight()
    {

    }

    private void ApplyUpwardsForce()
    {

    }

    private void Debugging()
    {

    }
}
