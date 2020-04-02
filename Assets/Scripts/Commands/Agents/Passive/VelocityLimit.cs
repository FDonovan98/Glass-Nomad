using UnityEngine;

[CreateAssetMenu(fileName = "DefaultVelocityLimit", menuName = "Commands/Passive/VelocityLimit", order = 0)]
public class VelocityLimit : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (!agentInputHandler.isJumping)
        {
            Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();

            if (agentRigidbody.velocity.magnitude > agentValues.maxSpeed && agentInputHandler.isGrounded)
            {
                agentRigidbody.velocity = agentRigidbody.velocity.normalized * agentValues.maxSpeed;
            }

            if (agentRigidbody.velocity.magnitude > agentValues.maxSprintSpeed && agentInputHandler.isGrounded)
            {
                agentRigidbody.velocity = agentRigidbody.velocity.normalized * agentValues.maxSprintSpeed;
            }
        }
    }
}