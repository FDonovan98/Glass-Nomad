using UnityEngine;

using UnityEngine.UI;

using TMPro;

[CreateAssetMenu(fileName = "DefaultManageOxygen", menuName = "Commands/Passive/ManageOxygen", order = 0)]
public class ManageOxygen : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        agentInputHandler.runCommandOnTriggerStay += RunCommandOnTriggerStay;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        agentInputHandler.currentOxygen -= Time.deltaTime;
        agentInputHandler.currentOxygen = Mathf.Clamp(agentInputHandler.currentOxygen, 0.0f, agentValues.maxOxygen);

        if (agentInputHandler.currentOxygen == 0.0f)
        {
            OxygenHasRunOut(agentInputHandler, agentValues);
        }

        if (agentInputHandler.oxygenDisplay != null)
        {
            Slider oxygenSlider = agentInputHandler.oxygenDisplay.GetComponentInChildren<Slider>();
            TextMeshProUGUI oxygenText = agentInputHandler.oxygenDisplay.GetComponentInChildren<TextMeshProUGUI>();

            oxygenSlider.value = agentInputHandler.currentOxygen / agentValues.maxOxygen * 100;
            oxygenText.text = (Mathf.Round(agentInputHandler.currentOxygen / agentValues.maxOxygen * 100)).ToString();
        }
    }

    void RunCommandOnTriggerStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        if (other.gameObject.tag == "OxygenRegenZone")
        {
            agentInputHandler.currentOxygen += Time.fixedDeltaTime * agentValues.oxygenRegenModifier;
            agentInputHandler.currentOxygen = Mathf.Clamp(agentInputHandler.currentOxygen, 0.0f, agentValues.maxOxygen);
        }
    }

    void OxygenHasRunOut(AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        AgentController agentController = agentInputHandler.gameObject.GetComponent<AgentController>();

        agentController.ChangeResourceCount(AgentController.ResourceType.Health, -(agentValues.suffocationDamage * Time.deltaTime));
    }
}