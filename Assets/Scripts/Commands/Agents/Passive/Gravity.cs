using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "DefaultGravity", menuName = "Commands/Passive/Gravity")]
public class Gravity : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnFixedUpdate += RunCommandOnFixedUpdate;
    }
    void RunCommandOnFixedUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (!agent.GetComponent<PhotonView>().IsMine) return;
        agentInputHandler.agentRigidbody.velocity += agentInputHandler.gravityDirection.normalized * agentValues.gravityAcceleration * Time.fixedDeltaTime;
    }
}