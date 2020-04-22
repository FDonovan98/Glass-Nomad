using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefaultObjectValues", menuName = "Commands/Objective Values")]
public class ObjectiveValues : ScriptableObject
{
    // The main objective text to display
    public string objectiveText = "INSERT OBJECTIVE TEXT HERE";
    
    // The shortened text to display permanently
    public string objectiveHint = "SMALLER TEXT AT THE TOP";

    // The objective(s) that need to be completed before this one.
    public List<ObjectiveValues> requiredObjectives = new List<ObjectiveValues>();

    // The sound/speech to play once the objective has been completed.
    public AudioClip objectiveAudio = null;

    // Has the objective been completed
    public bool completed;

    // The time, after the whole text is diplayed, for it to vanish.
    public float timeToDisappear = 1f;

    public bool AllRequiredObjectivesCompleted()
    {
        foreach (ObjectiveValues obj in requiredObjectives)
        {
            if (!obj.completed) return false;
        }
        return true;
    }
}