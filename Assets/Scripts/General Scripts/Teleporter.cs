using UnityEngine;

public class Teleporter : TriggerInteractionScript
{
    [SerializeField] private GameObject linkedTeleporter = null; // The destination of the TP.
    [SerializeField] private bool biDirectional = true; // If false, then this TP can ONLY be used TO teleport, and NOT FROM.

    private new void OnTriggerStay(Collider coll)
    {
        if (biDirectional)
        {
            base.OnTriggerStay(coll);
        }
    }

    protected override void InteractionComplete(GameObject player)
    {
        Debug.Log("teleporting player to: " + linkedTeleporter.transform.position);
        player.transform.position = linkedTeleporter.transform.position;
    }
}
