using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HiveRegenScript : MonoBehaviourPunCallbacks
{
    private float deltaTime = 0.0f;

    GameObject alien;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            alien = other.gameObject;
            int viewID = alien.GetPhotonView().ViewID;
            deltaTime += Time.deltaTime;
            if (deltaTime >= 0.2f)
            {
                alien.GetPhotonView().RPC("RegenHealth", RpcTarget.All, viewID, deltaTime);
                deltaTime = 0.0f;
            }
        }
    }

}
