using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class AlienController : MonoBehaviourPunCallbacks
{

    [SerializeField] private int speed = 10;
    [SerializeField] private int mouseSensitivity = 1;
    [SerializeField] private float jumpThrust = 10;
    [SerializeField] private LayerMask marineLayerMask;
    [SerializeField] private int hitDistance = 1;

    public int playerMaxHealth = 50; // Needs to be public as they are accessed by attacking enemies
    public GameObject healthSlider = null; // Needs to be public as they are accessed by attacking enemies
    public PlayerHealth healthScript; // Needs to be public as they are accessed by attacking enemies

    private Rigidbody rigidBody;
    private float playerHeight;
    private Camera cameraGO;

    private void Start()
    {
        healthScript = new PlayerHealth(maxHealth: playerMaxHealth);
        healthSlider.transform.localScale = new Vector3(1,1,1); // Sets the health slider to full on start.
        cameraGO = this.GetComponentInChildren<Camera>();
        // GetComponentInChildren<Text>().text = PhotonNetwork.NickName; // Sets the name tag above the player.
        playerHeight = GetComponent<Collider>().bounds.extents.y;

        if (!photonView.IsMine)
        {
            cameraGO.GetComponent<Camera>().enabled = false;
        }

        rigidBody = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            LightAttack();
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
        rigidBody.velocity = dir;

        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 playerRotation = new Vector3(0, mouseX, 0) * mouseSensitivity;
        transform.Rotate(playerRotation);

        Vector3 cameraRotation = new Vector3(-mouseY, 0, 0) * mouseSensitivity;
        cameraGO.transform.Rotate(cameraRotation);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.velocity = Vector2.up * jumpThrust;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, playerHeight + 0.1f);
    }

    private void LightAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraGO.transform.forward, out hit, hitDistance, marineLayerMask))
        {
            AlienController hitPlayer = hit.transform.gameObject.GetComponent<AlienController>();
            float newHealth = hitPlayer.healthScript.PlayerHit(damage: 5);
            hitPlayer.healthSlider.transform.localScale = new Vector3 (newHealth / hitPlayer.playerMaxHealth, 1, 1); // !!IMPORTANT!! Change this to marine movement/controller script at a later date!!!!
        }

        Debug.DrawRay(transform.position, cameraGO.transform.forward * 100, Color.red);
        
        Debug.Log(PhotonNetwork.NickName + " (Alien) did a light attack");
    }
}
