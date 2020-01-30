using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorPowerOn : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("All doors are now opened");
            GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
            foreach (GameObject door in doors)
            {
                DoorTriggerScript doorTrigger = door.GetComponent<DoorTriggerScript>();
                
                if (!doorTrigger.GetDoorOpen())
                {
                    doorTrigger.ChangeDoorState();
                }
                
                // Keeps the door open, forever.
                doorTrigger.LockDoor();
            }
            this.gameObject.SetActive(false);
        }
    }
}
