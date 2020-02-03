using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSwitchManager : MonoBehaviour
{
    private int numberOfSwitches = 2;
    private int currentSwitchesActivated = 0;

    public void SwitchActivated()
    {
        currentSwitchesActivated++;
        if (currentSwitchesActivated == numberOfSwitches)
        {
            OpenArmouryDoor();
        }
    }

    public void SwitchDeactivated()
    {
        currentSwitchesActivated--;
    }

    public void OpenArmouryDoor() // needs to be public for devtools to access it.
    {
        GetComponentInParent<DoorTriggerScript>().ChangeDoorState();
        GetComponentInParent<BoxCollider>().enabled = false;
    }
}
