using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class AgentInputHandler : MonoBehaviourPunCallbacks
{
    public AgentValues agentValues;
    public ActiveCommandObject[] activeCommands;
    public PassiveCommandObject[] passiveCommands;

    // Delegates used by commands.
    public delegate void RunCommandOnFixedUpdate(GameObject agent, AgentValues agentValues);
    public RunCommandOnFixedUpdate runCommandOnFixedUpdate;
    public delegate void RunCommandOnUpdate(GameObject agent, AgentValues agentValues);
    public RunCommandOnUpdate runCommandOnUpdate;

    private void Start()
    {
        agentValues.Initialise();

        foreach (ActiveCommandObject element in activeCommands)
        {
            element.RunCommandOnStart(this);
        }
        foreach (PassiveCommandObject element in passiveCommands)
        {
            element.RunCommandOnStart(this);
        }
    }

    private void Update()
    {
        runCommandOnUpdate(this.gameObject, agentValues);
    }

    private void FixedUpdate()
    {
        runCommandOnFixedUpdate(this.gameObject, agentValues);
    }

    private void OnCollisionStay(Collision other)
    {
        agentValues.isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        agentValues.isGrounded = false;
    }
}
