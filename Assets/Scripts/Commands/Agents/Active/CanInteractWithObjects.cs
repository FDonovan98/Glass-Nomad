using UnityEngine;

using Photon.Pun;

[CreateAssetMenu(fileName = "DefaultCanInteractWithObjects", menuName = "Commands/Active/CanInteractWithObjects", order = 0)]
public class CanInteractWithObjects : ActiveCommandObject
{
    [SerializeField]
    private KeyCode interact = KeyCode.E;

    protected override void OnEnable()
    {
        keyTable.Add("Interact", interact);
    }
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnTriggerEnter += RunCommandOnTriggerEnter;
        agentInputHandler.runCommandOnTriggerExit += RunCommandOnTriggerExit;
    }

    void RunCommandOnTriggerEnter(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();

        if (interactableObject != null)
        {
            interactableObject.ChangeCurrentInteractionTime(Time.fixedDeltaTime);
        }
    }

    void RunCommandOnTriggerExit(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();

        if (interactableObject != null)
        {
            interactableObject.LeftArea();
        }
    }
}