using UnityEngine;

public class AudioLogTriggerScript : TriggerInteractionScript
{
    [SerializeField] private AudioClip audioClip;

    protected override void InteractionComplete(GameObject player)
    {
        Objectives.PlaySpeechAudio(audioClip);
        Debug.Log("Audio log picked up");
        Destroy(this.gameObject);
    }
}
