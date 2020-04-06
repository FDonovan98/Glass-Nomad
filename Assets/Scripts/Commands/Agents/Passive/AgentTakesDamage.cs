using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAgentTakesDamage", menuName = "Commands/Passive/AgentTakesDamage", order = 0)]
public class AgentTakesDamage : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnAgentHasBeenHit += RunCommandOnAgentHasBeenHit;
    }

    private void RunCommandOnAgentHasBeenHit(AgentInputHandler agentInputHandler, Vector3 position, Vector3 normal, float value)
    {
        AgentController agentController = agentInputHandler.gameObject.GetComponent<AgentController>();

        agentController.ChangeResourceCount(AgentController.ResourceType.Health, -value);
    }
}