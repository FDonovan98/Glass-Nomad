using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAgentValues", menuName = "Commands/Agent Values")]
public class AgentValues : ScriptableObject
{
    [Header("XZ Movement")]
    public float moveSpeed = 1.0f;
    public bool sprintingIsAToggle;
    public bool isSprinting = false;
    public float sprintMultiplier = 2.0f;

    [Header("Camera")]
    public float mouseSensitivity = 1.0f;
    public float yRotationClamp = 80.0f;

    [Header("Misc")]
    public GameObject menu = null;

    public void Initialise()
    {
        isSprinting = false;
    }
}
