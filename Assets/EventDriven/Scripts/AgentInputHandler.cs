using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentInputHandler : MonoBehaviour
{
    public AgentValues agentValues;
    public ActiveCommandObject[] activeCommands;
    public ActiveCommandObject[] passiveCommands;

    private void Start()
    {
        agentValues.Initialise();
    }

    private void Update()
    {
        foreach (ActiveCommandObject element in activeCommands)
        {
            element.Execute(this.gameObject, agentValues);
        }

        foreach (ActiveCommandObject element in passiveCommands)
        {
            element.Execute(this.gameObject, agentValues);
        }
    }
}
