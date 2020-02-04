using UnityEngine;

abstract public class TriggerInteractionScript : MonoBehaviour
{
    [SerializeField] protected KeyCode inputKey = KeyCode.E;
    [SerializeField] protected float interactTime; // The time needed to interact with the object to activate/open it.
    protected float currInteractTime = 0f;
    [SerializeField] protected float cooldownTime;
    protected float currCooldownTime = 0f;
    protected bool interactionComplete = false;

    protected void Update()
    {
        if (currCooldownTime > 0)
        {
            currCooldownTime -= Time.deltaTime;
        }
    }

    protected void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Player" && currCooldownTime <= 0 && !interactionComplete)
        {
            if (Input.GetKey(inputKey))
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
            currInteractTime = 0f;
            return;
        }

        Debug.LogFormat("Cooldown: {0} seconds", currCooldownTime);
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

    abstract protected void InteractionComplete(GameObject player);
    virtual protected void LeftTriggerArea() { }
}
