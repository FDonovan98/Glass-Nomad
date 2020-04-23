using UnityEngine;

using Photon.Pun;

public class InteractableDoor : InteractableObject
{
    [SerializeField]
    private Animator animator;
    private bool isDoorOpen = false;

    [PunRPC]
    protected override void InteractionComplete()
    {
        ChangeDoorState();
    }

    public override void LeftArea(AgentInputHandler agentInputHandler)
    {
        base.LeftArea(agentInputHandler);

        if (isDoorOpen)
        {
            ChangeDoorState();
        }
    }

    /// <summary>
    /// Sets the interaction as completed and toggles the door's state.
    /// Also plays the animation for opening/closing the door.
    /// </summary>
    void ChangeDoorState()
    {
        isDoorOpen = !isDoorOpen;
        Debug.Log(isDoorOpen ? "Door opening" : "Door closing");
        animator.SetBool("doorOpen", isDoorOpen);

        // So that we can open the door immediately again.
        interactionComplete = false; 
    }
}