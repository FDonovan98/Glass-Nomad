using UnityEngine;

public class PlayerInteraction : ObjectInteraction
{
    public new InteractionType interactionType;

    public void ProcessTriggers(float deltaTime)
    {
        if (interactionType == InteractionType.Door)
        {
            float actionDuration = 5;

            if (deltaTime >= actionDuration)
            {
                // Run animation to open door
            }
        }
    }
}
