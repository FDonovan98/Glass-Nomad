using Photon.Pun;
using UnityEngine;

public class GenericObjectiveTriggerScript : TriggerInteractionScript
{
    [SerializeField] private string objectiveName = "";
    [SerializeField] private string objectiveRequired = "";
    [SerializeField] private bool destroyObjectAfter = true;
    [SerializeField] private GameObject objectToDestroy = null;

    protected override void InteractionComplete(GameObject player)
    {
        Objectives.ObjectiveComplete(objectiveName, objectiveRequired);
        
        GetComponent<Collider>().enabled = false;
        if (destroyObjectAfter)
        {
            if (objectToDestroy == null) PhotonNetwork.Destroy(gameObject);
            else PhotonNetwork.Destroy(objectToDestroy);
        }
    }
}
