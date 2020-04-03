using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefaultObjectValues", menuName = "Commands/Objective Values")]
public class ObjectiveValues : ScriptableObject
{
    // The Objective's name will just be the name of the scriptable object.
    public string objectiveText = "INSERT OBJECTIVE TEXT HERE";
    public List<ObjectiveValues> requiredObjectives = new List<ObjectiveValues>();
    public AudioClip objectiveAudio = null;
    public bool completed;
    // time per letter and time to disappear can be handled by the master objective handler.

    public bool AllRequiredObjectivesCompleted()
    {
        foreach (ObjectiveValues obj in requiredObjectives)
        {
            if (!obj.completed) return false;
        }
        return true;
    }
}