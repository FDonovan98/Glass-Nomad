using UnityEngine;

using Photon.Pun;

public class InteractableObject : MonoBehaviourPunCallbacks
{
    [SerializeField] 
    private float interactTime; // The time needed to interact with the object to activate/open it.
    [SerializeField] 
    protected bool debug = false; // Should the debug messages be displayed.
    [SerializeField]
    private bool displayProgressToReticule = false;
    [SerializeField]
    private string interactionPrompt;
    private float currentInteractionTime = 0f; // How long the player has been pressing the interact key.
    private bool interactionComplete = false; // Is the interaction complete?

    public void ChangeCurrentInteractionTime(float value)
    {
        currentInteractionTime += value;

        if (!interactionComplete && currentInteractionTime >= interactTime)
        {
            interactionComplete = true;
            InteractionComplete();
        }
    }

    public void LeftArea()
    {
        currentInteractionTime = 0.0f;
    }

    void InteractionComplete()
    {

    }
}