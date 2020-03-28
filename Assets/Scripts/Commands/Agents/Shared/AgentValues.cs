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

    [Header("Spider Climb")]
    public float surfaceSwitchSpeed = 1.0f;
    public float gravityResetDelay = 0.5f;

    [Header("ChargeLeap")]
    public bool leapCanChargeInAir = true;
    public float leapVelocity = 10.0f;
    public float forwardLeapModifier = 1.0f;
    public float verticalLeapModifier = 1.0f;
    public float leapChargeDuration = 2.0f;
    public float currentLeapCharge = 0.0f;
     
    [Header("Camera")]
    public float mouseSensitivity = 1.0f;
    public float yRotationClamp = 80.0f;

    [Header("Toggleable Behaviours")]
    public Behaviour behaviourToToggle;

    [Header("Velocity Degradation")]
    public bool reduceVelocityInAir = true;
    public bool scaleVelocityDegWithVel = true;
    public float velocityDegradationGrounded = 80.0f;
    public float velocityDegradationInAir = 80.0f;

    [Header("Stairs")]
    public float maxStairHeight = 1.0f;
    public float minDistanceToStair = 1.0f;
    public float minLedgeWidth = 1.0f;
    public float stepUpAcceleration = 1.0f;

    [Header("Gravity")]
    public bool applyGravity = true;
    public float gravityAcceleration = 10.0f;
    public Vector3 gravityDirection = Vector3.down;


    [Header("Misc")]
    public GameObject menu = null;
    public bool isGrounded = true;
    public bool allowInput = true;

    public void Initialise()
    {
        isSprinting = false;
        gravityDirection = Vector3.down;
        allowInput = true;
        currentLeapCharge = 0.0f;
    }
}
