using UnityEngine;

public class GeneratorPowerOn : TriggerInteractionScript
{
    protected override void InteractionComplete(GameObject player)
    {
        // Unlocks everything requiring power.
        GameObject[] powered = GameObject.FindGameObjectsWithTag("NeedsPower");
        foreach (GameObject element in powered)
        {
            if (element.GetComponent<Teleporter>() != null)
            {
                element.GetComponent<Teleporter>().powered = true;
            }
        }

        // Open all doors.
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

        // Trigger next objective.
        Objectives.ObjectiveComplete("GENERATOR", "START");
        gameObject.SetActive(false);
    }
}
