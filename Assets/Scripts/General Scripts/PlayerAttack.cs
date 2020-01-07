using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerAttack : MonoBehaviourPunCallbacks
{
    // Used to change the health bar slider above the player.
    [SerializeField] 
    public Image healthSlider = null; 

    // Used to set the player's health the max, on initialisation.
    [SerializeField] 
    private int maxHealth = 100; 
    
    // Used to control the health of this player.
    public PlayerHealth healthScript;     
    // Used to disable/enable the camera so that we only control our local player's camera.
    private GameObject cameraGO; 

    private void Start()
    {
        healthScript = new PlayerHealth(this.gameObject, maxHealth);

        // Gets the camera child on the player.
        cameraGO = this.GetComponentInChildren<Camera>().gameObject; 

        WeaponClass rifle = new WeaponClass(3, 2, 20, 20, 40);
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            // Calls the 'Attack' method on all clients, meaning that the health will be synced across all clients.
            photonView.RPC("FireWeapon", RpcTarget.All);
        }
    }
}
