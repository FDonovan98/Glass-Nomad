using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMoveRight", menuName = "Commands/Move Right")]
public class MoveRight : CommandObject
{
    public override void Execute(GameObject agent, MovementValues movementValues)
    {
        if (Input.GetKey(keycode))
        {
            agent.GetComponent<Rigidbody>().velocity += CalculateVelocityVector(agent, movementValues);
        }
    }

    public Vector3 CalculateVelocityVector(GameObject agent, MovementValues movementValues)
    {
        if (movementValues.isSprinting)
        {
            return movementValues.moveSpeed * movementValues.sprintMultiplier * agent.transform.right;
        }
        else
        {
            return movementValues.moveSpeed * agent.transform.right;
        }
    }
}
