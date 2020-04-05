using UnityEngine;
using Photon.Pun;

public class DoorTriggerScript : TriggerInteractionScript
{
    [Header("Door Interaction")]
    // Plays the animation of the door opening.
    private Animator anim;

    // Whether the door is open or not.
    private bool isDoorOpen = false;

    /// <summary>
    /// Gets the doors current state.
    /// </summary>
    /// <returns>Whether the door is open or not.</returns>
    public bool GetDoorOpen() { return isDoorOpen; }

    /// <summary>
    /// Assigns the animator component on startup.
    /// </summary>
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Overrides the TriggerInteraction method, adding the functionality
    /// of changing the doors current state.
    /// </summary>
    /// <param name="coll"></param>
    protected override void LeftTriggerArea()
    {
        base.LeftTriggerArea();
        if (isDoorOpen) ChangeDoorState();
    }

    /// <summary>
    /// Changes the door's state once the interaction has been completed.
    /// </summary>
    /// <param name="player"></param>
    [PunRPC]
    protected override void InteractionComplete()
    {
        ChangeDoorState();
    }

    /// <summary>
    /// Sets the interaction as completed and toggles the door's state.null
    /// Also plays the animation for opening/closing the door.
    /// </summary>
    public void ChangeDoorState()
    {
        interactionComplete = true;
        isDoorOpen = !isDoorOpen;
        Debug.Log(isDoorOpen ? "Door opening" : "Door closing");
        anim.SetBool("doorOpen", isDoorOpen);
    }

    /// <summary>
    /// If the door is to be locked, then we disabled the collider, so that
    /// the player can no longer interact with the door.
    /// </summary>
    public void LockDoorOpen()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

}
