using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSequencing : MonoBehaviour
{
    public enum SequenceType
    {
        Flash,
        Fade
    };

    public SequenceType sequenceType;
    public List<Light> lights = new List<Light>();
    public Color lightStartingColour = Color.red;
    public Color lightEndColour = Color.green;
    public float timeBetweenFlashes = 1.0f;
    public float timeUntilFlashingEnds = 10.0f;

    private Color targetColor;

    // Only used for Fade.
    public AnimationCurve intensity;

    public Material[] emmisiveMaterials;
    private Color materialStartColor;

    // Start is called before the first frame update
    void Start()
    {
        if (sequenceType == SequenceType.Flash)
        {
            targetColor = lightEndColour;

            if (lights.Count > 0)
            {
                ChangeLightColour();

                Invoke("EndRepeating", timeUntilFlashingEnds);
                InvokeRepeating("ChangeLightColour", timeBetweenFlashes, timeBetweenFlashes);            
            }
        }
        else if (sequenceType == SequenceType.Fade)
        {
            if (lights.Count > 0)
            {
                InvokeRepeating("ChangeLightIntensity", 0.0f, timeBetweenFlashes);
            }

            if (emmisiveMaterials.Length > 0)
            {
                materialStartColor = emmisiveMaterials[0].GetColor("_EmissionColor");
                InvokeRepeating("ChangeMaterialIntensity", 0.0f, timeBetweenFlashes);
            }
        }
    }

    private void ChangeMaterialIntensity()
    {
        float randTime = Random.Range(0.0f, 1.0f);

        foreach (Material element in emmisiveMaterials)
        {
            Debug.Log(element.color);
            element.SetColor("_EmissionColor", Color.Lerp(Color.black, materialStartColor, intensity.Evaluate(randTime)));
        }
    }

    private void ChangeLightIntensity()
    {
        float randTime = Random.Range(0.0f, 1.0f);

        foreach (Light element in lights)
        {
            element.intensity = intensity.Evaluate(randTime);
        }
    }

    private void EndRepeating()
    {
        targetColor = lightEndColour;
        foreach (Light element in lights)
        {
            element.color = targetColor;
        }

        CancelInvoke("ChangeLightColour");
    }

    private void ChangeLightColour()
    {
        targetColor = (targetColor == lightStartingColour) ? lightEndColour : lightStartingColour;

        foreach (Light element in lights)
        {
            element.color = targetColor;
        }
    }

}
