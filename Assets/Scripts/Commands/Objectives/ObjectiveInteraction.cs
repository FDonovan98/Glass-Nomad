using UnityEngine;

public abstract class ObjectiveInteraction : TriggerInteractionScript
{
    [SerializeField] private ObjectiveValues objectiveValues;

    protected override void InteractionComplete(GameObject player)
    {
        if (!objectiveValues.AllRequiredObjectivesCompleted())

        objectiveValues.completed = true;
        ObjectiveComplete();
    }

    protected abstract void ObjectiveComplete();
}