using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSequencing : MonoBehaviour
{
    public List<Light> lights = new List<Light>();
    public Color startingColour = Color.red;
    public Color endColour = Color.green;
    public static float timeOnStartColour = 10.0f;

    private WaitForSeconds timeDelay;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        timeDelay = new WaitForSeconds(0.0f);
        ChangeLightColour(startingColour, timeDelay);

        timeDelay = new WaitForSeconds(timeOnStartColour);
        coroutine = ChangeLightColour(endColour, timeDelay);
        
        StartCoroutine(coroutine);
    }

    private IEnumerator ChangeLightColour(Color targetColor, WaitForSeconds delay)
    {
        yield return delay;

        foreach (Light element in lights)
        {
            element.color = targetColor;
        }
    }

}
