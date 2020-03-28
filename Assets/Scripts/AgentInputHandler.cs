using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class AgentInputHandler : MonoBehaviourPunCallbacks
{
    public GameObject pauseMenu;
    public Behaviour behaviourToToggle;
    public AgentValues agentValues;
    public ActiveCommandObject[] activeCommands;
    public PassiveCommandObject[] passiveCommands;
    private GameObject agent;

    // Delegates used by commands.
    public delegate void RunCommandOnUpdate(GameObject agent, AgentValues agentValues);
    public RunCommandOnUpdate runCommandOnUpdate;
    public delegate void RunCommandOnFixedUpdate(GameObject agent, AgentValues agentValues);
    public RunCommandOnFixedUpdate runCommandOnFixedUpdate;
    public delegate void RunCommandOnCollisionEnter(GameObject agent, AgentValues agentValues, Collision other);
    public RunCommandOnCollisionStay runCommandOnCollisionEnter;
    public delegate void RunCommandOnCollisionStay(GameObject agent, AgentValues agentValues, Collision other);
    public RunCommandOnCollisionStay runCommandOnCollisionStay;
    public delegate void RunCommandOnCollisionExit(GameObject agent, AgentValues agentValues, Collision other);
    public RunCommandOnCollisionExit runCommandOnCollisionExit;


    private void Start()
    {
        agent = this.gameObject;
        agentValues.menu = pauseMenu;
        agentValues.behaviourToToggle = behaviourToToggle;

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

    private void OnCollisionEnter(Collision other)
    {
        runCommandOnCollisionEnter(agent, agentValues, other);
    }

    private void OnCollisionStay(Collision other)
    {
        runCommandOnCollisionStay(agent, agentValues, other);
    }

    private void OnCollisionExit(Collision other)
    {
        runCommandOnCollisionExit(agent, agentValues, other);
    }
}
