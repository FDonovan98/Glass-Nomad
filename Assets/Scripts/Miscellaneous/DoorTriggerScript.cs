using UnityEngine;

public class DoorTriggerScript : TriggerInteractionScript
{
    // Plays the animation of the door opening.
    private Animator anim;

    // Keeps track of whether the door is open or not.
    private bool isDoorOpen = false;

    public bool GetDoorOpen() { return isDoorOpen; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    protected override void LeftTriggerArea()
    {
        if (isDoorOpen) ChangeDoorState();
    }

    protected override void InteractionComplete(GameObject player)
    {
        ChangeDoorState();
    }

    public void ChangeDoorState()
    {
        interactionComplete = true;
        isDoorOpen = !isDoorOpen;
        Debug.Log(isDoorOpen ? "Door opening" : "Door closing");
        anim.SetBool("doorOpen", isDoorOpen);
    }

    public void LockDoorOpen()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

}
