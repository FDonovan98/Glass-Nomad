using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject linkedTeleporter = null; // The destination of the TP.
    [SerializeField] private float duration = 2f; // How long it takes to TP.
    [SerializeField] private float cooldown = 0f; // How long it takes before you can use the TP again.
    [SerializeField] private bool biDirectional = true; // If false, then this TP can ONLY be used TO teleport, and NOT FROM.
    private float currDuration = 0f; // How long the TP has been activated for.
    private float currCooldown = 0f; // How long it has been sinced you last used the TP.

    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (biDirectional && currCooldown <= 0)
                {
                    currDuration += Time.deltaTime;
                    Debug.LogFormat("Teleporter progress: {0}%", (currDuration / duration) * 100);
                    if (currDuration >= duration)
                    {
                        Debug.Log("teleporting player to: " + linkedTeleporter.transform.position);
                        coll.gameObject.transform.position = linkedTeleporter.transform.position;
                        currCooldown = cooldown;
                    }
                }

                Debug.LogFormat("Teleport can be used again in: {0} seconds", cooldown - currCooldown);
                return;
            }

            currCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        currDuration = 0f;
    }
}
