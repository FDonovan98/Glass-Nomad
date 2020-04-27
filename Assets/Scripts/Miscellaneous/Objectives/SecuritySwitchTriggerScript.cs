using UnityEngine;
using Photon.Pun;

public class SecuritySwitchTriggerScript : TriggerInteractionScript
{

    // Tells the red switch manager when this switch has been (de)activated.
    [SerializeField] private SecuritySwitchManager switchManager = null;
    [SerializeField] private Material materialToChange;
    [SerializeField] private Color colorToChangeTo;
    private Color prevColor = Color.clear;

    /// <summary>
    /// Once the player enters the switch's collider and their holding 'E',
    /// the timer is started. If the player successfully holds the switch for
    /// the duration of the timer, then the switch is activated. If the player
    /// releases the switch, then it is deactivated and will need to be activated
    /// again.
    /// </summary>
    /// <param name="coll"></param>
    private new void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Player" && coll.gameObject.layer == 8 && currCooldownTime <= 0)
        {
            if (Input.GetKey(inputKey))
            {
                if (!interactionComplete)
                {
                    if (currInteractTime >= interactTime)
                    {
                        photonView.RPC("InteractionComplete", RpcTarget.All);
                        currInteractTime = 0f;
                        currCooldownTime = cooldownTime;
                    }

                    currInteractTime += Time.deltaTime;
                    float percentage = (currInteractTime / interactTime) * 100;
                    if (debug) Debug.LogFormat("Interaction progress: {0}%", percentage);

                    ReticleProgress.UpdateReticleProgress(percentage, outerReticle);
                    playerInteracting.GetComponent<AgentInputHandler>().allowInput = false;
                    return;
                }
            }
            else // if the player is not pressing then reset the switch's state.
            {
                LeftTriggerArea();
            }

            interactionText.text = textToDisplay;
        }
    }

    [PunRPC]
    protected override void InteractionComplete()
    {
        if (debug) Debug.Log("Switch activated");
        interactionComplete = true;
        switchManager.SwitchActivated();
        prevColor = materialToChange.GetColor("_EmissionColor");
        materialToChange.SetColor("_EmissionColor", colorToChangeTo);
    }

    /// <summary>
    /// If the player exits the switch's collider, then reset the switch's state and timer.
    /// </summary>
    protected override void LeftTriggerArea()
    {
        if (interactionComplete)
        {
            photonView.RPC("Deactivate", RpcTarget.All);
        }
        playerInteracting.GetComponent<AgentInputHandler>().allowInput = true;
        base.LeftTriggerArea();
    }

    [PunRPC]
    private void Deactivate()
    {
        if (debug) Debug.Log("Switch deactivated");
        switchManager.SwitchDeactivated();
        interactionComplete = false;
        materialToChange.SetColor("_EmissionColor", prevColor);
    }
}
