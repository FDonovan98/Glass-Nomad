using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "DefaultAgentGivesHitFeedback", menuName = "Commands/Passive/AgentGivesHitFeedback", order = 0)]
public class AgentGivesHitFeedback : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnAgentHasBeenHit += RunCommandOnAgentHasBeenHit;
    }

    /// <summary>
    /// The method to be run when an agent has been hit.
    /// </summary>
    /// <param name="agentInputHandler"></param>
    /// <param name="position">The position when the agent was hit.</param>
    /// <param name="normal">The normal of the hit position.</param>
    /// <param name="value"></param>
    private void RunCommandOnAgentHasBeenHit(AgentInputHandler agentInputHandler, Vector3 position, Vector3 normal, float value)
    {
        Debug.Log("AGENT HIT FEEDBACK");
        
        GameObject agent = agentInputHandler.gameObject;

        if (agentInputHandler.agentHitParticles != null)
        {
            Debug.Log("Instantiating hit effects!");
			GameObject hitEffect = Instantiate(agentInputHandler.agentHitParticles, position, Quaternion.Euler(normal));

            if (agentInputHandler.agentHitSound != null)
            {   
                AudioSource audioSource = hitEffect.GetComponent<AudioSource>();
                audioSource.PlayOneShot(agentInputHandler.agentHitSound);
            }
            else
            {
                Debug.LogAssertion(agent.name + " doesn't have a hit feedback sound");
            }

            Destroy(hitEffect, 5f);
        }
        else
        {
            Debug.LogAssertion(agent.name + " doesn't have a hit feedback particle effect");
        }
    }

    private IEnumerator DisableHitParticlesObject()
    {
        yield return null;
    }
}