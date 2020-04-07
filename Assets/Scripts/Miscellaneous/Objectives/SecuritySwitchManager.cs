using Photon.Pun;
using UnityEngine;

public class SecuritySwitchManager : ObjectiveInteraction
{
    // The number of switches that need to be activated to activate the doors.
    private int numberOfSwitches = 2;

    // The number of switches that are currently activated.
    private int currentSwitchesActivated = 0;

    // The particle effects to start playing once the power has turned on.
    [SerializeField] private ParticleSystem[] generatorParticleSystems = null;

    // The objects that should be emissive once power is turned on.
    [SerializeField] private GameObject[] emissiveObjects = null;

    // The source to play audio from once power is turned on.
    [SerializeField] private AudioSource generatorAudioSource = null;

    // The audio clip to play once power is turned on.
    [SerializeField] private AudioClip generatorSound = null;

    /// <summary>
    /// Called by the SecuritySwitchTrigger.cs to increase the number of switches
    /// currently activated. If all switches are activated then we call open
    /// the armoury doors.
    /// </summary>
    public void SwitchActivated()
    {
        currentSwitchesActivated++;
        if (currentSwitchesActivated == numberOfSwitches)
        {
            photonView.RPC("InteractionComplete", RpcTarget.All);
        }
    }

    public void SwitchDeactivated()
    {
        currentSwitchesActivated--;
    }

    /// <summary>
    /// Changes the door state of the armoury door, and locks it open.
    /// </summary>
    [PunRPC]
    protected override void InteractionComplete()
    {
        PowerOn();
    }

    /// <summary>
    /// Called once the switches have been completed.
    /// Calls all the necessary methods for the power on sequence.
    /// </summary>
    private void PowerOn()
    {
        OpenAllDoors();
        TurnOnGenerators();
        StartSoundEffect();
        EnableEmissives();
    }

    /// <summary>
    /// Finds all gameobjects with the tag 'Door' and sets it state to open, 
    /// if it's not already.
    /// </summary>
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

    /// <summary>
    /// Loops through all the particle systems assigned in the inspector and
    /// plays them.
    /// </summary>
    private void TurnOnGenerators()
    {
        foreach (ParticleSystem ps in generatorParticleSystems)
        {
            if (ps != null) ps.Play();
        }
    }
    
    /// <summary>
    /// Plays the generator sound effect.
    /// </summary>
    private void StartSoundEffect()
    {
        generatorAudioSource.PlayOneShot(generatorSound);
    }

    /// <summary>
    /// Enables the emissive materials of the emissive objects set in inspector.
    /// </summary>
    private void EnableEmissives()
    {
        // Enable generator room emission material.
        foreach (GameObject mat in emissiveObjects)
        {
            Debug.Log("Enabling the emission on: " + mat.name + " on " + mat.GetComponent<Renderer>().materials[1].name);
            mat.GetComponent<Renderer>().materials[1].EnableKeyword("_EMISSION");
        }
    }
}
