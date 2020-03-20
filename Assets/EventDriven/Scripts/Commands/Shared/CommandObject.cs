using UnityEngine;

public abstract class CommandObject : ScriptableObject
{
    [SerializeField]
    public KeyCode keycode;

    public abstract void Execute(GameObject agent, MovementValues movementValues);

    public virtual void ChangeKeycode(string newKeycode)
    {
        char[] charKeycode = newKeycode.ToCharArray();
        keycode = (KeyCode)charKeycode[0];
    }

    // I am unsure whether this would be helpful or redundant. 
    // My current thought is redundant so it will remain commented out for now.
    // public virtual bool ExecuteCondition()
    // {
    //     return true;
    // }
}
