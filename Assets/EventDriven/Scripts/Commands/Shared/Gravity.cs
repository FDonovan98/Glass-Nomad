using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGravity", menuName = "Commands/Passive/Gravity")]
public class Gravity : PassiveCommandObject
{
    public override void Execute(GameObject agent, AgentValues agentValues)
    {
        Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();
        agentRigidbody.velocity += agentValues.gravityDirection * agentValues.gravityAcceleration * Time.deltaTime;
    }
}