using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAgentValues", menuName = "Commands/Agent Values")]
public class AgentValues : ScriptableObject
{
    [Header("XZ Movement")]
    public float moveAcceleration = 100.0f;
    public float maxSpeed = 50.0f;
    public bool sprintingIsAToggle;
    public bool isSprinting = false;
    public float sprintMultiplier = 2.0f;

    [Header("Velocity Degradation")]
    public bool reduceVelocityInAir = true;
    public bool scaleVelocityDegWithVel = true;
    public float velocityDegradationValue = 80.0f;

    [Header("Stairs")]
    public float maxStairHeight = 1.0f;
    public float minDistanceToStair = 1.0f;
    public float minLedgeWidth = 1.0f;
    public float stepUpAcceleration = 1.0f;

    [Header("Gravity")]
    public bool applyGravity = true;
    public float gravityAcceleration = 10.0f;
    public Vector3 gravityDirection = Vector3.down;

    [Header("Camera")]
    public float mouseSensitivity = 1.0f;
    public float yRotationClamp = 80.0f;

    [Header("Misc")]
    public GameObject menu = null;
    public bool isGrounded = true;

    public void Initialise()
    {
        isSprinting = false;
        gravityDirection = Vector3.down;
    }
}
