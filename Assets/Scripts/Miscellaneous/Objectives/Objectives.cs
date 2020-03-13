using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class Objectives : MonoBehaviour
{
    // The CSV file to read the objectives from.
    private string csvPath = "Assets/Scripts/Miscellaneous/Objectives/objectivesTextFile.csv";

    // A list of all of the objectives.
    private static List<Objective> objectives = new List<Objective>();
    
    // The time it takes to display each letter.
    private static float timePerLetter = 0.05f;

    // The time, after the whole text is diplayed, for it to vanish.
    private static float timeToDisappear = 1f;

    // The Text object which will display the text.
    [SerializeField] private TMP_Text objectiveText = null;

    // The static text object as it needs to be static.
    public static TMP_Text captionText = null;


    /// <summary>
    /// Reads all the data from the CSV file, and calls the 'START' objective.
    /// </summary>
    private void Start()
    {
        captionText = objectiveText;
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
                objectives.Add(new Objective(data[0], data[1], Resources.Load<AudioClip>("Audio/" + data[0].Replace(" ", "_"))));
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

        if (objectiveRequired.title == "" || (objectiveRequired.title != "ERROR" && objectiveRequired.completed == true))
        {
            if (objectiveCompleted.title != "ERROR" && objectiveCompleted.completed != true)
            {
                try
                {
                    if (Camera.allCameras[0].GetComponentInParent<MarineController>() == null) return;
                    Debug.Log(objectiveCompleted.dialogue);
                    objectiveCompleted.completed = true;
                    WriteTextToHud(objectiveCompleted.dialogue, timePerLetter, timeToDisappear);
                    PlaySpeechAudio(objectiveCompleted.speechAudio);
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

    public static bool IsObjectiveComplete(string objectiveName)
    {
        Objective obj = GetObjective(objectiveName);
        if ((obj.title != "" || obj.title != "ERROR") && obj.completed) return true;
        return false;
    }

    public static async void WriteTextToHud(string diag, float perLetter, float toDisappear = 0f)
    {
        string currText = "";
        foreach (Char letter in diag.ToCharArray())
        {
            currText += letter;
            captionText.text = "<mark=#000000aa>" + currText + "</mark>";
            await Task.Delay(TimeSpan.FromSeconds(perLetter));
        }
        await Task.Delay(TimeSpan.FromSeconds(toDisappear));
        captionText.text = "";
    }

    private static void PlaySpeechAudio(AudioClip speech)
    {
        Camera.allCameras[0].GetComponentInChildren<AudioSource>().PlayOneShot(speech);
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
    public AudioClip speechAudio;

    /// <summary>
    /// Constructor method of this class. Assigns the title and dialogue strings, and the completed
    /// boolean.
    /// </summary>
    /// <param name="ttl"></param>
    /// <param name="diag"></param>
    public Objective(string ttl, string diag, AudioClip audioClip = null)
    {
        title = ttl;
        dialogue = diag;
        speechAudio = audioClip;
        completed = false;
    }
}
