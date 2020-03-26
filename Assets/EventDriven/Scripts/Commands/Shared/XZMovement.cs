using UnityEngine;

[CreateAssetMenu(fileName = "DefaultXZMovement", menuName = "Commands/Active/XZ Movement")]
public class XZMovement : ActiveCommandObject
{
    [SerializeField]
    private KeyCode MoveForward = KeyCode.W;
    [SerializeField]
    private KeyCode MoveBack = KeyCode.S;
    [SerializeField]
    private KeyCode MoveLeft = KeyCode.A;
    [SerializeField]
    private KeyCode MoveRight = KeyCode.D;

    protected override void OnEnable()
    {
        keyTable.Add("Move Forward", MoveForward);
        keyTable.Add("Move Back", MoveBack);
        keyTable.Add("Move Left", MoveLeft);
        keyTable.Add("Move Right", MoveRight);
    }

    public override void Execute(GameObject agent, AgentValues agentValues)
    {
        Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();


        Vector3 inputMovementVector = GetKeyInput(agent);

        VelocityDegradation(agentRigidbody, agentValues, inputMovementVector);

        inputMovementVector *= agentValues.moveSpeed * Time.deltaTime;

        if (agentValues.isSprinting)
        {
            agentRigidbody.velocity += inputMovementVector * agentValues.sprintMultiplier;
        }
        else
        {
            agentRigidbody.velocity += inputMovementVector;
        }

        if (agentRigidbody.velocity.magnitude > agentValues.maxSpeed)
        {
            agentRigidbody.velocity = agentRigidbody.velocity.normalized * agentValues.maxSpeed;
        }
    }

    Vector3 GetKeyInput(GameObject agent)
    {
        Vector3 inputMovementVector = Vector3.zero;

        if (Input.GetKey(MoveForward))
        {
            inputMovementVector += agent.transform.forward;
        }
        if (Input.GetKey(MoveBack))
        {
            inputMovementVector -= agent.transform.forward;
        }
        if (Input.GetKey(MoveLeft))
        {
            inputMovementVector -= agent.transform.right;
        }
        if (Input.GetKey(MoveRight))
        {
            inputMovementVector += agent.transform.right;
        }

        return inputMovementVector.normalized;
    }

    void VelocityDegradation(Rigidbody agentRigidbody, AgentValues agentValues, Vector3 inputMovementVector)
    {
        if (agentValues.reduceVelocityInAir || agentValues.isGrounded)
        {
            Vector3 localVel = agentRigidbody.transform.worldToLocalMatrix * agentRigidbody.velocity;
            float RelativeVelDeg = agentValues.velocityDegradationValue * Time.deltaTime;
            inputMovementVector = agentRigidbody.transform.worldToLocalMatrix * inputMovementVector;

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

            agentRigidbody.velocity = agentRigidbody.transform.localToWorldMatrix * localVel;
        }
    }
}
