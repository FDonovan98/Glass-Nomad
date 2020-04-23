using UnityEngine;

[CreateAssetMenu(fileName = "DefaultActivateSprint", menuName = "Commands/Active/Activate Sprint")]
public class ActivateSprint : ActiveCommandObject
{
    [SerializeField]
    private string sprintKeyCode = "Sprint";

    protected override void OnEnable()
    {
        keyTable.Add("Sprint", sprintKeyCode);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (agentValues.sprintingIsAToggle)
        {
            if (Input.GetButtonDown(sprintKeyCode))
            {
                agentInputHandler.isSprinting = !agentInputHandler.isSprinting;

                if (agentInputHandler.isSprinting)
                {
                    agentInputHandler.ChangeMovementSpeedModifier(agentValues.sprintMultiplier, true);
                }
                else
                {
                    agentInputHandler.ChangeMovementSpeedModifier(agentValues.sprintMultiplier, false);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown(sprintKeyCode))
            {
                agentInputHandler.isSprinting = true;
                agentInputHandler.ChangeMovementSpeedModifier(agentValues.sprintMultiplier, true);
            }
            else if (Input.GetButtonUp(sprintKeyCode))
            {
                agentInputHandler.isSprinting = false;
                agentInputHandler.ChangeMovementSpeedModifier(agentValues.sprintMultiplier, false);
            }
        }


    }
}
