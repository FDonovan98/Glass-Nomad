using UnityEngine;

public abstract class BaseObjective : ScriptableObject
{
    [SerializeField] private ObjectiveValues objectiveValues;

    public abstract void RunCommandOnStart(ObjectiveHandler objectiveHandler);
    protected abstract void RunCommandOnTriggerEnter();
    protected abstract void RunCommandOnTriggerStay();
    protected abstract void RunCommandOnTriggerExit();

    protected virtual void RunCommandOnCompleted()
    {
        if (!objectiveValues.AllRequiredObjectivesCompleted()) return;
        objectiveValues.completed = true;
    }
}