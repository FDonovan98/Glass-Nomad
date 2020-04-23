using UnityEngine;

using Photon.Pun;

using UnityEngine.UI;

public abstract class InteractableObject : MonoBehaviourPunCallbacks
{
    public string interactionPrompt;
    [SerializeField] 
    private float interactTime; // The time needed to interact with the object to activate/open it.
    [SerializeField] 
    protected bool debug = false; // Should the debug messages be displayed.
    [SerializeField]
    private bool displayProgressToReticule = false;
    private float currentInteractionTime = 0f; // How long the player has been pressing the interact key.
    public bool interactionComplete = false; // Is the interaction complete?

    public void ChangeCurrentInteractionTime(AgentInputHandler agentInputHandler, float value)
    {
        currentInteractionTime += value;

        if (agentInputHandler.progressBar != null)
        {
            UpdateProgressBar(agentInputHandler.progressBar, currentInteractionTime);
        }

        if (!interactionComplete && currentInteractionTime >= interactTime)
        {
            interactionComplete = true;
            photonView.RPC("InteractionComplete", RpcTarget.All);
        }
    }

    public static void UpdateProgressBar(Image progressBar, float value)
    {
        progressBar.fillAmount = (value / 100);
        if (value >= 100)
        {
            progressBar.fillAmount = 0;
        }
    }

    public virtual void LeftArea(AgentInputHandler agentInputHandler)
    {
        currentInteractionTime = 0.0f;
        interactionComplete = false;
        UpdateProgressBar(agentInputHandler.progressBar, currentInteractionTime);
    }

    [PunRPC]
    abstract protected void InteractionComplete();
}