using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DefaultChargeLeap", menuName = "Commands/Active/Charge Leap")]
public class ChargeLeap : ActiveCommandObject
{
    [SerializeField] private KeyCode chargeLeap = KeyCode.Space;
    private float timeJumpingFor = 0.0f;

    protected override void OnEnable()
    {
        keyTable.Add("Charge Leap", chargeLeap);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        if (agentInputHandler.isLocalAgent)
        {
            agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        }
    }

    private void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {   
        AgentController agentController = (AgentController)agentInputHandler;

        if (agentInputHandler.isJumping)
        {
            timeJumpingFor += Time.deltaTime;
            if (timeJumpingFor > agentValues.jumpCooldown)
            {
                agentInputHandler.isJumping = false;
                timeJumpingFor = 0.0f;
            }
        }

        if (agentValues.leapCanChargeInAir || agentInputHandler.isGrounded)
        {
            if (Input.GetKey(chargeLeap) && !agentInputHandler.isJumping)
            {
                agentInputHandler.currentLeapCharge += Time.deltaTime;
                float percentage = (agentInputHandler.currentLeapCharge / agentValues.leapChargeDuration) * 100;
                if (percentage >= 100) percentage = 99.9f;
                ReticleProgress.UpdateReticleProgress(percentage, agentController.progressBar);
            }

            if (Input.GetKeyUp(chargeLeap))
            {
                if (agentInputHandler.isGrounded)
                {
                    ReticleProgress.UpdateReticleProgress(0, agentController.progressBar);
                    agentInputHandler.isJumping = true;

                    float jumpImpulse = Mathf.Min(agentInputHandler.currentLeapCharge, agentValues.leapChargeDuration);

                    jumpImpulse /= agentValues.leapChargeDuration;
                    jumpImpulse *= agentValues.leapVelocity * agentInputHandler.moveSpeedMultiplier;

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
}