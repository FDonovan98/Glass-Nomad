using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMovement : MonoBehaviour
{

    public Rigidbody rb;
    public int speed = 10;
    public float jumpThrust = 10;
    private float playerHeight;

    private void Start()
    {

        playerHeight = GetComponent<Collider>().bounds.extents.y;
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float z = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector3(x, rb.velocity.y, z);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * jumpThrust;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, playerHeight + 0.1f);
    }
}
