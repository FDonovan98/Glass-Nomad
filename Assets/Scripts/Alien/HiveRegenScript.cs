using UnityEngine;
using Photon.Pun;

public class HiveRegenScript : MonoBehaviourPunCallbacks
{
    #region variable-declaration

    // Used to keep track of how often to regen health.
    private float deltaTime = 0.0f;

    // Used to update the canvas, when the alien regens health.
    private UIBehaviour hudCanvas;

    #endregion

    /// <summary>
    /// Assigns the hudCanvas variable so that it can be updated when regenerating health.
    /// </summary>
    private void Start()
    {
        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
    }
    
    /// <summary>
    /// When the alien enters the hive regeneration collider, their health will begin regen
    /// and their HUD will update.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GameObject alien = coll.gameObject;
            int viewID = alien.GetPhotonView().ViewID;
            deltaTime += Time.deltaTime;
            if (deltaTime >= 0.2f)
            {
                // PunRPC is in AlienController.cs.
                // Update the alien's health, using an RPC so all clients update it, too.
                alien.GetPhotonView().RPC("RegenHealth", RpcTarget.All, viewID, -1);
                
                // Update the alien's UI, with its new health.
                hudCanvas.UpdateUI(coll.GetComponent<PlayerAttack>());

                // Reset the timer.
                deltaTime = 0.0f;
            }
        }
    }
}
