using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGravity", menuName = "Commands/Passive/Gravity")]
public class Gravity : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnFixedUpdate += RunCommandOnFixedUpdate;
    }
    void RunCommandOnFixedUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        agentInputHandler.agentRigidbody.velocity += agentInputHandler.gravityDirection.normalized * agentValues.gravityAcceleration * Time.fixedDeltaTime;
    }
}