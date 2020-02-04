using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject linkedTeleporter = null; // The destination of the TP.
    [SerializeField] private float duration = 2f; // How long it takes to TP.
    [SerializeField] private bool biDirectional = true; // If false, then this TP can ONLY be used TO teleport, and NOT FROM.
    private float currDuration = 0f; // How long the TP has been activated for.
    // teleport cooldown?

    private void OnTriggerStay(Collider coll)
    {
        if (biDirectional)
        {
            if (coll.tag == "Player")
            {
                if (Input.GetKey(KeyCode.E))
                {
                    currDuration += Time.deltaTime;
                    Debug.LogFormat("Teleporter progress: {0}%", (currDuration / duration) * 100);
                    if (currDuration >= duration)
                    {
                        Debug.Log("teleporting player to: " + linkedTeleporter.transform.position);
                        coll.gameObject.transform.position = linkedTeleporter.transform.position;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        currDuration = 0f;
    }
}
