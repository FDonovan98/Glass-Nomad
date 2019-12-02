using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AlienMovement : MonoBehaviourPunCallbacks
{

    public Rigidbody rb;
    public int speed = 10;
    public int mouseSensitivity = 1;
    public float jumpThrust = 10;
    public LayerMask marineLayerMask;
    public int hitDistance = 1;
    private float playerHeight;
    public GameObject cameraGO;

    private void Start()
    {
        playerHeight = GetComponent<Collider>().bounds.extents.y;
        if (!photonView.IsMine)
        {
            cameraGO.GetComponent<Camera>().enabled = false;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        if (Input.GetAxisRaw("Fire1") == 1)
        {
            Debug.Log("Shoot");
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        // Player movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = transform.TransformDirection(new Vector3(x, 0, z) * speed);
        rb.velocity = dir;

        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 playerRotation = new Vector3(0, mouseX, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        Vector3 cameraRotation = new Vector3(-mouseY, 0, 0) * mouseSensitivity;
        cameraGO.transform.Rotate(cameraRotation);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector2.up * jumpThrust;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, playerHeight + 0.1f);
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, hitDistance, marineLayerMask))
        {
            Debug.Log("Hit Marine Character");
            hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        Debug.DrawRay(transform.position, Vector3.forward * 100, Color.red);
    }
}
