using UnityEngine;

public class GeneratorPowerOn : TriggerInteractionScript
{
    protected override void InteractionComplete(GameObject player)
    {
        if (debug) Debug.Log("All doors are now opened");
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            DoorTriggerScript doorTrigger = door.GetComponent<DoorTriggerScript>();
                
            if (!doorTrigger.GetDoorOpen())
            {
                doorTrigger.ChangeDoorState();
            }
                
            doorTrigger.LockDoorOpen();
        }

        Objectives.ObjectiveComplete("GENERATOR", "START");
        gameObject.SetActive(false);
    }
}
