using UnityEngine;
using Photon.Pun;

public class DoorTriggerScript : MonoBehaviourPunCallbacks
{
    private Animator anim;
    private bool isDoorOpen = false;

    public bool GetDoorOpen() { return isDoorOpen; }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (isDoorOpen) { return; } // if the door is already open,
                                    // we don't need to open it again.
        ChangeDoorState(coll.gameObject);
    }

    private void OnTriggerExit(Collider coll)
    {
        ChangeDoorState(coll.gameObject);
    }

    public void ChangeDoorState(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            isDoorOpen = !isDoorOpen;
            Debug.Log(isDoorOpen ? "Door opening" : "Door closing");
            anim.SetBool("doorOpen", isDoorOpen);
        }
    }
}
