using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerInteractionScript : MonoBehaviourPunCallbacks
{
    [SerializeField] protected KeyCode inputKey = KeyCode.E; // Which key the player needs to be pressing to interact.
    [SerializeField] protected float interactTime; // The time needed to interact with the object to activate/open it.
    protected float currInteractTime = 0f; // How long the player has been pressing the interact key.
    [SerializeField] protected float cooldownTime; // How long it takes for the player to interact with the object again.
    protected float currCooldownTime = 0f; // How long it has been since the player last interacted with the object.
    protected bool interactionComplete = false; // Is the interaction complete?
    [SerializeField] protected bool debug = false; // Should the debug messages be displayed.
    protected Image outerReticle = null;
    private TMP_Text interactionText = null;
    [SerializeField] protected string objectiveName = "";
    [SerializeField] private string objectiveRequired = "";
    [SerializeField] private bool destroyObjectAfter = true;
    [SerializeField] private GameObject objectToDestroy = null;

    /// <summary>
    /// Constantly decreases the current cooldown time, unless its already 0.
    /// </summary>
    protected void Update()
    {
        if (currCooldownTime > 0)
        {
            currCooldownTime -= Time.deltaTime;
        }
    }

    protected void OnTriggerEnter(Collider coll)
    {
        // interactionText = coll.GetComponent<AgentController>().transform.GetChild(2).GetChild(1).GetChild(0).GetChild(2).GetComponent<TMP_Text>();
        // outerReticle = coll.GetComponent<AgentController>().transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// Checks it is a player interacting with the object, the cooldown has worn off, the interaction
    /// isn't already completed, and the player is pressing the interaction key. This adds to the 
    /// current interaction timer. If this is equal or greater than the required interaction time,
    /// then the interaction is marked as complete, the current interaction timer gets set to 0, the
    /// cooldown is reset and the 'InteractionComplete' method is called.
    /// 'InteractionComplete' is an abstract method, meaning that all sub classes of this class are
    /// required to have an implementation of this method.
    /// </summary>
    /// <param name="coll"></param>
    protected void OnTriggerStay(Collider coll)
    {
        if (interactionText != null) interactionText.text = "Press E to interact with <color=#2222ff>" + gameObject.name + "</color>";
        if (coll.tag == "Player" && currCooldownTime <= 0 && !interactionComplete)
        {
            if (Input.GetKey(inputKey) || inputKey == KeyCode.None)
            {
                if (currInteractTime >= interactTime)
                {
                    photonView.RPC("Completed", RpcTarget.All, coll.gameObject.GetPhotonView().ViewID);
                    currInteractTime = 0f;
                    interactionComplete = true;
                    currCooldownTime = cooldownTime;
                    coll.gameObject.GetComponent<AgentInputHandler>().allowInput = true;
                    return;
                }

                currInteractTime += Time.deltaTime;
                float percentage = (currInteractTime / interactTime) * 100;
                if (debug) Debug.LogFormat("Interaction progress: {0}%", percentage);

                if (outerReticle != null) ReticleProgress.UpdateReticleProgress(percentage, outerReticle);
                coll.gameObject.GetComponent<AgentInputHandler>().allowInput = false;
                return;
            }

            LeftTriggerArea(coll);
        }

        if (coll.tag == "Player" && interactionComplete)
        {
            coll.gameObject.GetComponent<AgentInputHandler>().allowInput = true;
        }

        if (debug) Debug.LogFormat("Cooldown: {0} seconds", currCooldownTime);
    }

    /// <summary>
    /// If this player leaves the trigger area, then the current interaction timer is set back to 0,
    /// the interaction is marked as incomplete and the 'LeftTriggerArea' method is called.
    /// 'LeftTriggerArea' is a virtual method meaning that sub classes can (but don't have to) have
    /// an implementation of this method.
    /// </summary>
    /// <param name="coll"></param>
    protected void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            interactionComplete = false;
            LeftTriggerArea(coll);
        }
    }

    [PunRPC]
    public void Completed(int pv)
    {
        GameObject player = PhotonView.Find(pv).gameObject;
        InteractionComplete(player);
    }

    virtual protected void InteractionComplete(GameObject player)
    {
        Objectives.ObjectiveComplete(objectiveName, objectiveRequired);
        
        // If the objective failed to be set as completed (e.g. a required objective hasn't been completed),
        // then don't do anything else.
        if (!Objectives.IsObjectiveComplete(objectiveName)) return;

        GetComponent<Collider>().enabled = false;
        if (destroyObjectAfter)
        {
            if (objectToDestroy == null) Destroy(gameObject);
            else Destroy(objectToDestroy);
        }
    }
    
    virtual protected void LeftTriggerArea(Collider coll)
    {
        currInteractTime = 0f;
        if (interactionText != null) interactionText.text = "";
        if (outerReticle != null) ReticleProgress.UpdateReticleProgress(0, outerReticle);
        coll.gameObject.GetComponent<AgentInputHandler>().allowInput = true;
        return;
    }
}
