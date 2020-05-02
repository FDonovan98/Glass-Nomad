using UnityEngine;
using Photon.Pun;

public class SecuritySwitchTriggerScript : TriggerInteractionScript
{

    // Tells the red switch manager when this switch has been (de)activated.
    [SerializeField] private SecuritySwitchManager switchManager = null;
    [SerializeField]
    private Renderer rendererToChangeMaterial;
    [SerializeField]
    private Material materialToChangeFrom;
    [SerializeField] private Material materialToChangeTo = null;

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
        if (!coll.GetComponent<PhotonView>().IsMine) return;

        if (coll.tag == "Player" && coll.gameObject.layer == 8)
        {
            checkForInput = true;

            interactionText.text = textToDisplay;
        }
    }

    private new void Update()
    {
        if (checkForInput)
        {
            if (!interactionComplete && Input.GetKey(inputKey))
            {
                if (currInteractTime >= interactTime)
                {
                    photonView.RPC("InteractionComplete", RpcTarget.All);
                    checkForInput = false;
                    return;
                }

                currInteractTime += Time.deltaTime;
                float percentage = (currInteractTime / interactTime) * 100;
                if (debug) Debug.LogFormat("Interaction progress: {0}%", percentage);

                ReticleProgress.UpdateReticleProgress(percentage, outerReticle);
                playerInteracting.GetComponent<AgentInputHandler>().allowInput = false;
                return;
            }
            else if (Input.GetKeyUp(inputKey))// if the player is not pressing then reset the switch's state.
            {
                LeftTriggerArea();
                checkForInput = false;
            }
        }
    }

    [PunRPC]
    protected override void InteractionComplete()
    {
        checkForInput = false;
        Debug.Log("Switch activated");
        interactionComplete = true;
        switchManager.SwitchActivated();
        rendererToChangeMaterial.material = materialToChangeTo;
        base.LeftTriggerArea();
        if (interactionText != null) interactionText.text = string.Empty;
        playerInteracting = null;
    }

    /// <summary>
    /// If the player exits the switch's collider, then reset the switch's state and timer.
    /// </summary>
    protected override void LeftTriggerArea()
    {
        if (interactionComplete) photonView.RPC("Deactivate", RpcTarget.All);
        base.LeftTriggerArea();
    }

    [PunRPC]
    private void Deactivate()
    {
        Debug.Log("Switch deactivated");
        switchManager.SwitchDeactivated();
        interactionComplete = false;
        rendererToChangeMaterial.material = materialToChangeFrom;
        currInteractTime = 0.0f;
    }
}
