using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [Tooltip("The distance that the tracker can see")]
    public float range;

    [Tooltip("The width that the tracker can see.")]
    public float width;

    [Tooltip("Which tags the tracker is able to detect.")]
    public string tagMask;

    private List<Vector3> playersFound = new List<Vector3>();

    private void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, 1f, range);
        boxCollider.center = new Vector3(0f, 0f, range / 2);
    }

    private void OnTriggerStay(Collider coll)
    {
        playersFound.Clear();
        if (coll.gameObject.tag == tagMask)
        {
            Debug.Log("Player found");
            playersFound = new List<Vector3>() { coll.gameObject.transform.position };
            // ---- Implement functionality here ---- (e.g. red dot on tracker
        }
    }

    private void OnTriggerExit() { playersFound.Clear(); }

    // This method is for debug purposes only. The gizmos are only seen in the editor.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 player in playersFound)
        {
            Gizmos.DrawSphere(player, 0.1f);
        }
    }
}
