using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HiveRegenScript : MonoBehaviourPunCallbacks
{
    private float deltaTime = 0.0f;
    private GameObject alien;
    private UIBehaviour hudCanvas;

    private void Start()
    {
        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            alien = other.gameObject;
            int viewID = alien.GetPhotonView().ViewID;
            deltaTime += Time.deltaTime;
            if (deltaTime >= 0.2f)
            {
                // PunRPC is in AlienController.cs.
                alien.GetPhotonView().RPC("RegenHealth", RpcTarget.All, viewID, -1);
                hudCanvas.UpdateUI(other.GetComponent<PlayerAttack>());
                deltaTime = 0.0f;
            }
        }
    }

}
