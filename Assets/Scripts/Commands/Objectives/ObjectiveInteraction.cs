using UnityEngine;

public abstract class ObjectiveInteraction : TriggerInteractionScript
{
    [SerializeField] private ObjectiveValues objectiveValues;
    
    // The time it takes to display each letter.
    private static float timePerLetter = 0.05f;

    // The time, after the whole text is diplayed, for it to vanish.
    private static float timeToDisappear = 1f;

    protected override void InteractionComplete(GameObject player)
    {
        if (!objectiveValues.AllRequiredObjectivesCompleted())

        objectiveValues.completed = true;
        ObjectiveComplete();
    }

    protected abstract void ObjectiveComplete();

    /// <summary>
    /// Gets the only active camera (which should be the local players) and players the audio clip
    /// passed through the parameters.
    /// </summary>
    /// <param name="speech"></param>
    private static void PlaySpeechAudio(AudioClip speech)
    {
        Camera.allCameras[0].GetComponentInChildren<AudioSource>().PlayOneShot(speech);
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
}