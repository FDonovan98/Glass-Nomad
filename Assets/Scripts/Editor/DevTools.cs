using UnityEditor;

using UnityEngine;

using Photon.Pun;
using System;

public class DevTools : EditorWindow
{
    bool showHealthBars = true;
    [MenuItem("Window/Developer Tools/General")]
    public static void ShowWindow()
    {
        GetWindow<DevTools>("General Tools");
    }

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

        if (GUILayout.Button("Activate Armoury Switches"))
        {
            ActivateSwitches();
        }
    }

    private void ToggleHealthBars(bool enable)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject element in playerObjects)
        {
            element.transform.GetChild(1).gameObject.SetActive(enable);
        }
    }

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

    private void ActivateSwitches()
    {
        GameObject armoury = GameObject.FindGameObjectWithTag("ArmouryDoor");
        armoury.GetComponentInChildren<RedSwitchManager>().OpenArmouryDoor();
    }
}
