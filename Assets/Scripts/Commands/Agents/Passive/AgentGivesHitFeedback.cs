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

        if (agentInputHandler.agentHitSound != null)
        {
            AudioSource agentAudioSource = agent.GetComponent<AudioSource>();

            if (agentAudioSource == null)
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
            agentInputHandler.agentHitParticles.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogAssertion(agent.name + " doesn't have a hit feedback particle effect");
        }
    }
}