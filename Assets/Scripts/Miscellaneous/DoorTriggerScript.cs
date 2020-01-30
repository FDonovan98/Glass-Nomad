using UnityEngine;
using Photon.Pun;

public class DoorTriggerScript : MonoBehaviourPunCallbacks
{
    // Plays the animation of the door opening.
    private Animator anim;

    // Keeps track of whether the door is open or not.
    private bool isDoorOpen = false;
    
    // How long it takes to open the door.
    private float timeToOpen = 5f;

    // Keeps track of how long the player has been opening the door.
    private float currentTime = 0;

    public bool GetDoorOpen() { return isDoorOpen; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        // Resets the countdown timer until the door opens.
        currentTime = 0f;
    }

    /// <summary>
    /// If the player walks within the door's collider and holds down 'E' for the 
    /// time specified in the 'timeToOpen' variable, then the ChangeDoorState will
    /// be called.
    /// </summary>
    /// <param name="coll"></param>
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

    /// <summary>
    /// If the player exits the door's collider, then the door will close.
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerExit(Collider coll)
    {
        if (isDoorOpen)
        {
            ChangeDoorState();
        }
    }

    /// <summary>
    /// Used by multiple scripts, as well as this one, to change the door's state.
    /// So, if the door is open, this function play the closing animation.
    /// But, if the door is closed, this function will play the opening animation.
    /// </summary>
    public void ChangeDoorState()
    {
        isDoorOpen = !isDoorOpen;
        Debug.Log(isDoorOpen ? "Door opening" : "Door closing");
        anim.SetBool("doorOpen", isDoorOpen);
    }

    public void LockDoor()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

}
