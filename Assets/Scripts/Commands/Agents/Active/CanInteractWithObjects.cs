using UnityEngine;

using Photon.Pun;

[CreateAssetMenu(fileName = "DefaultCanInteractWithObjects", menuName = "Commands/Active/CanInteractWithObjects", order = 0)]
public class CanInteractWithObjects : ActiveCommandObject
{
    [SerializeField]
    private KeyCode interact = KeyCode.E;

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
            agentInputHandler.runCommandOnTriggerStay += RunCommandOnTriggerStay;
            agentInputHandler.runCommandOnTriggerExit += RunCommandOnTriggerExit;
        }
    }

    void RunCommandOnTriggerEnter(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        if (interactableObject == null)
        {
            InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        }

        if (interactableObject != null)
        {
            agentInputHandler.interactionPromptText.text = interactableObject.interactionPrompt;
        }
    }

    void RunCommandOnTriggerStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        
        if (interactableObject != null && !interactableObject.interactionComplete)
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
            }
        }

    }

    void RunCommandOnTriggerExit(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        interactableObject = null;

        if (interactableObject != null)
        {
            interactableObject.LeftArea();

            agentInputHandler.interactionPromptText.text = null;
        }
    }
}