using UnityEditor;

using UnityEngine;

public class UIEnvironment : EditorWindow
{
    bool showHealthBars = false;
    [MenuItem("Window/UI and Environment")]
    public static void ShowWindow()
    {
        GetWindow<UIEnvironment>("UI and Environment");
    }

    void OnGUI()
    {
        showHealthBars = EditorGUILayout.Toggle("Enable Health Bars", showHealthBars);
    }

    void DisableHealthBars()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    }
}
