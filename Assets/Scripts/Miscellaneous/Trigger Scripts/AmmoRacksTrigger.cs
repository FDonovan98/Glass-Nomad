using Photon.Pun;
using UnityEngine;

public class AmmoRacksTrigger : TriggerInteractionScript
{
    public int maxAmmoGiven = 30;
    [ReadOnly] public int currAmmoGiven = 0;

    protected override void OnTriggerStay(Collider coll)
    {
        if (!coll.GetComponent<PhotonView>().IsMine) return;
        playerInteracting = coll.gameObject;
        
        if (playerInteracting.tag == "Player" && currAmmoGiven < 30 && !interactionComplete)
        {            
            if (Input.GetKey(inputKey) || inputKey == KeyCode.None)
            {
                if (currInteractTime >= interactTime)
                {
                    // Give player 30 ammo
                    currInteractTime = 0f;
                    interactionComplete = true;
                    currCooldownTime = cooldownTime;
                    playerInteracting.GetComponent<AgentInputHandler>().allowInput = true;
                    return;
                }

                currInteractTime += Time.deltaTime;
                float percentage = (currInteractTime / interactTime) * 100;
                if (debug) Debug.LogFormat("Interaction progress: {0}%", percentage);
                
                ReticleProgress.UpdateReticleProgress(percentage, outerReticle);
                playerInteracting.GetComponent<AgentInputHandler>().allowInput = false;
                return;
            }
            else if (Input.GetKeyUp(inputKey))
            {
                LeftTriggerArea();
            }
            
            interactionText.text = textToDisplay;
        }

        if (coll.tag == "Player" && interactionComplete)
        {
            coll.GetComponent<AgentInputHandler>().allowInput = true;
        }

        if (debug) Debug.LogFormat("Cooldown: {0} seconds", currCooldownTime);
    }
}
