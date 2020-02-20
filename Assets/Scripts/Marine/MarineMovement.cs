using UnityEngine;
using Photon.Pun;

public class MarineMovement : PlayerMovement
{       
    protected new void Start()
    {
        base.Start();

        if (!photonView.IsMine) return;
    }
    
    protected new void Update()
    {
        base.Update();

        if (!photonView.IsMine) return;

        if (!inputEnabled || Cursor.lockState == CursorLockMode.None) return;
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (!photonView.IsMine) return;

        if (!inputEnabled || Cursor.lockState == CursorLockMode.None) return;
        // Player movement
        charRigidbody.AddForce(transform.TransformDirection(GetPlayerInput()), ForceMode.Acceleration);
    }

    /// <summary>
    /// Retrieves the player's WASD and jump input, applying forces where necessary.
    /// </summary>
    public override Vector3 GetPlayerInput()
    {
        Vector3 XZMovement = base.GetPlayerInput();
        float y;

        // Jump and ground detection
        if (IsGrounded(transform.position, -Vector3.up) && Input.GetKeyDown(KeyCode.Space))
        {
            y = jumpSpeed;
        }
        else
        {
            y = charRigidbody.velocity.y;
        }

        return new Vector3(XZMovement.x, y, XZMovement.z);
    }
}