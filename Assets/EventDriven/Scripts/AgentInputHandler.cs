using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentInputHandler : MonoBehaviour
{
    public AgentValues agentValues;
    public CommandObject[] activeCommands;
    public CommandObject[] passiveCommands;

    private void Start()
    {
        agentValues.Initialise();
    }

    private void Update()
    {
        foreach (CommandObject element in activeCommands)
        {
            element.Execute(this.gameObject, agentValues);
        }

        foreach (CommandObject element in passiveCommands)
        {
            element.Execute(this.gameObject, agentValues);
        }
    }
}
