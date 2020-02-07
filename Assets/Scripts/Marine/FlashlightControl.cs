using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    private Light flashlight;
    
    private void Start()
    {
        flashlight = GetComponent<Light>();
    }

    private void Update()
    {
        //Press F to toggle flashlight on and off
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
