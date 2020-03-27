using UnityEngine;

[CreateAssetMenu(fileName = "DefualtCheckIfGrounded", menuName = "Commands/Passive/CheckIfGrounded")]
public class CheckIfGrounded : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnCollisionStay += RunCommandOnCollisionStay;
        agentInputHandler.runCommandOnCollisionExit += RunCommandOnCollisionExit;
    }

    void RunCommandOnCollisionStay(GameObject agent, AgentValues agentValues)
    {
        agentValues.isGrounded = true;
    }
    void RunCommandOnCollisionExit(GameObject agent, AgentValues agentValues)
    {
        agentValues.isGrounded = false;
    }
}