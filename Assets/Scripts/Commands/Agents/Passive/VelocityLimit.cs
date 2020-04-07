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

            if (agentInputHandler.isGrounded)
            {
                if (agentInputHandler.isSprinting)
                {    
                    if (agentRigidbody.velocity.magnitude > agentValues.maxSprintSpeed)
                    {
                        LimitVelocity(agentRigidbody, agentValues.maxSprintSpeed, agentValues);
                    }
                }
                else
                {
                    if (agentRigidbody.velocity.magnitude > agentValues.maxSpeed)
                    {
                        LimitVelocity(agentRigidbody, agentValues.maxSpeed, agentValues);
                    }
                }
            }
            else
            {
                if (agentInputHandler.isSprinting)
                {    
                    if (agentRigidbody.velocity.magnitude > agentValues.maxSprintSpeedInAir)
                    {
                        LimitVelocity(agentRigidbody, agentValues.maxSprintSpeedInAir, agentValues);
                    }
                }
                else
                {
                    if (agentRigidbody.velocity.magnitude > agentValues.maxSpeedInAir)
                    {
                        LimitVelocity(agentRigidbody, agentValues.maxSpeedInAir, agentValues);
                    }
                }
            }
        }
    }

    void LimitVelocity(Rigidbody rigidbody, float limitValue, AgentValues agentValues)
    {
        Vector3 newVel = rigidbody.velocity.normalized * limitValue;
        newVel.y = rigidbody.velocity.y;

        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, newVel, agentValues.velocityLimitRate);
    }
}