using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Photon.Pun;

public abstract class ObjectiveInteraction : TriggerInteractionScript
{
    [SerializeField] private ObjectiveValues objectiveValues;

    private TMP_Text captionText;

    // The time it takes to display each letter.
    private const float timePerLetter = 0.05f;

    protected override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);

        try {
            if (photonView.IsMine) captionText = coll.GetComponent<AgentController>().transform.GetChild(2).GetChild(1).GetChild(0).GetChild(2).GetComponent<TMP_Text>();
        }
        catch {
            Debug.LogError("Caption Text (for Objectives) has not been set correctly.");
        }
    }

    protected override void InteractionComplete(GameObject player)
    {
        if (!objectiveValues.AllRequiredObjectivesCompleted())

        objectiveValues.completed = true;
        ObjectiveComplete();
        WriteTextToHud();
    }

    protected abstract void ObjectiveComplete();

    /// <summary>
    /// Gets the only active camera (which should be the local players) and players the audio clip
    /// passed through the parameters.
    /// </summary>
    /// <param name="speech"></param>
    private void PlaySpeechAudio()
    {
        Camera.allCameras[0].GetComponentInChildren<AudioSource>().PlayOneShot(objectiveValues.objectiveAudio);
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
    public async void WriteTextToHud()
    {
        string currText = "";
        foreach (Char letter in objectiveValues.objectiveText.ToCharArray())
        {
            currText += letter;
            captionText.text = "<mark=#000000aa>" + currText + "</mark>";
            await Task.Delay(TimeSpan.FromSeconds(perLetter));
        }
        await Task.Delay(TimeSpan.FromSeconds(objectiveValues.timeToDisappear));
        captionText.text = "";
    }
}