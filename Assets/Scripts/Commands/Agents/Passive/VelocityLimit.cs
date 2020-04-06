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

            if (agentInputHandler.isGrounded || agentValues.reduceVelocityInAir)
            {
                if (agentInputHandler.isSprinting)
                {    
                    if (agentRigidbody.velocity.magnitude > agentValues.maxSprintSpeed)
                    {
                        agentRigidbody.velocity = agentRigidbody.velocity.normalized * agentValues.maxSprintSpeed;
                    }
                }
                else
                {
                    if (agentRigidbody.velocity.magnitude > agentValues.maxSpeed)
                    {
                        agentRigidbody.velocity = agentRigidbody.velocity.normalized * agentValues.maxSpeed;
                    }
                }

            }
        }
    }
}