using UnityEngine;
using Photon.Pun;

public class DoorTriggerScript : MonoBehaviourPunCallbacks
{
    private Animator anim;
    private bool isDoorOpen = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            // ---- might need to check that this is a local player ----
            // ---- might want a button press to trigger the below? ----
            ChangeDoorState();
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            // ---- might need to check that this is a local player ----
            ChangeDoorState();
        }
    }

    private void ChangeDoorState()
    {
        isDoorOpen = !isDoorOpen;
        Debug.Log(isDoorOpen ? "Door opening" : "Door closing");
        anim.SetBool("doorOpen", isDoorOpen);
    }
}
