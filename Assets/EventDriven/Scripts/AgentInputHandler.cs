using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentInputHandler : MonoBehaviour
{
    public MovementValues movementValues;
    public CommandObject[] activeCommands;
    public CommandObject[] passiveCommands;

    private void Start()
    {
        movementValues.Initialise();
    }

    private void Update()
    {
        foreach (CommandObject element in activeCommands)
        {
            element.Execute(this.gameObject, movementValues);
        }

        foreach (CommandObject element in passiveCommands)
        {
            element.Execute(this.gameObject, movementValues);
        }
    }
}
