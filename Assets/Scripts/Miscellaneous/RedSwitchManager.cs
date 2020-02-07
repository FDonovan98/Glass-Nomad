using UnityEngine;

public class RedSwitchManager : MonoBehaviour
{
    // The number of switches that need to be activated to activate the doors.
    private int numberOfSwitches = 2;

    // The number of switches that are currently activated.
    private int currentSwitchesActivated = 0;

    /// <summary>
    /// Called by the RedSwitchTrigger.cs to increase the number of switches
    /// currently activated. If all switches are activated then we call open
    /// the armoury doors.
    /// </summary>
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

    /// <summary>
    /// Changes the door state of the armoury door, and locks it open.
    /// This function needs to be public as it is accessed by the dev tools.
    /// </summary>
    public void OpenArmouryDoor() // needs to be public for devtools to access it.
    {
        GetComponentInParent<DoorTriggerScript>().ChangeDoorState();
        GetComponentInParent<BoxCollider>().enabled = false;
        Objectives.ObjectiveComplete("RED SWITCH", "GENERATOR");
    }
}
