using UnityEngine;

abstract public class TriggerInteractionScript : MonoBehaviour
{
    public KeyCode inputKey;
    public float interactTime; // The time needed to interact with the object to activate/open it.
    private float currInteractTime = 0f;
    public float cooldownTime;
    private float currCooldownTime = 0f;
    private bool interactionComplete = false;

    protected void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player" && currCooldownTime <= 0 && !interactionComplete)
        {
            if (Input.GetKey(inputKey))
            {
                if (currInteractTime >= interactTime)
                {
                    InteractionComplete();
                    currInteractTime = 0f;
                    interactionComplete = true;
                }

                currInteractTime += Time.deltaTime;
                Debug.LogFormat("Interaction progress: {0}%", (currInteractTime / interactTime) * 100);
            }

            Debug.LogFormat("Cooldown: {0} seconds", cooldownTime - currCooldownTime);
            return;
        }

        currCooldownTime -= Time.deltaTime;
    }

    protected void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            currInteractTime = 0f;
            interactionComplete = false;
            LeftTriggerArea();
        }
    }

    abstract protected void InteractionComplete();
    virtual protected void LeftTriggerArea() { }
}
