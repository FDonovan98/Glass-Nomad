using UnityEngine;

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

<<<<<<< HEAD
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
=======
        return new Vector3(XZMovement.x, y, XZMovement.z);
>>>>>>> master
    }
}