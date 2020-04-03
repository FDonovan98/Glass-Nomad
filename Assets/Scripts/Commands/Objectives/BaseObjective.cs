using UnityEngine;

public abstract class BaseObjective : ScriptableObject
{
    protected abstract void RunCommandOnTriggerEnter();
    protected abstract void RunCommandOnTriggerStay();
    protected abstract void RunCommandOnTriggerExit();
}