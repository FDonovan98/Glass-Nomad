using UnityEngine;

public class EvacZone : MonoBehaviour
{
    public int numberOfMarinesInEvac = 0;

    private void OnTriggerEnter(Collider coll)
    {
        UpdateMarineCount(coll, 1);
    }

    private void OnTriggerExit(Collider coll)
    {
        UpdateMarineCount(coll, -1);
    }

    private void UpdateMarineCount(Collider player, int increment)
    {
        if (player.tag == "Player")
        {
            if (player.gameObject.layer == 8) // Marine layer (Luke typed this, and Harry didn't even tell him to :D )
            {
                numberOfMarinesInEvac += increment;
            }
        }
    }
}