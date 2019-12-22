using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

using System;

public class Objectives : EditorWindow
{    
    int chosenTrigger = 0;
    Button button;

    // enum TriggerOptions is used to refrence string[] triggerOptions.
    // As such, the order of the options in each needs to be the same.
    enum TriggerOptions
    {
        Button,
        Bool,
        Areas
    }
    string[] triggerOptions = 
    {
        "On Button Press",
        "On Bool is True",
        "On Area Entered"
    };

    [MenuItem("Window/Dev Tools/Objective Creation")]
    public static void ShowWindow()
    {
        GetWindow<Objectives>("Objective Creation");
    }

    void OnGUI()
    {
        Triggers();
        Effects();
    }

    // Renders menu items handling the selection of triggers.
    void Triggers()
    {
        if(EditorGUILayout.DropdownButton(new GUIContent(triggerOptions[chosenTrigger]), new FocusType()))
        {
            GenericMenu menu = new GenericMenu();

            // Loops through every element in TriggerOptions.
            foreach (TriggerOptions element in (TriggerOptions[]) Enum.GetValues(typeof(TriggerOptions)))
            {
                AddTriggerOptionToMenu(menu, element);
            }

            menu.ShowAsContext();
        }

        switch (chosenTrigger)
        {
            case 0:
                button = (Button)EditorGUILayout.ObjectField("Trigger Button", button, typeof(Button), true);
                return;

            default:
                return;
        }
    }

    // Function to make adding items cleaner.
    // Uses TriggerOptions value to select the right string from triggerOptions.
    void AddTriggerOptionToMenu(GenericMenu menu, TriggerOptions trigger)
    {
        menu.AddItem(new GUIContent(triggerOptions[(int)trigger]), chosenTrigger.Equals(trigger), setTriggerOptions, trigger);
    }

    void setTriggerOptions(object choice)
    {
        chosenTrigger = (int)choice;
    }

    // Renders menu items handling the effect of the trigger.
    void Effects()
    {

    }
}
