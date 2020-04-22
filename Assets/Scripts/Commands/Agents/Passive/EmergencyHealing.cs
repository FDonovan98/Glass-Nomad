using UnityEngine;

[CreateAssetMenu(fileName = "DefaultEmergencyHealing", menuName = "Commands/Passive/Emergency Healing", order = 0)]
public class EmergencyHealing : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;

        agentInputHandler.runCommandOnAgentHasBeenHit += RunCommandOnAgentHasBeenHit;
    }

    void RunCommandOnAgentHasBeenHit(AgentInputHandler agentInputHandler, Vector3 position, Vector3 normal, float value)
    {
        AgentController agentController = (AgentController)agentInputHandler;
        if (!agentController.emergencyRegenActive)
        {
            float healthPercent = agentController.currentHealth - value;
            healthPercent /= agentInputHandler.agentValues.maxHealth;
            healthPercent *= 100;

            Debug.Log(healthPercent);

            if (healthPercent <= agentInputHandler.agentValues.emergencyRegenThreshold)
            {
                agentController.emergencyRegenActive = true;
                agentController.ChangeMovementSpeedModifier(agentInputHandler.agentValues.emergencyRegenSpeedMultiplier, true);
                // Set agent health.
                agentController.currentHealth = value + agentInputHandler.agentValues.maxHealth * agentInputHandler.agentValues.emergencyRegenMaxHealthModifier;
            }
        }
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        AgentController agentController = (AgentController)agentInputHandler;

        if (agentController.emergencyRegenActive)
        {
            agentController.ChangeResourceCount(AgentController.ResourceType.Health, -agentInputHandler.agentValues.emergencyRegenDownTickValue * Time.deltaTime);

            if (agentController.currentHealth < agentController.agentValues.maxHealth)
            {
                agentController.emergencyRegenActive = false;
                agentController.ChangeMovementSpeedModifier(agentInputHandler.agentValues.emergencyRegenSpeedMultiplier, false);
            }
        }
    } 
}