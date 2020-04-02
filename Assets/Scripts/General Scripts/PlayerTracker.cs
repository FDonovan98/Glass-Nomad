using UnityEngine;
using UnityEngine.UI;

public class PlayerTracker : MonoBehaviour
{
    // The distance that the tracker can see.
    public float range;

    // The width that the tracker can see.
    public float width;

    // Which tags the tracker is able to detect.
    public string tagMask;

    // Used to instantiate the red dot onto the canvas.
    public Transform canvas;

    // The prefab for the red dot.
    public GameObject redDot;

    // The maximum scale of the dot.
    public float dotScaler = 3f;

    // The maximum opacity of the dot.
    public float dotOpacity = 1f;

    // Used to translate the tracked player's position onto the canvas.
    private Camera cam;

    /// <summary>
    /// Alters the box collider's width, height and length to the range and width
    /// set in the inspector. Assigns the camera and deactivates the gameobject
    /// owner of this script.
    /// </summary>
    private void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, width, range);
        boxCollider.center = new Vector3(0f, 0f, range / 2);
        boxCollider.gameObject.SetActive(false);

        cam = GetComponentInParent<Camera>();
        this.gameObject.SetActive(false);
        canvas = GameObject.Find("EMP_UI").transform.GetChild(1).gameObject.transform;
    }

    /// <summary>
    /// When a player enters the collider, it is checked against the tag mask.
    /// If it is found in the tag mask, then a red dot is instantiated at the 
    /// player's position (in the canvas). This dot is then scaled and it's 
    /// visibility is altered in accordance with its distance away from the 
    /// player that is tracking.
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == tagMask)
        {
            GameObject dot = Instantiate(redDot, cam.WorldToScreenPoint(coll.gameObject.transform.position), Quaternion.identity, canvas);

            // Change the scale of the dot.
            float dist = Vector3.Distance(transform.parent.position, coll.gameObject.transform.position);
            dot.transform.localScale = new Vector3(1 / Mathf.Pow(dist, 1 / dotScaler), 1 / Mathf.Pow(dist, 1 / dotScaler), 1f);

            // Change the opacity of the dot - closer is more visible.
            dist = dist > dotOpacity ? dist : dotOpacity;
            Color temp = dot.GetComponent<Image>().color;
            temp.a = dotOpacity / dist;
            dot.GetComponent<Image>().color = temp;

            Destroy(dot, Time.fixedDeltaTime);
        }
    }
}