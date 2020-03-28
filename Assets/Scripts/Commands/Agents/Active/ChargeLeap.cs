using UnityEngine;

[CreateAssetMenu(fileName = "DefaultChargeLeap", menuName = "Commands/Active/Charge Leap")]
public class ChargeLeap : ActiveCommandObject
{
    [SerializeField] 
    KeyCode chargeLeap = KeyCode.Space;

    protected override void OnEnable()
    {
        keyTable.Add("Charge Leap", chargeLeap);
    }
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {        
        if (agentValues.leapCanChargeInAir || agentInputHandler.isGrounded)
            if (Input.GetKey(chargeLeap))
            {
                agentInputHandler.currentLeapCharge += Time.deltaTime;
            }

            if (Input.GetKeyUp(chargeLeap))
            {
                if (agentInputHandler.isGrounded)
                {
                    float jumpImpulse = Mathf.Min(agentInputHandler.currentLeapCharge, agentValues.leapChargeDuration);

                    jumpImpulse /= agentValues.leapChargeDuration;
                    jumpImpulse *= agentValues.leapVelocity;

                    Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();
                    Camera agentCamera = agent.GetComponent<Camera>();
                    if (agentCamera == null)
                    {
                        agentCamera = agent.GetComponentInChildren<Camera>();
                    }

                    agentRigidbody.velocity += agentValues.forwardLeapModifier * jumpImpulse * agentCamera.transform.forward;
                    agentRigidbody.velocity += agentValues.verticalLeapModifier * jumpImpulse * agent.transform.up;
                }

                agentInputHandler.currentLeapCharge = 0.0f;
            }
    }
}