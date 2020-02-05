using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    private string csvPath = "Assets/Scripts/Miscellaneous/objectivesTextFile.csv";
    private static List<Objective> objectives = new List<Objective>();

    private void Start()
    {
        ReadData();
        ObjectiveComplete("START");
    }

    private void ReadData()
    {
        using (StreamReader reader = new StreamReader(csvPath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] data = line.Split(',');
                objectives.Add(new Objective(data[0], data[1]));
            }
        }
    }

    public static void ObjectiveComplete(string objName)
    {
        Objective objectiveCompleted = new Objective("", "");
        foreach (Objective obj in objectives) { if (obj.title == objName) { objectiveCompleted = obj; } }
        if (objectiveCompleted.title != "" && objectiveCompleted.completed != true)
        {
            objectiveCompleted.Say();
        }
    }
}

public class Objective
{
    public string title;
    public string dialogue;
    public bool completed;

    public Objective(string ttl, string diag)
    {
        title = ttl;
        dialogue = diag;
        completed = false;
    }

    public void Say()
    {
        Debug.Log(dialogue);
        completed = true;
    }
}
