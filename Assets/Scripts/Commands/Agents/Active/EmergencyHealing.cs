using UnityEngine;

[CreateAssetMenu(fileName = "DefaultEmergencyHealing", menuName = "Commands/Active/Emergency Healing", order = 0)]
public class EmergencyHealing : ActiveCommandObject
{
    public KeyCode emergencyRegenKeyCode = KeyCode.E;

    protected override void OnEnable()
    {
        keyTable.Add("Activate Regeneration", emergencyRegenKeyCode);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        AgentController agentController = (AgentController)agentInputHandler;

        if (agentController.emergencyRegenUsesRemaining > 0 && Input.GetKeyDown(emergencyRegenKeyCode))
        {
            ActivateEmergencyHealing(agentController);
        }
    } 

    void ActivateEmergencyHealing(AgentController agentController)
    {
        agentController.ChangeStat(ResourceType.EmergencyRegen, true);
        agentController.ChangeMovementSpeedModifier(agentController.agentValues.emergencyRegenSpeedMultiplier, true);
        agentController.emergencyRegenUsesRemaining--;

        // Set agent health.
        agentController.currentHealth = agentController.agentValues.maxHealth * agentController.agentValues.emergencyRegenMaxHealthModifier;

        agentController.runCommandOnUpdate += RunCommandOnUpdate;
    }
}