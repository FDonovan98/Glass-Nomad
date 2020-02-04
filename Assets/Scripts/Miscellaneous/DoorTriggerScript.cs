using UnityEngine;

public class DoorTriggerScript : TriggerInteractionScript
{
    private Animator anim;
    private bool isDoorOpen = false;

    public bool GetDoorOpen() { return isDoorOpen; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    protected override void LeftTriggerArea()
    {
        if (isDoorOpen)
        {
            ChangeDoorState();
        }
    }

    protected override void InteractionComplete()
    {
        ChangeDoorState();
    }

    public void ChangeDoorState()
    {
        isDoorOpen = !isDoorOpen;
        Debug.Log(isDoorOpen ? "Door opening" : "Door closing");
        anim.SetBool("doorOpen", isDoorOpen);
    }

    public void LockDoorOpen()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

}
