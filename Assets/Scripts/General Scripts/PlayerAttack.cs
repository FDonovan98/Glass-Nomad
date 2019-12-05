using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerAttack : MonoBehaviourPunCallbacks
{
    [SerializeField] private LayerMask hitLayerMask = new LayerMask(); // Used to control which layers the player can hit.
    [SerializeField] private int hitDistance = 1; // Used to control how far the alien can hit.
    [SerializeField] private int playerDamage = 25; // Used to damage the other players.
    [SerializeField] private Image healthSlider = null; // Used to change the health bar slider above the player.
    [SerializeField] private int maxHealth = 100; // Used to set the player's health the max, on initialisation.
    public PlayerHealth healthScript; // Used to control the health of this player.
    private Camera cameraGO; // Used to disable/enable the camera so that we only control our local player's camera.

    private void Start()
    {
        healthScript = new PlayerHealth(maxHealth);
        cameraGO = this.GetComponentInChildren<Camera>(); // Gets the camera child on the player.
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
            photonView.RPC("Attack", RpcTarget.All);
        }
    }

    [PunRPC] // Important as this is needed to be able to be called by the PhotonView.RPC().
    private void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraGO.transform.forward, out hit, hitDistance, hitLayerMask))
        {
            PlayerAttack hitPlayer = hit.transform.gameObject.GetComponent<PlayerAttack>();
            PlayerHealth hitPlayerHealth = hitPlayer.healthScript;

            hitPlayerHealth.PlayerHit(damage: playerDamage);
            hitPlayer.healthSlider.fillAmount = hitPlayerHealth.fillAmount;
        }

        Debug.DrawRay(transform.position, cameraGO.transform.forward * hitDistance, Color.red);

        Debug.Log(PhotonNetwork.NickName + " did a light attack");
    }
}
