using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMoveBackwards", menuName = "Commands/Move Backwards")]
public class MoveBackwards : MoveForwards
{
    public override void Execute(GameObject agent, MovementValues movementValues)
    {
        if (Input.GetKey(keycode))
        {
            agent.GetComponent<Rigidbody>().velocity -= CalculateVelocityVector(agent, movementValues);
        }
    }
}
