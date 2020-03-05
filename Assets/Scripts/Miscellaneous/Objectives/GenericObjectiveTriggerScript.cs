using Photon.Pun;
using UnityEngine;

public class GenericObjectiveTriggerScript : TriggerInteractionScript
{
    [SerializeField] private string objectiveName = "";
    [SerializeField] private string objectiveRequired = "";
    [SerializeField] private bool destroyObjectAfter = true;

    protected override void InteractionComplete(GameObject player)
    {
        Objectives.ObjectiveComplete(objectiveName, objectiveRequired);
        if (destroyObjectAfter)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
