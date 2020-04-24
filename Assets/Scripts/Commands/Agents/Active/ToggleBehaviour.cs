using UnityEngine;

[CreateAssetMenu(fileName = "DefaultToggleBehaviour", menuName = "Commands/Active/ToggleBehaviour", order = 0)]
public class ToggleBehaviour : ActiveCommandObject
{
    [SerializeField]
    KeyCode toggleBehaviour = KeyCode.F;
    
    protected override void OnEnable()
    {
        keyTable.Add("Toggle Item", toggleBehaviour);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (Input.GetKeyDown(toggleBehaviour))
        {
            agentInputHandler.behaviourToToggle.enabled = !agentInputHandler.behaviourToToggle.isActiveAndEnabled;
        }
    }
}