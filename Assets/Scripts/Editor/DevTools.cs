using UnityEditor;
using UnityEngine;
using Photon.Pun;
using System;

public class DevTools : EditorWindow
{
<<<<<<< HEAD
    bool showHealthBars = true;
=======
    // Used to toggle the visibility of health bars.
    private bool showHealthBars = true;

>>>>>>> master
    [MenuItem("Window/Developer Tools/General")]
    public static void ShowWindow()
    {
        GetWindow<DevTools>("General Tools");
    }

    /// <summary>
    /// Sets up the GUI of the developer tools window.
    /// When a button is pressed, it calls the appropriate function.
    /// </summary>
    void OnGUI()
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

<<<<<<< HEAD
=======
        if (GUILayout.Button("Open All Doors"))
        {
            OpenAllDoors();
        }

>>>>>>> master
        if (GUILayout.Button("Activate Armoury Switches"))
        {
            ActivateSwitches();
        }
    }

<<<<<<< HEAD
=======
    /// <summary>
    /// Finds all the players in the game and shows/hides their health bar.
    /// </summary>
    /// <param name="enable"></param>
>>>>>>> master
    private void ToggleHealthBars(bool enable)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject element in playerObjects)
        {
            element.transform.GetChild(1).gameObject.SetActive(enable);
        }
    }

<<<<<<< HEAD
=======
    /// <summary>
    /// Finds all the players and swaps the prefab of character that the local player
    /// is controlling.
    /// </summary>
>>>>>>> master
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

<<<<<<< HEAD
=======
    /// <summary>
    /// Spawns a marine or alien, and disables their input, camera and audio listener.
    /// </summary>
    /// <param name="prefabName"></param>
>>>>>>> master
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
                    creature.GetComponent<MarineMovement>().enabled = false;
                }
                
                creature.GetComponentInChildren<Camera>().enabled = false;

                return;
            }
        }
    }

<<<<<<< HEAD
    private void ActivateSwitches()
    {
        GameObject armoury = GameObject.FindGameObjectWithTag("ArmouryDoor");
        armoury.GetComponentInChildren<RedSwitchManager>().OpenArmouryDoor();
=======
    /// <summary>
    /// Finds all doors with the tag 'Door' and opens them, if they are not already open.
    /// </summary>
    private void OpenAllDoors()
    {
        GameObject.Find("ObjectivesManager").GetComponent<SecuritySwitchManager>().OpenAllDoors();
    }

    /// <summary>
    /// Opens the armoury door.
    /// </summary>
    private void ActivateSwitches()
    {
        GameObject.Find("ObjectivesManager").GetComponent<SecuritySwitchManager>().RedSwitchesCompleted();
>>>>>>> master
    }
}
