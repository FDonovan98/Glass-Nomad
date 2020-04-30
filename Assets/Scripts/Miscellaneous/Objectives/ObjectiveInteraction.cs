using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TriggerExtensionMethods;

public abstract class ObjectiveInteraction : TriggerInteractionScript
{
    [Header("Objective Interaction")]
    [SerializeField] protected ObjectiveValues objectiveValues; // The objective values that to use.
    [SerializeField] protected List<GameObject> objectsToDestroy = new List<GameObject>(); // A different object to destroy after the interaction is complete.
    [SerializeField] protected List<Behaviour> componentsToDisable = new List<Behaviour>(); // A different component to disable after the interaction is complete.

    protected TMP_Text captionText;
    protected TMP_Text hintText;

    // The time it takes to display each letter.
    protected const float timePerLetter = 0.05f;

    protected override void PopulateVariables()
    {
        base.PopulateVariables();

        try {
            if (playerInteracting.GetComponent<PhotonView>().IsMine)
            {
                RetrieveObjectiveUIElements();
            }
        }
        catch {
            Debug.LogError("Caption Text (for Objectives) has not been set correctly.");
        }
    }

    private void RetrieveObjectiveUIElements()
    {
        captionText = playerInteracting.transform.parent.GetChild(1).gameObject.FindComponentWithTag<TMP_Text>("Objective Prompt");
        hintText = playerInteracting.transform.parent.GetChild(1).gameObject.FindComponentWithTag<TMP_Text>("Objective Hint");
    }

    protected override void OnTriggerStay(Collider coll)
    {
        if (!coll.GetComponent<PhotonView>().IsMine) return;
        if (coll.GetComponent<AgentController>().agentValues.name != "MarineAgentValues") return;
        base.OnTriggerStay(coll);
    }

    [PunRPC]
    protected override void InteractionComplete()
    {
        Debug.Log("Interaction Complete: the alien should see this too!");     

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                playerInteracting = player;
            }
        }

        // Error catching
        if (playerInteracting == null) return;
        
        // If we haven't completed the correct objectives, yet, or we are the alien, then don't continue.
        if (!objectiveValues.AllRequiredObjectivesCompleted() || playerInteracting.layer != 8) return;
        
        Debug.LogFormat("Interaction Complete: {0}, all of the marines should receieve this.", objectiveValues.name);

        RetrieveObjectiveUIElements();
        objectiveValues.completed = true;
        ObjectiveComplete();
        WriteTextToHud();
        PlaySpeechAudio();
        
        foreach (Behaviour go in componentsToDisable)
        {
            Destroy(go);
        }

        foreach (GameObject go in objectsToDestroy)
        {
            Destroy(go);
        }

        playerInteracting = null;
    }

    protected virtual void ObjectiveComplete() {}

    /// <summary>
    /// Gets the only active camera (which should be the local players) and players the audio clip
    /// passed through the parameters.
    /// </summary>
    /// <param name="speech"></param>
    private void PlaySpeechAudio()
    {
        playerInteracting.GetComponentInChildren<Camera>().GetComponentInChildren<AudioSource>().PlayOneShot(objectiveValues.objectiveAudio);
    }

    /// <summary>
    /// Using Tasks (delays), it asynchronously writes text, letter by letter, to the HUD.
    /// After the text has finished writing, it waits a given amount of time until it disappears.
    /// </summary>
    /// <param name="diag">The text to display</param>
    /// <param name="perLetter">The time it takes to display each letter</param>
    /// <param name="toDisappear">The time it takes for the text to disappear, after it has finished
    /// displaying.</param>
    /// <returns>Nothing</returns>
    protected async void WriteTextToHud()
    {
        WriteHintTextToHUD(""); // Clears the hint text.
        string currText = "";
        foreach (Char letter in objectiveValues.objectiveText.ToCharArray())
        {
            currText += letter;
            captionText.text = "<mark=#000000aa>" + currText + "</mark>";
            await Task.Delay(TimeSpan.FromSeconds(timePerLetter));
        }
        await Task.Delay(TimeSpan.FromSeconds(objectiveValues.timeToDisappear));
        captionText.text = "";
        WriteHintTextToHUD(objectiveValues.objectiveHint);
    }

    /// <summary>
    /// Displays the text passed in the parameters to the hintText.
    /// Called by WriteTextToHUD.
    /// </summary>
    /// <param name="textToDisplay"></param>
    private void WriteHintTextToHUD(string textToDisplay)
    {
        hintText.text = textToDisplay;
    }
}