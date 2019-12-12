using UnityEditor;

using UnityEngine;

using System.Collections.Generic;

using Photon.Pun;

public class UIEnvironment : EditorWindow
{
    bool showHealthBars = true;
    [MenuItem("Window/UI and Environment")]
    public static void ShowWindow()
    {
        GetWindow<UIEnvironment>("UI and Environment");
    }

    void OnGUI()
    {
        showHealthBars = EditorGUILayout.Toggle("Enable Health Bars", showHealthBars);
        ToggleHealthBars(showHealthBars);

        if(GUILayout.Button("Switch Character"))
        {
            SwitchModel();
        }
    }

    void ToggleHealthBars(bool enable)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject element in playerObjects)
        {
            element.transform.GetChild(1).gameObject.SetActive(enable);
        }
    }

    void SwitchModel()
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
            }
        }
    }
}
