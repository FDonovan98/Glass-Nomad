using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAgentValues", menuName = "Commands/Agent Values")]
public class AgentValues : ScriptableObject
{
    [Header("XZ Movement")]
    public float moveSpeed = 1.0f;
    public bool sprintingIsAToggle;
    public bool isSprinting = false;
    public float sprintMultiplier = 2.0f;

    [Header("Velocity Degradation")]
    public bool reduceVelocityInAir = true;
    public bool scaleVelocityDegWithVel = true;
    public float velocityDegradationValue = 1.0f;

    [Header("Camera")]
    public float mouseSensitivity = 1.0f;
    public float yRotationClamp = 80.0f;

    [Header("Misc")]
    public GameObject menu = null;
    public bool isGrounded = true;

    public void Initialise()
    {
        isSprinting = false;
    }
}
