using UnityEngine;

[CreateAssetMenu(fileName = "DefualtCheckIfGrounded", menuName = "Commands/Passive/CheckIfGrounded")]
public class CheckIfGrounded : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnCollisionExit += RunCommandOnCollisionExit;
        agentInputHandler.runCommandOnCollisionStay += RunCommandOnCollisionStay;
    }

    void RunCommandOnCollisionStay(GameObject agent, AgentValues agentValues, Collision other)
    {
        agentValues.isGrounded = true;
    }
    
    void RunCommandOnCollisionExit(GameObject agent, AgentValues agentValues, Collision other)
    {
        agentValues.isGrounded = false;
    }
}