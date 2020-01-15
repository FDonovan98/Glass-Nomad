using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public enum InteractionType
    {
        None,
        Door
    }

    public InteractionType interactionType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AlienController playerInteraction = other.gameObject.GetComponent<AlienController>();
            playerInteraction.playerInteraction.interactionType = interactionType;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.gameObject.GetComponent<PlayerInteraction>();
        playerInteraction.interactionType = InteractionType.None;
    }
}
