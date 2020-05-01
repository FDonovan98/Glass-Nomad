using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAgentValues", menuName = "Commands/Agent Values")]
public class AgentValues : ScriptableObject
{
    [Header("XZ Movement")]
    public float moveAcceleration = 100.0f;
    public bool sprintingIsAToggle;
    public float sprintMultiplier = 2.0f;
    public float footstepDelay = 1.0f;

    [Header("Velocity Degradation")]
    public bool reduceVelocityInAir = true;
    public bool scaleVelocityDegWithVel = true;
    public float velocityDegradationGrounded = 80.0f;
    public float velocityDegradationInAir = 80.0f;

    [Header("Velocity Limit")]
    public float maxSpeed = 50.0f;
    public float maxSprintSpeed = 80.0f;
    public float maxSpeedInAir = 80.0f;
    public float maxSprintSpeedInAir = 100.0f;
    [Range(0.0f, 1.0f)]
    public float velocityLimitRate = 0.1f;

    [Header("ChargeLeap")]
    public bool leapCanChargeInAir = true;
    public float leapVelocity = 10.0f;
    public float forwardLeapModifier = 1.0f;
    public float verticalLeapModifier = 1.0f;
    public float leapChargeDuration = 2.0f;
    public float jumpCooldown = 0.1f;
    
    [Header("Spider Climb")]
    [Range(0.0f, 1.0f)]
    public float surfaceSwitchSpeed = 1.0f;
    public float gravityResetDelay = 0.5f;
     
    [Header("Camera")]
    public float mouseSensitivity = 1.0f;
    public float yRotationClamp = 80.0f;

    [Header("Aim Down Sight")]
    public bool aDSIsAToggle;

    [Header("Stairs")]
    public float maxStepHeight = 0.4f;
    public float stepSearchOvershoot = 0.01f;

    [Header("Gravity")]
    public bool applyGravity = true;
    public float gravityAcceleration = 10.0f;
    public float slopeLimitAngle = 30.0f;

    [Header("Oxygen")]
    public float maxOxygen = 100.0f;
    public float oxygenRegenModifier = 1.0f;
    public float suffocationDamage = 5.0f;

    [Header("Health")]
    public float maxHealth = 100.0f;

    [Header("Emergency Health Regen")]
    [Range(0.0f, 100.0f)]
    public float emergencyRegenThreshold = 20.0f;
    public float emergencyRegenMaxHealthModifier = 1.5f;
    public float emergencyRegenDownTickValue = 10.0f;
    [Range(0.0f, 1.0f)]
    public float postEmergencyRegenHealthModifier = 0.8f;
    public float emergencyRegenSpeedMultiplier = 1.5f;
    public int emergencyRegenUses = 1;

    [Header("UI Lag")]
    public bool[] lagUIInAxis = {false, false};
    [Range(0.0f, 1.0f)]
    public float[] UICatchupSpeed = {0.5f, 0.5f};

    [Header("Death Noise")]
    public AudioClip deathNoise;
}