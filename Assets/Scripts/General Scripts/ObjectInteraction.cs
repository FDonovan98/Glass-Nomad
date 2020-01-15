using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public enum InteractionType
    {
        None,
        Door
    }

    protected InteractionType interactionType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
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
