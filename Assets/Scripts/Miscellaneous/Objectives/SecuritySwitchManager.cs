using UnityEngine;
using Photon.Pun;

public class SecuritySwitchManager : MonoBehaviour
{
    // The number of switches that need to be activated to activate the doors.
    private int numberOfSwitches = 2;

    // The number of switches that are currently activated.
    private int currentSwitchesActivated = 0;
    [SerializeField] private ParticleSystem[] generatorParticleSystems = null;

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
            RedSwitchesCompleted();
        }
    }

    public void SwitchDeactivated()
    {
        currentSwitchesActivated--;
    }

    /// <summary>
    /// Changes the door state of the armoury door, and locks it open.
    /// </summary>
    public void RedSwitchesCompleted()
    {
        Objectives.ObjectiveComplete("RED SWITCH", "GENERATOR");
        if (Objectives.IsObjectiveComplete("RED SWITCH"))
        {
            PowerOn();
        }
    }

    private void PowerOn()
    {
        OpenAllDoors();
        TurnOnGenerators();
        // Start generator sound effect.
        // Enable generator room emission material.
    }

    public static void OpenAllDoors()
    {
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
    }

    private void TurnOnGenerators()
    {
        foreach (ParticleSystem ps in generatorParticleSystems)
        {
            ps.Play();
        }
    }
}
