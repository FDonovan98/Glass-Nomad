using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMoveForwards", menuName = "Commands/Move Forward")]
public class MoveForwards : CommandObject
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
            return movementValues.moveSpeed * movementValues.sprintMultiplier * agent.transform.forward;
        }
        else
        {
            return movementValues.moveSpeed * agent.transform.forward;
        }
    }
}
