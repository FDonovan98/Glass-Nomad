using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGoUpStairs", menuName = "Commands/Passive/GoUpStairs")]
public class GoUpStairs : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnUpdate(GameObject agent, AgentValues agentValues)
    {
        if (agentValues.isGrounded)
        {
            Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();
            Vector3 rayDirection = agentRigidbody.velocity.normalized;
            rayDirection.y = 0.0f;

            Collider agentCollider = agent.GetComponent<Collider>();

            Vector3 rayOrigin = agent.transform.position;
            rayOrigin.y -= agentCollider.bounds.extents.y;

            float rayMaxDistance = agentValues.minDistanceToStair;
            rayMaxDistance += GetBoundsExtentInDirection(agentCollider.bounds.extents, rayDirection);

            RaycastHit stairCheck;
            Debug.DrawRay(rayOrigin, rayDirection * rayMaxDistance, Color.red);
            Vector3 debugRayOrigin = rayOrigin;
            debugRayOrigin.y += agentValues.maxStairHeight;

            if (Physics.Raycast(rayOrigin, rayDirection, out stairCheck, agentValues.minDistanceToStair))
            {
                rayDirection = -agent.transform.up;
                rayMaxDistance = agentValues.maxStairHeight;

                Vector3 xzVel = agentRigidbody.velocity.normalized;
                xzVel.y = 0.0f;
                rayOrigin = stairCheck.point + xzVel * agentValues.minLedgeWidth;
                rayOrigin.y += agentValues.maxStairHeight;

                RaycastHit topOfStair;
                Debug.DrawRay(rayOrigin, rayDirection * rayMaxDistance, Color.green);
                if (Physics.Raycast(rayOrigin, rayDirection, out topOfStair, rayMaxDistance))
                {
                    Vector3 localStairBottom = agent.transform.worldToLocalMatrix * stairCheck.point;
                    Vector3 localStairTop = agent.transform.worldToLocalMatrix * topOfStair.point;

                    float stairHeight = Mathf.Abs(localStairBottom.y - localStairTop.y);

                    ApplyUpwardsForce(agent, agentValues, stairHeight);
                }
            }
        }
    }

    void ApplyUpwardsForce(GameObject agent, AgentValues agentValues, float stepHeight)
    {
        Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();

        agentRigidbody.velocity += -agentValues.gravityDirection.normalized * (agentValues.gravityAcceleration + agentValues.stepUpAcceleration) * Time.deltaTime * (stepHeight / agentValues.maxStairHeight);
    }

    // Currently assumes direction.y == 0.
    // I'm not doing this maths in 3D, my brain will melt.
    float GetBoundsExtentInDirection(Vector3 agentColliderExtents, Vector3 direction)
    {
        float angleTheta = Mathf.Atan(direction.x / direction.z);

        if (Mathf.Abs(direction.z - agentColliderExtents.z) > Mathf.Abs(direction.x - agentColliderExtents.x))
        {
            return agentColliderExtents.z * 1 / (Mathf.Sin(Mathf.PI - angleTheta));
        }
        else
        {
            return agentColliderExtents.x * 1 / (Mathf.Sin(angleTheta));
        }
    }
}