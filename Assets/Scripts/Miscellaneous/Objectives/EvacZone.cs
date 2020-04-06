using UnityEngine;

public class EvacZone : MonoBehaviour
{
    // The number of marines currently in the evac zone.
    public int numberOfMarinesInEvac = 0;

    private void OnTriggerEnter(Collider coll)
    {
        UpdateMarineCount(coll, 1);
    }

    private void OnTriggerExit(Collider coll)
    {
        UpdateMarineCount(coll, -1);
    }

    /// <summary>
    /// Increases or decreases the number of marines that are currently in the evacuation zone.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="increment"></param>
    private void UpdateMarineCount(Collider player, int increment)
    {
        if (player.tag == "Player")
        {
            if (player.GetComponent<AgentController>().agentValues.name == "MarineAgentValues")
            {
                numberOfMarinesInEvac += increment;
            }
        }
    }
}