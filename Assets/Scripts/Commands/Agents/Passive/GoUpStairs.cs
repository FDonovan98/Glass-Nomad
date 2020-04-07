//Code Source: https://cobertos.com/blog/post/how-to-climb-stairs-unity3d/

using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGoUpStairs", menuName = "Commands/Passive/GoUpStairs")]
public class GoUpStairs : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnCollisionEnter += RunCommandOnCollisionEnter;
        agentInputHandler.runCommandOnCollisionStay += RunCommandOnCollisionStay;
        agentInputHandler.runCommandOnFixedUpdate += RunCommandOnFixedUpdate;
    }

    void RunCommandOnFixedUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        Vector3 stepUpOffset;
        Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();

        if (agentInputHandler.isGrounded)
        {
            if (FindStair(out stepUpOffset, agent, agentRigidbody, agentInputHandler, agentValues))
            {
                agentRigidbody.position += stepUpOffset;
                agentRigidbody.velocity = agentInputHandler.lastVelocity;
            }
        }

        agentInputHandler.lastVelocity = agentRigidbody.velocity;
        agentInputHandler.allCPs.Clear();
    }

    bool FindStair(out Vector3 stepUpOffset, GameObject agent, Rigidbody agentRigidbody, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        stepUpOffset = Vector3.zero;

        Vector2 agentXZVel = new Vector2(agentRigidbody.velocity.x, agentRigidbody.velocity.z);

        if (agentXZVel.sqrMagnitude > 0.0f)
        {
            foreach (ContactPoint element in agentInputHandler.allCPs)
            {
                if (CheckForStair(out stepUpOffset, agent, element, agentValues, agentXZVel, agentInputHandler))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    bool CheckForStair(out Vector3 stepUpOffset, GameObject agent, ContactPoint contactPoint, AgentValues agentValues, Vector2 agentXZVel, AgentInputHandler agentInputHandler)
    {
        stepUpOffset = Vector3.zero;
        // Should be changed to check for angle between horizontal and normal.
        if (Mathf.Abs(contactPoint.normal.y) > 0.01f)
        {
            return false;
        }

        float bottomOfAgent = agent.transform.position.y - agent.GetComponent<Collider>().bounds.extents.y;

        if (contactPoint.point.y - bottomOfAgent > agentValues.maxStairHeight)
        {
            return false;
        }

        // Overcast is sent in direction of player movement.
        RaycastHit hit;
        float rayOriginHeight = bottomOfAgent + agentValues.maxStepHeight;
        Vector3 overshootDirection = new Vector3(agentXZVel.x, 0.0f, agentXZVel.y);
        Vector3 rayOrigin = new Vector3(contactPoint.point.x, rayOriginHeight, contactPoint.point.z);
        rayOrigin += overshootDirection * agentValues.stepSearchOvershoot;

        Ray ray = new Ray(rayOrigin, agentInputHandler.gravityDirection);

        if (!(contactPoint.otherCollider.Raycast(ray, out hit, agentValues.maxStepHeight)))
        {
            return false;
        }

        stepUpOffset = new Vector3(0.0f, hit.point.y + 0.0001f - bottomOfAgent, 0.0f);
        stepUpOffset += overshootDirection * agentValues.stepSearchOvershoot;

        return true;
    }

    void RunCommandOnCollisionEnter(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collision other)
    {
        agentInputHandler.allCPs.AddRange(other.contacts);
    }

    void RunCommandOnCollisionStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collision other)
    {
        agentInputHandler.allCPs.AddRange(other.contacts);
    }
}