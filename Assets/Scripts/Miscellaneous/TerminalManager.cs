using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip fanWhirlSound = null;
    [SerializeField] private AudioClip typingSound = null;

    private void Start()
    {
        SpawnFadeFromBlack.Fade(Color.black, Color.clear, 3, this);
        // fan audio whirl
        // enable password ui
        // enable username
        // play audio typing game??
        // enable password typing
        // disable password ui
        // enable loading ui
        // loading dots disable enable
        // disable loading ui
        // enable main termianl menu
        // enable player navigation
        // player clicks audio log
        // key sound
        // audio log play
        // log off button press
        // fan whirl down
        // ctr screen off effect
        // load lobby
    }

    private async void TextScroll(string textToType, TMP_Text textElement, double timePerLetter = 0.1f, double timeToDisappear = 1f)
    {
        string currText = "";
        foreach (Char letter in textToType.ToCharArray())
        {
            currText += letter;
            textElement.text = "<mark=#000000aa>" + currText + "</mark>";
            await Task.Delay(TimeSpan.FromSeconds(timePerLetter));
        }
        await Task.Delay(TimeSpan.FromSeconds(timeToDisappear));
        textElement.text = "";
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void ToggleElement(GameObject elementToToggle)
    {
        elementToToggle.SetActive(!elementToToggle.activeInHierarchy);
    }
}
