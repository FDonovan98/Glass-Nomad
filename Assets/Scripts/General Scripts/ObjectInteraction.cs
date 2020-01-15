using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public enum InteractionType
    {
        None,
        Door
    }

    protected InteractionType interactionType;

    void OnTriggerEnter(Collider other)
    {
        if (interactionType == InteractionType.Door && other.tag == "Player")
        {
            // Time the action has been going on for.
            float interactionDuration;
            
            // Time taken for the action to complete.
            float actionDuration;

            PlayerInteraction playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
            playerInteraction.interactionType = interactionType;
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
        playerInteraction.interactionType = InteractionType.None;
    }
}
