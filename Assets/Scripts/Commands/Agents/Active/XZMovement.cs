using UnityEngine;

[CreateAssetMenu(fileName = "DefaultXZMovement", menuName = "Commands/Active/XZ Movement")]
public class XZMovement : ActiveCommandObject
{
    [SerializeField]
    private string HorizontalMovement = "Horizontal";
    [SerializeField]
    private string VerticalMovement = "Vertical";
    [SerializeField]

    protected override void OnEnable()
    {
        keyTable.Add("Horizontal", HorizontalMovement);
        keyTable.Add("Vertical", VerticalMovement);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        if (agentInputHandler.isLocalAgent)
        {
            agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        }
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        Vector3 inputMovementVector = GetKeyInput(agent);

        if (agentInputHandler.allowInput)
        {
            inputMovementVector *= agentValues.moveAcceleration * Time.deltaTime * agentInputHandler.moveSpeedMultiplier;

            agentInputHandler.agentRigidbody.velocity += inputMovementVector * agentInputHandler.moveSpeedMultiplier;

        }
        else
        {
            inputMovementVector = Vector3.zero;
        }
        

        if (agentInputHandler.isGrounded)
        {
            VelocityDegradation(agentValues.velocityDegradationGrounded, inputMovementVector, agentInputHandler);
        }
        else if (agentValues.reduceVelocityInAir)
        {
            VelocityDegradation(agentValues.velocityDegradationInAir, inputMovementVector, agentInputHandler);
        }
    }

    Vector3 GetKeyInput(GameObject agent)
    {
        Vector3 inputMovementVector = Vector3.zero;

        if (Input.GetAxis(VerticalMovement) > 0)
        {
            inputMovementVector += agent.transform.forward;
        }
        if (Input.GetAxis(VerticalMovement) < 0)
        {
            inputMovementVector -= agent.transform.forward;
        }
        if (Input.GetAxis(HorizontalMovement) < 0)
        {
            inputMovementVector -= agent.transform.right;
        }
        if (Input.GetAxis(HorizontalMovement) > 0)
        {
            inputMovementVector += agent.transform.right;
        }

        return inputMovementVector.normalized;
    }

    void VelocityDegradation(float velocityDegradationValue, Vector3 inputMovementVector, AgentInputHandler agentInputHandler)
    {
        if (!agentInputHandler.isJumping)
        {
            Vector3 localVel = agentInputHandler.agentRigidbody.transform.worldToLocalMatrix * agentInputHandler.agentRigidbody.velocity;
            float RelativeVelDeg = velocityDegradationValue * Time.deltaTime;
            inputMovementVector = agentInputHandler.agentRigidbody.transform.worldToLocalMatrix * inputMovementVector;

            float[] xzVel = 
            {
                localVel.x,
                localVel.z
            };

            float[] xzInput = 
            {
                inputMovementVector.x,
                inputMovementVector.z
            };

            for (int i = 0; i < 2; i++)
            {
                if (xzInput[i] == 0.0f)
                {
                    if (xzVel[i] > 0.0f)
                    {
                        xzVel[i] = Mathf.Clamp(xzVel[i] - RelativeVelDeg, 0.0f, xzVel[i]);
                    }
                    else if (xzVel[i] < 0.0f)
                    {
                        xzVel[i] = Mathf.Clamp(xzVel[i] + RelativeVelDeg, xzVel[i], 0.0f);
                    } 
                }
                else if (xzInput[i] > 0.0f && xzVel[i] < 0.0f)
                {
                    xzVel[i] = Mathf.Clamp(xzVel[i] + RelativeVelDeg, xzVel[i], 0.0f);
                }
                else if (xzInput[i] < 0.0f && xzVel[i] > 0.0f)
                {
                    xzVel[i] = Mathf.Clamp(xzVel[i] - RelativeVelDeg, 0.0f, xzVel[i]);
                }
            }

            localVel = new Vector3(xzVel[0], localVel.y, xzVel[1]);

            agentInputHandler.agentRigidbody.velocity = agentInputHandler.agentRigidbody.transform.localToWorldMatrix * localVel;
        }
    }
}
