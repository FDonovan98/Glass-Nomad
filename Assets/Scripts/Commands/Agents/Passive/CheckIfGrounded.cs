using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

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
        if (!agent.GetComponent<PhotonView>().IsMine) return;
        
        bool foundGround = false;
        ContactPoint currentGround = new ContactPoint();
        float currentGroundTheta = float.MaxValue;

        foreach (ContactPoint element in allCPs)
        {
            // Should be changed to use a slope angle.

            float cosTheta = Vector3.Dot(element.normal, agentInputHandler.gravityDirection);
            float theta = Mathf.Abs(Mathf.Acos(cosTheta) * Mathf.Rad2Deg - 180);
            
            // Catches bug cause when cosTheta == -1.
            if (float.IsNaN(theta))
            {
                theta = 0.0f;
            }

            if (theta < agentValues.slopeLimitAngle && theta < currentGroundTheta)
            {
                foundGround = true;
                currentGround = element;
                currentGroundTheta = theta;
            }
        }

        if (!foundGround)
        {
            Vector3 averageNormal = Vector3.zero;

            foreach (ContactPoint element in allCPs)
            {
                averageNormal += element.normal;
            }

            averageNormal = averageNormal.normalized;

            float cosTheta = Vector3.Dot(averageNormal, agentInputHandler.gravityDirection);
            float theta = Mathf.Abs(Mathf.Acos(cosTheta) * Mathf.Rad2Deg - 180);
            
            // Catches bug cause when cosTheta == -1.
            if (float.IsNaN(theta))
            {
                theta = 0.0f;
            }

            if (theta < agentValues.slopeLimitAngle && theta < currentGroundTheta)
            {
                foundGround = true;
                currentGround = default(ContactPoint);
                currentGroundTheta = theta;
            }
        }

        if (!foundGround && agentInputHandler.currentLeapCharge > 0)
        {
            agentInputHandler.currentLeapCharge = 0.0f;
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