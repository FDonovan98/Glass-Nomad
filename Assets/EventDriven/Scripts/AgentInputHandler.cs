using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class AgentInputHandler : MonoBehaviourPunCallbacks
{
    public AgentValues agentValues;
    private GameObject agent;
    public ActiveCommandObject[] activeCommands;
    public PassiveCommandObject[] passiveCommands;

    // Delegates used by commands.
    public delegate void RunCommandOnUpdate(GameObject agent, AgentValues agentValues);
    public RunCommandOnUpdate runCommandOnUpdate;
    public delegate void RunCommandOnFixedUpdate(GameObject agent, AgentValues agentValues);
    public RunCommandOnFixedUpdate runCommandOnFixedUpdate;
    public delegate void RunCommandOnCollisionStay(GameObject agent, AgentValues agentValues);
    public RunCommandOnCollisionStay runCommandOnCollisionStay;
    public delegate void RunCommandOnCollisionExit(GameObject agent, AgentValues agentValues);
    public RunCommandOnCollisionExit runCommandOnCollisionExit;


    private void Start()
    {
        agent = this.gameObject;
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
        runCommandOnUpdate(agent, agentValues);
    }

    private void FixedUpdate()
    {
        runCommandOnFixedUpdate(agent, agentValues);
    }

    private void OnCollisionStay(Collision other)
    {
        runCommandOnCollisionStay(agent, agentValues);
    }

    private void OnCollisionExit(Collision other)
    {
        runCommandOnCollisionExit(agent, agentValues);
    }
}
