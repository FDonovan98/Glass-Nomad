using UnityEditor;
using UnityEngine;
using Photon.Pun;

public class DevTools : EditorWindow
{
    // Used to toggle the visibility of health bars.
    private bool showHealthBars = true;

    [MenuItem("Window/Developer Tools/General")]
    public static void ShowWindow()
    {
        GetWindow<DevTools>("General Tools");
    }

    /// <summary>
    /// Sets up the GUI of the developer tools window.
    /// When a button is pressed, it calls the appropriate function.
    /// </summary>
    private void OnGUI()
    {
        showHealthBars = EditorGUILayout.Toggle("Enable Health Bars", showHealthBars);
        ToggleHealthBars(showHealthBars);

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Spawn Alien"))
        {
            SpawnCreature("Alien (Cylinder)");
        }

        if(GUILayout.Button("Spawn Marine"))
        {
            SpawnCreature("Marine (Cylinder)");
        }

        GUILayout.EndHorizontal();

        if(GUILayout.Button("Switch Character"))
        {
            SwitchModel();
        }

        if (GUILayout.Button("Open All Doors"))
        {
            OpenAllDoors();
        }

        if (GUILayout.Button("Activate Armoury Switches"))
        {
            ActivateSwitches();
        }
    }

    /// <summary>
    /// Finds all the players in the game and shows/hides their health bar.
    /// </summary>
    /// <param name="enable"></param>
    private void ToggleHealthBars(bool enable)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject element in playerObjects)
        {
            element.transform.GetChild(1).gameObject.SetActive(enable);
        }
    }

    /// <summary>
    /// Finds all the players and swaps the prefab of character that the local player
    /// is controlling.
    /// </summary>
    private void SwitchModel()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject element in playerObjects)
        {
            if (element.GetComponent<PhotonView>().IsMine)
            {
                Vector3 playerPos = element.transform.position;
                Quaternion playerRot = element.transform.rotation;
                string prefabName;

                if (element.GetComponent<AlienController>() != null)
                {
                    prefabName = "Marine (Cylinder)";
                }
                else
                {
                    prefabName ="Alien (Cylinder)";
                }

                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                PhotonNetwork.Instantiate(prefabName, playerPos, playerRot);

                return;
            }
        }
    }

    /// <summary>
    /// Spawns a marine or alien, and disables their input, camera and audio listener.
    /// </summary>
    /// <param name="prefabName"></param>
    private void SpawnCreature(string prefabName)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject element in playerObjects)
        {
            if (element.GetComponent<PhotonView>().IsMine)
            {
                Transform playerCam = element.GetComponentInChildren<Camera>().transform;
                Ray ray = new Ray(element.transform.position, playerCam.forward);

                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                GameObject creature = PhotonNetwork.Instantiate(prefabName, hit.point, new Quaternion());

                if (creature.GetComponent<AlienController>() != null)
                {
                    creature.GetComponent<AlienController>().enabled = false;
                }
                else 
                {
                    creature.GetComponent<MarineController>().enabled = false;
                }
                
                creature.GetComponentInChildren<AudioListener>().enabled = false;
                creature.GetComponentInChildren<Camera>().enabled = false;

                return;
            }
        }
    }


    /// <summary>
    /// Finds all doors with the tag 'Door' and opens them, if they are not already open.
    /// </summary>
    private void OpenAllDoors()
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

            doorTrigger.LockDoorOpen();
        }
    }

    /// <summary>
    /// Opens the armoury door.
    /// </summary>
    private void ActivateSwitches()
    {
        GameObject armoury = GameObject.FindGameObjectWithTag("ArmouryDoor");
        armoury.GetComponentInChildren<RedSwitchManager>().OpenArmouryDoor();
    }
}
