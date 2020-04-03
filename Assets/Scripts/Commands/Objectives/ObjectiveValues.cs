using UnityEngine;

[CreateAssetMenu(fileName = "DefaultObjectValues", menuName = "Commands/Objective Values")]
public class ObjectiveValues : ScriptableObject
{
    // The Objective's name will just be the name of the scriptable object.
    [SerializeField] private string objectiveText = "INSERT OBJECTIVE TEXT HERE";
    [SerializeField] private List<BaseObjective> requiredObjectives = new List<BaseObjective>();
    [SerializeField] private AudioClip objectiveAudio = null;
    private bool completed;
    // time per letter and time to disappear can be handled by the master objective handler.
}