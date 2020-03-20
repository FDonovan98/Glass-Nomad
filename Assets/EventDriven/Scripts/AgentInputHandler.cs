using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentInputHandler : MonoBehaviour
{
    public MovementValues movementValues;
    public CommandObject[] commandList;

    private void Start()
    {
        movementValues.Initialise();
    }

    private void Update()
    {
        foreach (CommandObject element in commandList)
        {
            element.Execute(this.gameObject, movementValues);
        }
    }
}
