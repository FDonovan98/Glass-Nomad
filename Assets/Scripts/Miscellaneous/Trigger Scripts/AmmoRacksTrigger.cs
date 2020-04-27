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
                if (currAmmoGiven == maxAmmoGiven)
                {
                    interactionComplete = true;
                    playerInteracting.GetComponent<AgentInputHandler>().allowInput = true;
                    return;
                }

                currInteractTime += Time.deltaTime;

                if (currInteractTime > (interactTime / maxAmmoGiven) + (currAmmoGiven * (interactTime / maxAmmoGiven)))
                {
                    currAmmoGiven++;
                    playerInteracting.GetComponent<AgentController>().ChangeStat(ResourceType.ExtraAmmo, 1);
                }

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
    }

    protected override void LeftTriggerArea()
    {
        ReticleProgress.UpdateReticleProgress(0, outerReticle);
        playerInteracting.GetComponent<AgentInputHandler>().allowInput = true;
    }
}
