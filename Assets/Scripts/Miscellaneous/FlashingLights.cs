using UnityEngine;

public class FlashingLights : MonoBehaviour
{
    private void Start()
    {
        Material newMat = GetComponent<Renderer>().material;
        newMat.SetColor("_EmissionColor", Color.blue);
        GetComponent<Renderer>().material = newMat;
    }
}