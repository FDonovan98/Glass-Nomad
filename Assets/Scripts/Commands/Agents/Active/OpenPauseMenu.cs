using UnityEngine;

[CreateAssetMenu(fileName = "OpenPauseMenu", menuName = "Commands/Active/Open Pause Menu")]
public class OpenPauseMenu : ActiveCommandObject
{
    [SerializeField]
    private KeyCode openMenuKey = KeyCode.Escape;
    [SerializeField]
    private KeyCode openMenuKeyInEditor = KeyCode.Comma;

    protected override void OnEnable()
    {
        keyTable.Add("Pause", openMenuKey);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        #if UNITY_EDITOR
            //Press the openMenuKeyInEditor to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(openMenuKeyInEditor))
            {
                if (Cursor.lockState == CursorLockMode.Locked) ToggleCursorAndMenu(true, agentInputHandler, agentValues);
                else ToggleCursorAndMenu(false, agentInputHandler, agentValues);
            } 
        #elif UNITY_STANDALONE_WIN
            //Press the openMenuKey to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(openMenuKey))
            {
                if (Cursor.lockState == CursorLockMode.Locked) ToggleCursorAndMenu(true, agentInputHandler, agentValues);
                else ToggleCursorAndMenu(false, agentInputHandler, agentValues);
            } 
        #endif
    }

    private void ToggleCursorAndMenu(bool turnOn, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        Cursor.lockState = turnOn ? CursorLockMode.None : CursorLockMode.Locked;
        ToggleMenu(turnOn, agentInputHandler, agentValues);
    }

    private void ToggleMenu(bool toggle, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        agentValues.menu.SetActive(toggle);
        agentInputHandler.allowInput = !toggle;
        Cursor.visible = toggle;
    }
}