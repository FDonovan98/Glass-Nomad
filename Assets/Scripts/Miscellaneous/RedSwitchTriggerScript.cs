using UnityEngine;

public class RedSwitchTriggerScript : TriggerInteractionScript
{

    // Tells the red switch manager when this switch has been (de)activated.
    public RedSwitchManager switchManager = null;

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
        if (coll.gameObject.tag == "Player" && currCooldownTime <= 0)
        {
            if (Input.GetKey(inputKey))
            {
                if (!interactionComplete)
                {
                    if (currInteractTime >= interactTime)
                    {
                        InteractionComplete(coll.gameObject);
                        currInteractTime = 0f;
                        interactionComplete = true;
                        currCooldownTime = cooldownTime;
                    }

                    currInteractTime += Time.deltaTime;
                    if (debug) Debug.LogFormat("Interaction progress: {0}%", (currInteractTime / interactTime) * 100);
                }
            }
            else // if the player is not pressing then reset the switch's state.
            {
                currInteractTime = 0f;
                LeftTriggerArea();
            }
        }
    }

    protected override void InteractionComplete(GameObject player)
    {
        if (debug) Debug.Log("Switch activated");
        interactionComplete = true;
        switchManager.SwitchActivated();
    }

    /// <summary>
    /// If the player exits the switch's collider, then reset the switch's state and timer.
    /// </summary>
    protected override void LeftTriggerArea()
    {
        if (interactionComplete)
        {
            if (debug) Debug.Log("Switch deactivated");
            switchManager.SwitchDeactivated();
            interactionComplete = false;
        }
        currInteractTime = 0f;
    }
}
