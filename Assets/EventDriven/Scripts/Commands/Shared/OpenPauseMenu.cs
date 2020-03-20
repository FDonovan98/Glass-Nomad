using UnityEngine;

[CreateAssetMenu(fileName = "OpenPauseMenu", menuName = "Commands/Open Pause Menu")]
public class OpenPauseMenu : CommandObject
{
    [SerializeField]
    private KeyCode openMenuKey = KeyCode.Escape;
    [SerializeField]
    private KeyCode openMenuKeyInEditor = KeyCode.Comma;

    private MovementValues movementValuesRef = null;

    protected override void OnEnable()
    {
        keyTable.Add("Pause", openMenuKey);
    }

    public override void Execute(GameObject agent, MovementValues movementValues)
    {
        if (movementValuesRef == null)
        {
            movementValuesRef = movementValues;
        }

        #if UNITY_EDITOR
            //Press the openMenuKeyInEditor to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(openMenuKeyInEditor))
            {
                if (Cursor.lockState == CursorLockMode.Locked) ToggleCursorAndMenu(true);
                else ToggleCursorAndMenu(false);
            } 
        #elif UNITY_STANDALONE_WIN
            //Press the openMenuKey to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(openMenuKey))
            {
                if (Cursor.lockState == CursorLockMode.Locked) ToggleCursorAndMenu(true);
                else ToggleCursorAndMenu(false);
            } 
        #endif
    }

    private void ToggleCursorAndMenu(bool turnOn)
    {
        Cursor.lockState = turnOn ? CursorLockMode.None : CursorLockMode.Locked;
        ToggleMenu(turnOn);
    }

    private void ToggleMenu(bool toggle)
    {
        movementValuesRef.menu.SetActive(toggle);
        Cursor.visible = toggle;
    }
}