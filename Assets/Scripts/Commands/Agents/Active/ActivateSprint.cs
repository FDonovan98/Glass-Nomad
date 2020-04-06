using UnityEngine;

[CreateAssetMenu(fileName = "DefaultActivateSprint", menuName = "Commands/Active/Activate Sprint")]
public class ActivateSprint : ActiveCommandObject
{
    [SerializeField]
    private KeyCode sprintKeyCode = KeyCode.LeftShift;

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
            if (Input.GetKeyDown(sprintKeyCode))
            {
                agentInputHandler.isSprinting = !agentInputHandler.isSprinting;
            }
        }
        else
        {
            if (Input.GetKeyDown(sprintKeyCode))
            {
                agentInputHandler.isSprinting = true;
            }
            else if (Input.GetKeyUp(sprintKeyCode))
            {
                agentInputHandler.isSprinting = false;
            }
        }


    }
}
