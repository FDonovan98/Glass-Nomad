using UnityEngine;
using Photon.Pun;

public class HiveRegenScript : MonoBehaviourPunCallbacks
{
    // Keeps track of how often to regen health
    private float deltaTime = 0.0f;

    private GameObject alien;

    /// <summary>
    /// When the alien enters the hive regeneration collider, their health will begin regen
    /// and their HUD will update.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Player")
        {
            alien = coll.gameObject;
            int viewID = alien.GetPhotonView().ViewID;
            deltaTime += Time.deltaTime;
            if (deltaTime >= 0.2f)
            {
                // PunRPC is in AlienController.cs.
                alien.GetPhotonView().RPC("RegenHealth", RpcTarget.All, viewID, -1);
                deltaTime = 0.0f;
            }
        }
    }
}
