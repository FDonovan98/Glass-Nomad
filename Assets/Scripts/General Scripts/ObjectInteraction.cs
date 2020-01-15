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
            if (other.gameObject.GetComponent<AlienController>() != null)
            {
                AlienController controller = other.gameObject.GetComponent<AlienController>();
                controller.alienInteraction.interactionType = interactionType;
            } 
            else
            {
                MarineController controller = other.gameObject.GetComponent<MarineController>();
                controller.marineInteraction.interactionType = interactionType;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<AlienController>() != null)
        {
            AlienController controller = other.gameObject.GetComponent<AlienController>();
            controller.alienInteraction.interactionType = InteractionType.None;
        } 
        else
        {
            MarineController controller = other.gameObject.GetComponent<MarineController>();
            controller.marineInteraction.interactionType = InteractionType.None;
        }
    }
}
