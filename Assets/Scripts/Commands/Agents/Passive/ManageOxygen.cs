using UnityEngine;

using UnityEngine.UI;

using TMPro;

[CreateAssetMenu(fileName = "DefaultManageOxygen", menuName = "Commands/Passive/ManageOxygen", order = 0)]
public class ManageOxygen : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        agentInputHandler.runCommandOnCollisionStay += RunCommandOnCollisionStay;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        agentInputHandler.currentOxygen -= Time.deltaTime;
        Slider oxygenSlider = agentInputHandler.oxygenDisplay.GetComponentInChildren<Slider>();
        TextMeshProUGUI oxygenText = agentInputHandler.oxygenDisplay.GetComponentInChildren<TextMeshProUGUI>();

        oxygenSlider.value = agentInputHandler.currentOxygen / agentValues.maxOxygen * 100;
        oxygenText.text = (Mathf.Round(agentInputHandler.currentOxygen)).ToString();
    }

    void RunCommandOnCollisionStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collision other)
    {
        if (other.gameObject.tag == "OxygenRegenZone")
        {
            agentInputHandler.currentOxygen = Mathf.Min(agentValues.maxOxygen, agentInputHandler.currentOxygen + (Time.fixedDeltaTime * agentValues.oxygenRegenModifier));
        }
    }
}