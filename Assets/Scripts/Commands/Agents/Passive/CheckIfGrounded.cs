using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefualtCheckIfGrounded", menuName = "Commands/Passive/CheckIfGrounded")]
public class CheckIfGrounded : PassiveCommandObject
{
    List<ContactPoint> allCPs = new List<ContactPoint>();
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnCollisionEnter += RunCommandOnCollisionEnter;
        agentInputHandler.runCommandOnCollisionStay += RunCommandOnCollisionStay;
        agentInputHandler.runCommandOnFixedUpdate += RunCommandOnFixedUpdate;
    }

    void RunCommandOnFixedUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        bool foundGround = false;
        ContactPoint currentGround = new ContactPoint();
        float currentGroundTheta = float.MaxValue;

        foreach (ContactPoint element in allCPs)
        {
            // Should be changed to use a slope angle.

            float cosTheta = Vector3.Dot(element.normal, agentInputHandler.gravityDirection);
            float theta = Mathf.Abs(Mathf.Acos(cosTheta) * Mathf.Rad2Deg - 180);

            if (theta < agentValues.slopeLimitAngle && theta < currentGroundTheta)
            {
                foundGround = true;
                currentGround = element;
                currentGroundTheta = theta;
            }
        }

        agentInputHandler.isGrounded = foundGround;
        agentInputHandler.groundContactPoint = currentGround;
        allCPs.Clear();
    }

    void RunCommandOnCollisionEnter(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collision other)
    {
        allCPs.AddRange(other.contacts);
    }

    void RunCommandOnCollisionStay(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues, Collision other)
    {
        allCPs.AddRange(other.contacts);
    }
}