using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerInteraction : ObjectInteraction
{
    public void ProcessTriggers(float deltaTime, bool isMarine)
    {
        // Script to execute if interacting with a door.
        if (interactionType == InteractionType.Door && isMarine)
        {
            float actionDuration = 2;

            Debug.Log(deltaTime);

            if (deltaTime >= actionDuration)
            {
                // animator.GetComponent<Animator>().Play(anim.name);
                Debug.Log("Open Door");
                TriggerAnimation();
            }
        }
    }

    private void TriggerAnimation()
    {
        byte eventCode = 1;
        int gameObjectID = animator.GetInstanceID();

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions 
        {
            Receivers = ReceiverGroup.All
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent(eventCode, gameObjectID, raiseEventOptions, sendOptions);

    }
}
