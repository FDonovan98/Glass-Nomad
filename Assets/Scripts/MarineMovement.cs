using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineMovement : PlayerMovement
{
    protected Vector3 playerMovementInput; // Used to store the players movement input.
    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private new void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        base.Update();
        GetPlayerInput();
        // Player movement
        Vector3 dir = transform.TransformDirection(playerMovementInput);
        charRigidbody.velocity = dir;
    }

    private void GetPlayerInput()
    {
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
}
