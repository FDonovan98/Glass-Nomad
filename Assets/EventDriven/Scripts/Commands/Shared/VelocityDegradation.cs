using UnityEngine;

[CreateAssetMenu(fileName = "VelocityDegradation", menuName = "Commands/Passive/Velocity Degradation")]
public class VelocityDegradation : PassiveCommandObject
{
    public override void Execute(GameObject agent, AgentValues agentValues)
    {
        if (!Input.anyKey)
        {
            if (agentValues.reduceVelocityInAir || agentValues.isGrounded)
            {
                Rigidbody charRigidbody = agent.GetComponent<Rigidbody>();

                float[] xzVel = 
                {
                    charRigidbody.velocity.x,
                    charRigidbody.velocity.z
                };

                float RelativeVelDeg = agentValues.velocityDegradationValue * Time.deltaTime;

                if (agentValues.scaleVelocityDegWithVel)
                {
                    RelativeVelDeg *= charRigidbody.velocity.magnitude;
                }                

                for (int i = 0; i < 2; i++)
                {
                    if (xzVel[i] > 0.0f)
                    {
                        xzVel[i] = Mathf.Clamp(xzVel[i] - RelativeVelDeg, 0.0f, xzVel[i]);
                    }
                    else if (xzVel[i] < 0.0f)
                    {
                        xzVel[i] = Mathf.Clamp(xzVel[i] + RelativeVelDeg, xzVel[i], 0.0f);
                    }
                    
                }

                charRigidbody.velocity = new Vector3(xzVel[0], charRigidbody.velocity.y, xzVel[1]);
            }
        }
    }
}