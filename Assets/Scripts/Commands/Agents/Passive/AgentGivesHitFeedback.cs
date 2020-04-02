using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAgentGivesHitFeedback", menuName = "Commands/Passive/AgentGivesHitFeedback", order = 0)]
public class AgentGivesHitFeedback : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnAgentHasBeenHit += RunCommandOnAgentHasBeenHit;
    }

    void RunCommandOnAgentHasBeenHit(AgentInputHandler agentInputHandler, Vector3 position, float value)
    {
        GameObject agent = agentInputHandler.gameObject;

        if (agentInputHandler.currentHealth - value > 0)
        {
            agentInputHandler.currentHealth -= value;
            Debug.LogFormat("{0}'s new health: {1}", agent.name, agentInputHandler.currentHealth);
        }
        else
        {
            // Player has died.
            Debug.LogFormat("{0}'s has died.", agent.name);
        }

        if (agentInputHandler.agentHitSound != null)
        {
            AudioSource agentAudioSource = agent.GetComponent<AudioSource>();

            if (agentAudioSource != null)
            {
                agentAudioSource = agent.AddComponent(typeof(AudioSource)) as AudioSource;
            }

            agentAudioSource.clip = agentInputHandler.agentHitSound;
            agentAudioSource.Play();
        }
        else
        {
            Debug.LogAssertion(agent.name + " doesn't have a hit feedback sound");
        }

        if (agentInputHandler.agentHitParticles != null)
        {
            agentInputHandler.agentHitParticles.transform.position = position;
            agentInputHandler.agentHitParticles.Play();
        }
        else
        {
            Debug.LogAssertion(agent.name + " doesn't have a hit feedback particle effect");
        }
    }
}