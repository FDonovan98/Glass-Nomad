using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    // The CSV file to read the objectives from.
    private string csvPath = "Assets/Scripts/Miscellaneous/objectivesTextFile.csv";

    // A list of all of the objectives.
    private static List<Objective> objectives = new List<Objective>();

    /// <summary>
    /// Reads all the data from the CSV file, and calls the 'START' objective.
    /// </summary>
    private void Start()
    {
        ReadData();
        ObjectiveComplete("START"); // Displays the starting text.
    }

    /// <summary>
    /// Loops through every line in the CSV file, finds the first comma, and splits it into 
    /// a string array. The first item in the array is the objective's name/title, and the 
    /// second item is the objective's dialogue/description. These objectives are then added
    /// to the 'objectives' list.
    /// </summary>
    private void ReadData()
    {
        using (StreamReader reader = new StreamReader(csvPath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] data = line.Split(new[] { ',' }, 2);
                objectives.Add(new Objective(data[0], data[1]));
            }
        }
    }

    /// <summary>
    /// Static method, so it can be called by other scripts, without needing to hold a reference
    /// to it. Attempts to find the objective name passed through the parameters. If an objective
    /// with the same title is found, then the 'Say' method is called.
    /// </summary>
    /// <param name="objName">The objective's title that has been completed.</param>
    /// <param name="requiredObjective">The title of any objective(s) that need
    /// to be completed before this objective can be completed.</param>
    public static void ObjectiveComplete(string objName, string requiredObjective = "")
    {
        Objective objectiveCompleted = GetObjective(objName);
        Objective objectiveRequired = GetObjective(requiredObjective);

        if (objectiveRequired.title != "ERROR" && objectiveRequired.completed == true)
        {
            if (objectiveCompleted.title != "ERROR" && objectiveCompleted.completed != true)
            {
                try
                {
                    objectiveCompleted.Say();
                }
                catch (Exception e)
                {
                    Debug.LogError("ERROR: Could not complete objective because it errored: \n" + e);
                }
                return;
            }
            Debug.Log("Objective already completed or error has occured.");
            return;
        }
        Debug.LogFormat("Objective '{0}' has not been completed. This objective is required before " +
            "completing the '{1}' objective", objectiveRequired.title, objectiveCompleted.title);
    }

    private static Objective GetObjective(string objName)
    {
        if (objName == "")
        {
            return new Objective("", "");
        }

        foreach (Objective obj in objectives)
        {
            if (obj.title == objName)
            {
                return obj;
            }
        }

        return new Objective("ERROR", "");
    }
}

/// <summary>
/// A small class that holds the title, dialogue and boolean of whether the objective has been
/// completed or not.
/// </summary>
public class Objective
{
    public string title;
    public string dialogue;
    public bool completed;

    /// <summary>
    /// Constructor method of this class. Assigns the title and dialogue strings, and the completed
    /// boolean.
    /// </summary>
    /// <param name="ttl"></param>
    /// <param name="diag"></param>
    public Objective(string ttl, string diag)
    {
        title = ttl;
        dialogue = diag;
        completed = false;
    }

    /// <summary>
    /// Called when an objective is completed.
    /// Displays the dialogue of the objective and marks it as completed.
    /// </summary>
    public void Say()
    {
        Debug.Log(dialogue);
        completed = true;
    }
}
