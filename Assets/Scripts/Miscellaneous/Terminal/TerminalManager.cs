using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip fanWhirlSound = null;
    [SerializeField] private AudioClip typingSound = null;

    [Header("UI Elements")]
    [SerializeField] private GameObject logInUI = null;
    [SerializeField] private GameObject mainTerminalUI = null;
    
    [Header("Username")]
    [SerializeField] private TMP_InputField usernameField = null;
    [SerializeField] private string correctUsername = null;

    [Header("Password")]
    [SerializeField] private TMP_InputField passwordField = null;
    [SerializeField] private string correctPassword = null;

    [Header("Logs")]
    [SerializeField] private TerminalLog[] terminalLogs;
    private TerminalLog currentLog = null;

    // Private Variables

    private void Start()
    {
        // fade in from black
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

    public void CheckUsernameField(string usernameText)
    {
        if (usernameText != correctUsername) return;
        Debug.Log("Username correct.");
        // UsernameCorrect();
    }

    public void CheckPasswordField(string passwordText)
    {
        if (passwordText != correctPassword) return;
        Debug.Log("Password correct.");
        // PasswordCorrect();
    }
}
