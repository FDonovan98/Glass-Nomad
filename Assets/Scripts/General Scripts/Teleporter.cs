using UnityEngine;
using Photon.Pun;

public class Teleporter : TriggerInteractionScript
{
	[Header("Teleporter Interaction")]
    [SerializeField][ReadOnly] private bool powered = false;
    [SerializeField] private GameObject linkedTeleporter = null; // The destination of the TP.
    [SerializeField] private bool biDirectional = true; // If false, then this TP can ONLY be used TO teleport, and NOT FROM.

    [SerializeField]
    private Behaviour[] behavioursToEnableWithPower;

    [SerializeField]
    private AudioClip audioClip = null;
    [SerializeField]
    private AudioSource audioSource = null;

    private new void OnTriggerStay(Collider coll)
    {
        if (biDirectional && powered)
        {
            base.OnTriggerStay(coll);
        }
    }

    /// <summary>
    /// Once the interaction is complete, we teleport the player from this teleporter, to the linked teleporter.
    /// </summary>
    /// <param name="player">The player to teleport.</param>
    [PunRPC]
    protected override void InteractionComplete()
    {
        Debug.Log("teleporting player to: " + linkedTeleporter.transform.position);
        Vector3 spawnLocation = linkedTeleporter.transform.position;
        spawnLocation += new Vector3(0.0f, playerInteracting.GetComponent<Collider>().bounds.extents.y + 1f, 0.0f);
        playerInteracting.transform.position = spawnLocation;

        if (audioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Power()
    {
        powered = true;
        
        foreach (Behaviour element in behavioursToEnableWithPower)
        {
            element.enabled = true;
        }
    }
}
