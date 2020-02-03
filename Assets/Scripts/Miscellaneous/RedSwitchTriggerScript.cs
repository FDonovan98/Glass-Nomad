using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSwitchTriggerScript : MonoBehaviour
{
    public RedSwitchManager switchManager = null;
    private float currentTime = 0f;
    private float timeToOpen = 5f;
    private bool switchActivated = false;

    private void OnTriggerEnter()
    {
        currentTime = 0f;
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!switchActivated)
                {
                    currentTime += Time.deltaTime;
                    Debug.Log("Switch progress: " + (currentTime / timeToOpen) * 100 + "%");

                    if (currentTime >= timeToOpen)
                    {
                        Debug.Log("Switch activated");
                        switchActivated = true;
                        switchManager.SwitchActivated();
                    }
                }
            } else // if the player is not pressing then reset the switch's state.
            {
                currentTime = 0f;
                if (switchActivated)
                {
                    Debug.Log("Switch deactivated");
                    switchActivated = false;
                    switchManager.SwitchDeactivated();
                }
            }
        }
    }

    private void OnTriggerExit()
    {
        currentTime = 0f;
        if (switchActivated)
        {
            Debug.Log("Switch deactivated");
            switchActivated = false;
            switchManager.SwitchDeactivated();
        }
    }
}
