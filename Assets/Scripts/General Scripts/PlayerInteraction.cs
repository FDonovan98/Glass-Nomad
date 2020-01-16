using UnityEngine;

public class PlayerInteraction : ObjectInteraction
{
    public new InteractionType interactionType;
    public new AnimationClip anim;
    public new GameObject animator;

    public void ProcessTriggers(float deltaTime, bool isMarine)
    {
        // Script to execute if interacting with a door.
        if (interactionType == InteractionType.Door && isMarine)
        {
            float actionDuration = 2;

            Debug.Log(deltaTime);

            if (deltaTime >= actionDuration)
            {
                animator.GetComponent<Animator>().Play(anim.name);
            }
        }
    }
}
