using UnityEditor;

public class ObjectiveCreation : EditorWindow
{
    [MenuItem("Window/Dev Tools/Objective Creation")]
    public static void ShowWindow()
    {
        GetWindow<ObjectiveCreation>("Objective Creation");
    }
}
