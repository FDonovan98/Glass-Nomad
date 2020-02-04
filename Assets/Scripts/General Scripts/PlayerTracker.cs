using UnityEngine;
using UnityEngine.UI;

public class PlayerTracker : MonoBehaviour
{
    [Tooltip("The distance that the tracker can see")]
    public float range;

    [Tooltip("The width that the tracker can see.")]
    public float width;

    [Tooltip("Which tags the tracker is able to detect.")]
    public string tagMask;

    public Transform canvas;
    public GameObject redDot;
    public float dotScaler = 3f;
    public float dotOpacity = 1f;
    private Camera cam;

    private void Start()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, width, range);
        boxCollider.center = new Vector3(0f, 0f, range / 2);
        cam = GetComponentInParent<Camera>();
        boxCollider.gameObject.SetActive(false);
        
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == tagMask)
        {
            GameObject dot = Instantiate(redDot, cam.WorldToScreenPoint(coll.gameObject.transform.position), Quaternion.identity, canvas);

            // Change the scale of the dot.
            float dist = Vector3.Distance(transform.parent.position, coll.gameObject.transform.position);
            dot.transform.localScale = new Vector3(1 / Mathf.Pow(dist, 1/dotScaler), 1 / Mathf.Pow(dist, 1/dotScaler), 1f);

            // Change the opacity of the dot - closer is more visible.
            dist = dist > dotOpacity ? dist : dotOpacity;
            Color temp = dot.GetComponent<Image>().color;
            temp.a = dotOpacity / dist;
            dot.GetComponent<Image>().color = temp;

            Destroy(dot, Time.fixedDeltaTime);
        }
    }
}
