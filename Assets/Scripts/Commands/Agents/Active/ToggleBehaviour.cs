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
            agent.GetComponent<PhotonView>().RPC("Toggle", RpcTarget.All, agent.GetComponent<PhotonView>().ViewID);
            agentInputHandler.behaviourToToggle.enabled = !agentInputHandler.behaviourToToggle.isActiveAndEnabled;
        }
    }

    [PunRPC]
    public void Toggle(int playersViewID)
    {
        AgentInputHandler agentInputHandler = PhotonNetwork.GetPhotonView(playersViewID).GetComponent<AgentController>();
        agentInputHandler.behaviourToToggle.enabled = !agentInputHandler.behaviourToToggle.isActiveAndEnabled;
    }
}