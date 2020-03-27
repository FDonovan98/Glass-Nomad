using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGravity", menuName = "Commands/Passive/Gravity")]
public class Gravity : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnFixedUpdate += RunCommandOnFixedUpdate;
    }
    void RunCommandOnFixedUpdate(GameObject agent, AgentValues agentValues)
    {
        Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();
        agentRigidbody.velocity += agentValues.gravityDirection.normalized * agentValues.gravityAcceleration * Time.fixedDeltaTime;
    }
}