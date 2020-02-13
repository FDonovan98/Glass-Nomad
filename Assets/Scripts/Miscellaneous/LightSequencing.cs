using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSequencing : MonoBehaviour
{
    public List<Light> lights = new List<Light>();
    public ParticleSystem particleSystem;
    public Color startingColour = Color.red;
    public Color endColour = Color.green;
    public float timeBetweenFlashes = 1.0f;
    public float timeUntilFlashingEnds = 10.0f;
    private Color targetColor;

    // Start is called before the first frame update
    void Start()
    {
        targetColor = endColour;
        ChangeLightColour();

        Invoke("EndRepeating", timeUntilFlashingEnds);
        InvokeRepeating("ChangeLightColour", timeBetweenFlashes, timeBetweenFlashes);
    }

    private void EndRepeating()
    {
        targetColor = endColour;
        foreach (Light element in lights)
        {
            element.color = targetColor;
        }

        CancelInvoke("ChangeLightColour");
    }

    private void ChangeLightColour()
    {
        targetColor = (targetColor == startingColour) ? endColour : startingColour;

        foreach (Light element in lights)
        {
            element.color = targetColor;
        }
    }

}
