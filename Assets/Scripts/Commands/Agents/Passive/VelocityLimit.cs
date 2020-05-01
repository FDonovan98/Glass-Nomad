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
            if (agentInputHandler.isGrounded)
            {
                if (agentInputHandler.isSprinting)
                {    
                    if (agentInputHandler.agentRigidbody.velocity.magnitude > agentValues.maxSprintSpeed)
                    {
                        LimitVelocity(agentInputHandler, agentValues.maxSprintSpeed, agentValues);
                    }
                }
                else
                {
                    if (agentInputHandler.agentRigidbody.velocity.magnitude > agentValues.maxSpeed * agentInputHandler.moveSpeedMultiplier)
                    {
                        LimitVelocity(agentInputHandler, agentValues.maxSpeed, agentValues);
                    }
                }
            }
            else
            {
                if (agentInputHandler.isSprinting)
                {    
                    if (agentInputHandler.agentRigidbody.velocity.magnitude > agentValues.maxSprintSpeedInAir * (agentInputHandler.moveSpeedMultiplier / agentValues.sprintMultiplier))
                    {
                        LimitVelocity(agentInputHandler, agentValues.maxSprintSpeedInAir, agentValues);
                    }
                }
                else
                {
                    if (agentInputHandler.agentRigidbody.velocity.magnitude > agentValues.maxSpeedInAir * agentInputHandler.moveSpeedMultiplier)
                    {
                        LimitVelocity(agentInputHandler, agentValues.maxSpeedInAir, agentValues);
                    }
                }
            }
        }
    }

    void LimitVelocity(AgentInputHandler agentInputHandler, float limitValue, AgentValues agentValues)
    {
        Vector3 newVel = agentInputHandler.agentRigidbody.velocity.normalized * limitValue * agentInputHandler.moveSpeedMultiplier;
        newVel.y = agentInputHandler.agentRigidbody.velocity.y;

        agentInputHandler.agentRigidbody.velocity = Vector3.Lerp(agentInputHandler.agentRigidbody.velocity, newVel, agentValues.velocityLimitRate);
    }
}