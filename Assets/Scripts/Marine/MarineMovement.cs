using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MarineMovement : PlayerMovement
{
    public float force = 150f;
    protected Vector3 playerMovementInput; // Used to store the players movement input.

    public bool InputEnabled = true;

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        
        GetPlayerInput();
        // Player movement
        Vector3 dir = transform.TransformDirection(playerMovementInput);
        charRigidbody.velocity = dir;
    }

    private void GetPlayerInput()
    {
        if (!InputEnabled) { return; }
        float x, y, z; // Declare x, y and z axis variables for player movement.

        // Jump and ground detection
        if (IsGrounded(-Vector3.up) && Input.GetKeyDown(KeyCode.Space))
        {
            y = jumpSpeed;
        }
        else
        {
            y = charRigidbody.velocity.y;
        }

        // Player movement
        x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        z = Input.GetAxisRaw("Vertical") * movementSpeed;
        playerMovementInput = new Vector3(x, charRigidbody.velocity.y, z);
    }

    public void Ragdoll()
    {
        // Disable input
        InputEnabled = false;

        // Enable rotation constraints
        charRigidbody.constraints = RigidbodyConstraints.None;

        // Apply force
        charRigidbody.AddForceAtPosition(RandomForce(force), transform.position);

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
}
