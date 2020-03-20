using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMovementValues", menuName = "Commands/Movement Values")]
public class MovementValues : ScriptableObject
{
    public float moveSpeed = 1.0f;
    public bool sprintingIsAToggle;
    public bool isSprinting = false;
    public float sprintMultiplier = 2.0f;

    public void Initialise()
    {
        isSprinting = false;
    }
}
