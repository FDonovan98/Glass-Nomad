using UnityEditor;

using UnityEngine;

using System.Collections.Generic;

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
    }

    void ToggleHealthBars(bool enable)
    {
        List<GameObject> playerObjects = new List<GameObject>();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject element in allObjects)
        {
            if (element.tag == "Player")
            {
                element.transform.GetChild(1).gameObject.SetActive(enable);
            }
        }
    }
}
