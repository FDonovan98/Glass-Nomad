using System.Collections;
using UnityEngine;

public class FinalObjective : TriggerInteractionScript
{
    [SerializeField] private float waitTimer = 10f;
    [SerializeField] private float timer = 30f;

    protected override void InteractionComplete(GameObject player)
    {
        base.InteractionComplete(player);

        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        // Wait for text to finish displaying and audio to start playing
        for (float i = 0; i < waitTimer; i += Time.deltaTime)
        {
            yield return null;
        }

        for (float i = 0; i < timer; i += Time.deltaTime)
        {
            Debug.Log("TIMER : " + i);
            yield return null;
        }
        
        StartGameOverSequence();
    }

    private void StartGameOverSequence()
    {
        Debug.Log("GAME OVER!");
    }
}
