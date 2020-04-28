using UnityEngine;

[CreateAssetMenu(fileName = "DefaultEmergencyHealing", menuName = "Commands/Passive/Emergency Healing", order = 0)]
public class EmergencyHealing : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnAgentHasBeenHit += RunCommandOnAgentHasBeenHit;
    }

    void RunCommandOnAgentHasBeenHit(AgentInputHandler agentInputHandler, Vector3 position, Vector3 normal, float value)
    {
        AgentController agentController = (AgentController)agentInputHandler;
        if (!agentController.emergencyRegenActive && agentController.emergencyRegenUsesRemaining > 0)
        {
            float healthPercent = agentController.currentHealth - value;
            healthPercent /= agentInputHandler.agentValues.maxHealth;
            healthPercent *= 100;

            Debug.Log(healthPercent);

            if (healthPercent <= agentInputHandler.agentValues.emergencyRegenThreshold)
            {
                agentController.ChangeStat(ResourceType.EmergencyRegen, true);
                agentController.ChangeMovementSpeedModifier(agentInputHandler.agentValues.emergencyRegenSpeedMultiplier, true);
                agentController.emergencyRegenUsesRemaining--;

                // Set agent health.
                agentController.currentHealth = value + agentInputHandler.agentValues.maxHealth * agentInputHandler.agentValues.emergencyRegenMaxHealthModifier;

                agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
            }
        }
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        AgentController agentController = (AgentController)agentInputHandler;

        agentController.ChangeStat(ResourceType.Health, -agentInputHandler.agentValues.emergencyRegenDownTickValue * Time.deltaTime);

        if (agentController.currentHealth < agentController.agentValues.maxHealth)
        {
            agentController.ChangeStat(ResourceType.EmergencyRegen, false);
            agentController.ChangeMovementSpeedModifier(agentInputHandler.agentValues.emergencyRegenSpeedMultiplier, false);

            agentInputHandler.runCommandOnUpdate -= RunCommandOnUpdate;
        }
    } 
}