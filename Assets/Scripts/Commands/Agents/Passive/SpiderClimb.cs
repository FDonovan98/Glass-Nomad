using UnityEngine;

using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefaultSpiderClimb", menuName = "Commands/Passive/SpiderClimb")]
public class SpiderClimb : PassiveCommandObject
{
    float timeToGravityReset;
    List<ContactPoint> allCPs = new List<ContactPoint>();
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        timeToGravityReset = -1.0f;
        agentInputHandler.runCommandOnFixedUpdate += RunCommandOnFixedUpdate;
        agentInputHandler.runCommandOnCollisionEnter += RunCommandOnCollisionEnter;
        agentInputHandler.runCommandOnCollisionStay += RunCommandOnCollisionStay;
    }

    void RunCommandOnFixedUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        SetGravityDirection(agentInputHandler, agentValues);

        if (-agent.transform.up != agentInputHandler.gravityDirection)
        {
            Vector3 agentTargetForward = Vector3.Cross(agent.transform.right, -agentInputHandler.gravityDirection);

            Quaternion agentTargetRotation = Quaternion.LookRotation(agentTargetForward, -agentInputHandler.gravityDirection);
            
            //agent.GetComponent<Rigidbody>().velocity -= agentValues.gravityDirection;
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, agentTargetRotation, agentValues.surfaceSwitchSpeed);
        }
        
    }

    void SetGravityDirection(AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        Vector3 averageNormal = Vector3.zero;
        foreach (ContactPoint element in allCPs)
        {
            averageNormal -= element.normal;
        }

        if (averageNormal != Vector3.zero)
        {
            agentInputHandler.gravityDirection = averageNormal.normalized;
            timeToGravityReset = agentValues.gravityResetDelay;
        }
        else 
        {
            timeToGravityReset -= Time.fixedDeltaTime;
        }

        if (timeToGravityReset < 0.0f)
        {
            agentInputHandler.gravityDirection = Vector3.down;
            timeToGravityReset = agentValues.gravityResetDelay;
        }
        
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