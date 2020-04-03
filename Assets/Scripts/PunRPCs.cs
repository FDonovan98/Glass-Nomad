using UnityEngine;

using Photon.Pun;

public class PunRPCs : MonoBehaviourPunCallbacks
{
    [PunRPC]
    public void WallWasHit(Vector3 cameraPos, Vector3 cameraForward, float weaponRange, int weaponDamage)
    {     
        Debug.Log("shoot");
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraForward, out hit, weaponRange))
        {
            Debug.Log(hit.transform.gameObject.name + " has been hit");
            AgentInputHandler targetAgentInputHandler = hit.transform.gameObject.GetComponent<AgentInputHandler>();

            Debug.Log(targetAgentInputHandler);
            
            if (targetAgentInputHandler != null)
            {
                if (targetAgentInputHandler.runCommandOnAgentHasBeenHit != null)
                {
                    Debug.Log("hit feedback");
                    targetAgentInputHandler.runCommandOnAgentHasBeenHit(targetAgentInputHandler, hit.point, hit.normal, weaponDamage);
                }
            }
            else
            {
                Debug.LogWarning(hit.transform.gameObject.name + " has no agentInputHandler");
            }
        }
    }

    [PunRPC]
    public void PlayerWasHit(int hitPlayerViewID, Vector3 hitPos, Vector3 hitNormal, int weaponDamage)
    {
        AgentInputHandler targetAgentInputHandler = PhotonNetwork.GetPhotonView(hitPlayerViewID).GetComponent<AgentInputHandler>();

        if (targetAgentInputHandler != null)
        {
            if (targetAgentInputHandler.runCommandOnAgentHasBeenHit != null)
                {
                    targetAgentInputHandler.runCommandOnAgentHasBeenHit(targetAgentInputHandler, hitPos, hitNormal, weaponDamage);
                }
        }
        else
        {
            Debug.LogWarning(targetAgentInputHandler.gameObject.name + " has no agentInputHandler");
        }
    }
}
