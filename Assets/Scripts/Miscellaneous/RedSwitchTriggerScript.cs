using UnityEngine;

public class RedSwitchTriggerScript : TriggerInteractionScript
{
    public RedSwitchManager switchManager = null;

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
                    Debug.LogFormat("Interaction progress: {0}%", (currInteractTime / interactTime) * 100);
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
        Debug.Log("Switch activated");
        interactionComplete = true;
        switchManager.SwitchActivated();
    }

    protected override void LeftTriggerArea()
    {
        if (interactionComplete)
        {
            Debug.Log("Switch deactivated");
            switchManager.SwitchDeactivated();
            interactionComplete = false;
        }
    }
}
