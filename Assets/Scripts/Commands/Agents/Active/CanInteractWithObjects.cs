using UnityEngine;

using Photon.Pun;

[CreateAssetMenu(fileName = "DefaultCanInteractWithObjects", menuName = "Commands/Active/CanInteractWithObjects", order = 0)]
public class CanInteractWithObjects : ActiveCommandObject
{
    [SerializeField]
    private string interact = "Interact";

    InteractableObject interactableObject;

    protected override void OnEnable()
    {
        keyTable.Add("Interact", interact);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        interactableObject = null;

        if (agentInputHandler.isLocalAgent)
        {
            agentInputHandler.runCommandOnTriggerEnter += RunCommandOnTriggerEnter;
            agentInputHandler.runCommandOnTriggerExit += RunCommandOnTriggerExit;
        }
    }

    void RunCommandOnTriggerEnter(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        if (interactableObject == null)
        {
            interactableObject = other.GetComponent<InteractableObject>();
        }

        if (interactableObject != null && !interactableObject.interactionComplete)
        {
            agentInputHandler.interactionPromptText.text = interactableObject.interactionPrompt;

            agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        }
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (Input.GetKey(interact))
        {
            if (agentInputHandler.allowInput)
            {
                agentInputHandler.allowInput = false;
            }

            interactableObject.ChangeCurrentInteractionTime(agentInputHandler, Time.fixedDeltaTime);
        }        
        else if (Input.GetKeyUp(interact))
        {
            agentInputHandler.allowInput = true;
            interactableObject.LeftArea(agentInputHandler);
        }
    }

    void RunCommandOnTriggerExit(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        if (interactableObject != null)
        {
            interactableObject.LeftArea(agentInputHandler);

            agentInputHandler.interactionPromptText.text = null;
            agentInputHandler.runCommandOnUpdate -= RunCommandOnUpdate;
            
            interactableObject = null;
        }
    }
}