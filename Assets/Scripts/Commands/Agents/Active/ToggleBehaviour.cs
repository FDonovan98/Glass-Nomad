using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "DefaultToggleBehaviour", menuName = "Commands/Active/ToggleBehaviour", order = 0)]
public class ToggleBehaviour : ActiveCommandObject
{
    [SerializeField]
    KeyCode toggleBehaviour = KeyCode.F;
    
    protected override void OnEnable()
    {
        keyTable.Add("Toggle Item", toggleBehaviour);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        if (agentInputHandler.isLocalAgent)
        {
            agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        }
    }

    private void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (Input.GetKeyDown(toggleBehaviour))
        {
            // The RPC is found in PunRPCs.cs
            agent.GetComponent<PhotonView>().RPC("Toggle", RpcTarget.All, agent.GetComponent<PhotonView>().ViewID);
        }
    }
}