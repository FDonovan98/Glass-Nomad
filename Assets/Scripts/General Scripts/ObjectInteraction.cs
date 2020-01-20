using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ObjectInteraction : MonoBehaviour
{
    public enum InteractionType
    {
        None,
        Door,
        Generator
    }

    public InteractionType interactionType;
    public AnimationClip anim;
    public GameObject animator;

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("Event Recieved");
        byte eventCode = photonEvent.Code;

        if (eventCode == (byte)1)
        {
            int targetID = (int)photonEvent.CustomData;
            if (this.GetInstanceID() == targetID) 
            {
                animator.GetComponent<Animator>().Play(anim.name);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SetPlayerInteraction(other, false);
    }

    private void OnTriggerExit(Collider other)
    {
        SetPlayerInteraction(other, true);
    }

    private void SetPlayerInteraction(Collider other, bool resetValues)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<AlienController>() != null)
            {
                AlienController controller = other.gameObject.GetComponent<AlienController>();
                SetPlayerInteractionValues(controller.alienInteraction, resetValues);
            } 
            else
            {
                MarineController controller = other.gameObject.GetComponent<MarineController>();
                SetPlayerInteractionValues(controller.marineInteraction, resetValues);
            }
        }
    }

    private void SetPlayerInteractionValues(PlayerInteraction playerInteraction, bool resetValues)
    {
        if (resetValues)
        {
            playerInteraction.interactionType = InteractionType.None;
            Debug.Log(playerInteraction.interactionType.ToString());
            playerInteraction.anim = null;
            playerInteraction.animator = null;
        }
        else
        {
            playerInteraction.interactionType = interactionType;
            Debug.Log(playerInteraction.interactionType.ToString());
            playerInteraction.anim = anim;
            playerInteraction.animator = animator;
        }
    }
}
