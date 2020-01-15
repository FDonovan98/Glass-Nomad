using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    // Time taken for the action to complete.
    public float actionDuration;
    public GameObject actionArea;

    // Time the action has been going on for.
    private float interactionDuration;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MarineMovement marineMovement = other.gameObject.GetComponent<MarineMovement>();
        }
    }
}
