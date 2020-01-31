using UnityEngine;

public class RedSwitchTriggerScript : MonoBehaviour
{
    // Tells the red switch manager when this switch has been (de)activated.
    [SerializeField] private RedSwitchManager switchManager = null;

    // The current time that the switch has been held for.
    private float currentTime = 0f;

    // The amount of time it takes for the switch to activate.
    private float timeToOpen = 5f;

    // Whether the switch is currently activated or not.
    private bool switchActivated = false;

    private void OnTriggerEnter()
    {
        // Resets the timer to activate the switch.
        currentTime = 0f;
    }

    /// <summary>
    /// Once the player enters the switch's collider and their holding 'E',
    /// the timer is started. If the player successfully holds the switch for
    /// the duration of the timer, then the switch is activated. If the player
    /// releases the switch, then it is deactivated and will need to be activated
    /// again.
    /// </summary>
    /// <param name="coll"></param>
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
            }
            else
            {
                // If the player is not pressing then reset the switch's state.
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

    /// <summary>
    /// If the player exits the switch's collider, then reset the 
    /// switch's state and timer.
    /// </summary>
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
