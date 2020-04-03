using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    public delegate void RunCommandOnTriggerEnter(Collider coll);
    public RunCommandOnTriggerEnter runCommandOnTriggerEnter;
    public delegate void RunCommandOnTriggerStay(Collider coll);
    public RunCommandOnTriggerStay runCommandOnTriggerStay;
    public delegate void RunCommandOnTriggerExit(Collider coll);
    public RunCommandOnTriggerExit runCommandOnTriggerExit;
    public delegate void RunCommandOnCompleted();
    public RunCommandOnCompleted runCommandOnCompleted;

    private void OnTriggerEnter(Collider coll)
    {
        if (runCommandOnTriggerEnter != null)
        {
            runCommandOnTriggerEnter(coll);
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if (runCommandOnTriggerStay != null)
        {
            runCommandOnTriggerStay(coll);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (runCommandOnTriggerExit != null)
        {
            runCommandOnTriggerExit(coll);
        }
    }
}