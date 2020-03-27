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

    void RunCommandOnUpdate(GameObject agent, AgentValues agentValues)
    {
        #if UNITY_EDITOR
            //Press the openMenuKeyInEditor to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(openMenuKeyInEditor))
            {
                if (Cursor.lockState == CursorLockMode.Locked) ToggleCursorAndMenu(true, agentValues);
                else ToggleCursorAndMenu(false, agentValues);
            } 
        #elif UNITY_STANDALONE_WIN
            //Press the openMenuKey to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(openMenuKey))
            {
                if (Cursor.lockState == CursorLockMode.Locked) ToggleCursorAndMenu(true, agentValues);
                else ToggleCursorAndMenu(false, agentValues);
            } 
        #endif
    }

    private void ToggleCursorAndMenu(bool turnOn, AgentValues agentValues)
    {
        Cursor.lockState = turnOn ? CursorLockMode.None : CursorLockMode.Locked;
        ToggleMenu(turnOn, agentValues);
    }

    private void ToggleMenu(bool toggle, AgentValues agentValues)
    {
        agentValues.menu.SetActive(toggle);
        agentValues.allowInput = !toggle;
        Cursor.visible = toggle;
    }
}