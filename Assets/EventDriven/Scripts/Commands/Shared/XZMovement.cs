using UnityEngine;

[CreateAssetMenu(fileName = "DefaultXZMovement", menuName = "Commands/Active/XZ Movement")]
public class XZMovement : ActiveCommandObject
{
    [SerializeField]
    private KeyCode MoveForward = KeyCode.W;
    [SerializeField]
    private KeyCode MoveBack = KeyCode.S;
    [SerializeField]
    private KeyCode MoveLeft = KeyCode.A;
    [SerializeField]
    private KeyCode MoveRight = KeyCode.D;

    protected override void OnEnable()
    {
        keyTable.Add("Move Forward", MoveForward);
        keyTable.Add("Move Back", MoveBack);
        keyTable.Add("Move Left", MoveLeft);
        keyTable.Add("Move Right", MoveRight);
    }

    public override void Execute(GameObject agent, AgentValues agentValues)
    {
        Vector3 movementVector = GetKeyInput(agent);

        movementVector *= agentValues.moveSpeed * Time.deltaTime;

        if (agentValues.isSprinting)
        {
            agent.GetComponent<Rigidbody>().velocity += movementVector * agentValues.sprintMultiplier;
        }
        else
        {
            agent.GetComponent<Rigidbody>().velocity += movementVector;
        }
    }

    Vector3 GetKeyInput(GameObject agent)
    {
        Vector3 movementVector = Vector3.zero;

        if (Input.GetKey(MoveForward))
        {
            movementVector += agent.transform.forward;
        }
        if (Input.GetKey(MoveBack))
        {
            movementVector -= agent.transform.forward;
        }
        if (Input.GetKey(MoveLeft))
        {
            movementVector -= agent.transform.right;
        }
        if (Input.GetKey(MoveRight))
        {
            movementVector += agent.transform.right;
        }

        return movementVector.normalized;
    }
}
