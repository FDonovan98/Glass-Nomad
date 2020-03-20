using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMoveLeft", menuName = "Commands/Move Left")]
public class MoveLeft : MoveRight
{
    public override void Execute(GameObject agent, MovementValues movementValues)
    {
        if (Input.GetKey(keycode))
        {
            agent.GetComponent<Rigidbody>().velocity -= CalculateVelocityVector(agent, movementValues);
        }
    }
}
