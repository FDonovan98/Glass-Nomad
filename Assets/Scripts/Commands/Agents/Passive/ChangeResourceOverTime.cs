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
               
                agentController.ChangeResourceCount(element.resourceType, Time.deltaTime * element.changeValue);
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

                agentController.ChangeResourceCount(element.resourceType, Time.deltaTime * element.changeValue);
            }
        }
    }
}

[Serializable]
public class TypeAndConstraints
{
    public AgentController.ResourceType resourceType;
    public float changeValue;
    public string areaTag;
}