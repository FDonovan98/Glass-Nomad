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

    void RunCommandOnUpdate(GameObject agent, AgentValues agentValues)
    {
        if (agentValues.leapCanChargeInAir || agentValues.isGrounded)
            if (Input.GetKey(chargeLeap))
            {
                agentValues.currentLeapCharge += Time.deltaTime;
            }

            if (Input.GetKeyUp(chargeLeap))
            {
                if (agentValues.isGrounded)
                {
                    float jumpImpulse = Mathf.Min(agentValues.currentLeapCharge, agentValues.leapChargeDuration);

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

                agentValues.currentLeapCharge = 0.0f;
            }
    }
}