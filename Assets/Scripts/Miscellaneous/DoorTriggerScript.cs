using UnityEngine;
using Photon.Pun;

public class DoorTriggerScript : MonoBehaviourPunCallbacks
{
    private Animator anim;
    private bool isDoorOpen = false;
    private float timeToOpen = 5f;
    private float currentTime = 0;

    public bool GetDoorOpen() { return isDoorOpen; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (isDoorOpen) { return; } // if the door is already open,
                                    // we don't need to open it again.

        currentTime = 0f; // Starts the countdown timer until the door opens.
    }

    private void OnTriggerStay(Collider coll)
    {
        if (Input.GetKey(KeyCode.E) && !isDoorOpen && coll.gameObject.tag == "Player")
        {
            currentTime += Time.deltaTime;
            Debug.Log("Door progress: " + (currentTime / timeToOpen) * 100 + "%");

            if (currentTime >= timeToOpen)
            {
                ChangeDoorState();
            }
        }
    }

    private void OnTriggerExit(Collider coll)
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
