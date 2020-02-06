using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MarineMovement : PlayerMovement
{
    public float force = 150f;
    protected Vector3 playerMovementInput; // Used to store the players movement input.

    protected new void Start()
    {
        base.Start();
        gravity = gravConstant;
        charNormal = Vector3.up;
    }

    protected new void Update()
    {
        base.Update();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void Ragdoll()
    {
        // Disable input
        inputEnabled = false;

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
