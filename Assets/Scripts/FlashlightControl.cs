using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    private Light flashlight;
    
    // Start is called before the first frame update
    void Start()
    {
        flashlight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        //Press F to toggle flashlight on and off
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
