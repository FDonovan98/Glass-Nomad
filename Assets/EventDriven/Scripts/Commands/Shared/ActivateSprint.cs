using UnityEngine;

[CreateAssetMenu(fileName = "DefaultActivateSprint", menuName = "Commands/Activate Sprint")]
public class ActivateSprint : CommandObject
{
    [SerializeField]
    private KeyCode sprintKeyCode = KeyCode.LeftShift;

    protected override void OnEnable()
    {
        keyTable.Add("Sprint", sprintKeyCode);
    }

    public override void Execute(GameObject agent, AgentValues agentValues)
    {
        if (agentValues.sprintingIsAToggle)
        {
            if (Input.GetKeyDown(sprintKeyCode))
            {
                agentValues.isSprinting = !agentValues.isSprinting;
            }
        }
        else
        {
            if (Input.GetKeyDown(sprintKeyCode))
            {
                agentValues.isSprinting = true;
            }

            if (Input.GetKeyUp(sprintKeyCode))
            {
                agentValues.isSprinting = false;
            }
        }


    }
}
