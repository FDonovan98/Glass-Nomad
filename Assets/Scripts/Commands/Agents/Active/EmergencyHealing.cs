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
            ActivateEmergencyHealing(agentController, agentValues);
        }

        if (agentController.emergencyRegenActive)
        {
            agentController.ChangeStat(ResourceType.Health, -agentValues.emergencyRegenDownTickValue * Time.deltaTime);

            if (agentController.currentHealth < agentValues.maxHealth)
            {
                agentController.ChangeStat(ResourceType.EmergencyRegen, false);
            }
        }
    } 

    void ActivateEmergencyHealing(AgentController agentController, AgentValues agentValues)
    {
        agentController.ChangeStat(ResourceType.EmergencyRegen, true);
        agentController.ChangeMovementSpeedModifier(agentController.agentValues.emergencyRegenSpeedMultiplier, true);
        agentController.emergencyRegenUsesRemaining--;

        //Play audio.
        agentController.mainAudioSource.PlayOneShot(agentController.emergencyRegenAudio);

        // Set agent health.
        float healthToAdd = agentValues.maxHealth - agentController.currentHealth;
        healthToAdd += agentValues.maxHealth * agentValues.emergencyRegenMaxHealthModifier - agentValues.maxHealth;
        agentController.ChangeStat(ResourceType.Health, healthToAdd);

        agentController.runCommandOnUpdate += RunCommandOnUpdate;
    }
}