using UnityEngine;
using Photon.Pun;

public class PunRPCs : MonoBehaviourPunCallbacks
{
    [PunRPC]
    public void WallWasHit(Vector3 cameraPos, Vector3 cameraForward, float weaponRange, int weaponDamage)
    {     
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraForward, out hit, weaponRange))
        {
            AgentInputHandler targetAgentInputHandler = hit.transform.gameObject.GetComponent<AgentInputHandler>();

            Debug.Log(targetAgentInputHandler);
            
            if (targetAgentInputHandler != null)
            {
                if (targetAgentInputHandler.runCommandOnAgentHasBeenHit != null)
                {
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
        AgentInputHandler targetAgentInputHandler = GetInputHandler(hitPlayerViewID);

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

    [PunRPC]
    public void Toggle(int playersViewID)
    {
        AgentInputHandler agentInputHandler = GetInputHandler(playersViewID);
        agentInputHandler.behaviourToToggle.enabled = !agentInputHandler.behaviourToToggle.isActiveAndEnabled;
    }

    private AgentInputHandler GetInputHandler(int viewId)
    {
        return PhotonNetwork.GetPhotonView(viewId).GetComponent<AgentController>();
    }
}
