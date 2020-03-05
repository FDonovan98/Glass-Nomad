using UnityEngine;

public class AudioLogTriggerScript : TriggerInteractionScript
{
    protected override void InteractionComplete(GameObject player)
    {
        Debug.Log("Audio log picked up");
        Destroy(this.gameObject);
    }
}
