using UnityEngine;

using System;

[CreateAssetMenu(fileName = "ChangeResourceOverTime", menuName = "Commands/Passive/Change Resource Over Time", order = 0)]
public class ChangeResourceOverTime : PassiveCommandObject
{
    public TypeAndConstraints[] resourcesToChange;
    
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        agentInputHandler.runCommandOnTriggerStay += RunCommandOnTriggerStay;
        agentInputHandler.runCommandOnTriggerExit += RunCommandOnTriggerExit;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        AgentController agentController = null;

        foreach (TypeAndConstraints element in resourcesToChange)
        {
            
            if (String.IsNullOrEmpty(element.areaTag))
            {
                if (agentController == null)
                {
                    agentController = (AgentController)agentInputHandler;
                }

                if (element.resourceType == ResourceType.Health)
                {
                    agentController.ChangeStat(element.resourceType, Time.deltaTime * element.changeValue * agentValues.maxHealth / 100);
                }
                else
                {
                    agentController.ChangeStat(element.resourceType, Time.deltaTime * element.changeValue);
                }
                
            }          
        }
    }

    void RunCommandOnTriggerStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        AgentController agentController = null;

        foreach (TypeAndConstraints element in resourcesToChange)
        {
            if (other.gameObject.tag == element.areaTag)
            {
                if (agentController == null)
                {
                    agentController = (AgentController)agentInputHandler;
                } 

                // Oxygen decrease rate scales with movement speed.
                if (element.resourceType == ResourceType.Oxygen)
                {
                    agentController.ChangeStat(element.resourceType, Time.deltaTime * element.changeValue * agentInputHandler.moveSpeedMultiplier);
                    
                    // Updates UI to flag oxy is regenerating.
                    if (element.changeValue > 0)
                    {
                        agentController.oxygenIsRegening = true;
                        agentController.updateUI(ResourceType.OxygenRegen);
                    }
                }
                else
                {
                    agentController.ChangeStat(element.resourceType, Time.deltaTime * element.changeValue);
                }

            }
        }
    }

    void RunCommandOnTriggerExit(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        AgentController agentController = null;

        foreach (TypeAndConstraints element in resourcesToChange)
        {
            if (other.gameObject.tag == element.areaTag)
            {
                if (agentController == null)
                {
                    agentController = (AgentController)agentInputHandler;
                } 

                // Updates UI to flag oxy is regenerating.
                if (element.resourceType == ResourceType.Oxygen && element.changeValue > 0)
                {
                    agentController.oxygenIsRegening = false;
                    agentController.updateUI(ResourceType.OxygenRegen);
                }
            }
        }
    }
}

[Serializable]
public class TypeAndConstraints
{
    public ResourceType resourceType;
    public float changeValue;
    public string areaTag;
}