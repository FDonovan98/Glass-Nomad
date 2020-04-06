using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAgentValues", menuName = "Commands/Agent Values")]
public class AgentValues : ScriptableObject
{
    [Header("XZ Movement")]
    public float moveAcceleration = 100.0f;
    public float maxSpeed = 50.0f;
    public float maxSprintSpeed = 80.0f;
    public bool sprintingIsAToggle;
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
    public float jumpCooldown = 0.1f;
     
    [Header("Camera")]
    public float mouseSensitivity = 1.0f;
    public float yRotationClamp = 80.0f;

    [Header("Aim Down Sight")]
    public bool aDSIsAToggle;

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

    [Header("Oxygen")]
    public float maxOxygen = 100.0f;
    public float oxygenRegenModifier = 1.0f;

    [Header("Health")]
    public float maxHealth = 100.0f;

    [Header("UI Lag")]
    public bool lagUIInX = false;
    public float UIXLagTime = 0.2f;
}