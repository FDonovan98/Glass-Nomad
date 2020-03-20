using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentInputHandler : MonoBehaviour
{
    public AgentValues agentValues;
    public ActiveCommandObject[] activeCommands;
    public PassiveCommandObject[] passiveCommands;

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

        foreach (PassiveCommandObject element in passiveCommands)
        {
            element.Execute(this.gameObject, agentValues);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        agentValues.isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        agentValues.isGrounded = false;
    }
}
