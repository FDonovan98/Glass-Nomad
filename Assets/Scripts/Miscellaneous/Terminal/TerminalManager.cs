using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioSource fanAudioSource = null;
    [SerializeField] private AudioClip startUpSound = null;
    [SerializeField] private AudioClip fanWhirlSound = null;
    [SerializeField] private AudioClip typingSound = null;
    [SerializeField] private AudioClip[] keyboardFoleySounds;

    [Header("UI Elements")]
    [SerializeField] private GameObject logInUI = null;
    [SerializeField] private GameObject loadingUI = null;
    [SerializeField] private GameObject mainTerminalUI = null;
    
    [Header("Username")]
    [SerializeField] private TMP_InputField usernameField = null;
    [SerializeField] private string correctUsername = null;
    private bool usernameCorrect = false;

    [Header("Password")]
    [SerializeField] private TMP_InputField passwordField = null;
    [SerializeField] private string correctPassword = null;
    private bool passwordCorrect = false;

    [Header("Logs")]
    [SerializeField] private GameObject[] terminalLogs;
    [SerializeField] private Color selectedLogColor = Color.white;
    private int currentLogIndex = 0;
    
    private bool menuControlsEnabled = false;

    // Private Variables

    private async void Start()
    {
        PlayAudioClip(startUpSound);
        await Task.Delay(TimeSpan.FromSeconds(3f));
        fanAudioSource.clip = fanWhirlSound;
        fanAudioSource.Play(); // ---> play constantly

        // PLAY SCAN LINE ANIMATIONS

        ToggleElement(logInUI);
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

    private void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip == null) return;
        audioSource.PlayOneShot(audioClip);
    }

    private void OnKeypressed()
    {
        if (keyboardFoleySounds.Length == 0) return;
        AudioClip keyboardClip = keyboardFoleySounds[UnityEngine.Random.Range(0, keyboardFoleySounds.Length - 1)];
        if (keyboardClip != null) PlayAudioClip(keyboardClip);
    }

    public void CheckUsernameField(string usernameText)
    {
        OnKeypressed();
        usernameCorrect = false;
        if (usernameText != correctUsername) return;
        Debug.Log("Username correct.");
        usernameCorrect = true;

        if (passwordCorrect)
        {
            LogInSuccessful();
        }
    }

    public void CheckPasswordField(string passwordText)
    {
        OnKeypressed();
        passwordCorrect = false;
        if (passwordText != correctPassword) return;
        Debug.Log("Password correct.");
        passwordCorrect = true;

        if (usernameCorrect)
        {
            LogInSuccessful();
        }
    }

    private async void LogInSuccessful()
    {
        // Disable Log In UI and enable the Loading UI
        await Task.Delay(TimeSpan.FromSeconds(1f));
        ToggleElement(logInUI);
        ToggleElement(loadingUI);
        CycleLoadingDots();
        await Task.Delay(TimeSpan.FromSeconds(4f));
        ToggleElement(loadingUI);
        ToggleElement(mainTerminalUI);
        menuControlsEnabled = true;
    }

    private async void CycleLoadingDots()
    {
        TMP_Text loadingText = loadingUI.GetComponentInChildren<TMP_Text>();
        for (int i = 0 ; i < 6; i++)
        {
            if (loadingText.text.Length > 10)
            {
                loadingText.text = "Loading.";
                await Task.Delay(TimeSpan.FromSeconds(0.5f));
                continue;
            }
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
            loadingText.text += ".";
        }
    }

    private void Update()
    {
        if (!menuControlsEnabled) return;

        MoveLogSelection(GetArrowInput());
    }

    private Vector3 GetArrowInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Vector2.down;
        }
        
        return Vector2.zero;
    }

    private void MoveLogSelection(Vector2 arrowMovement)
    {
        if (arrowMovement == Vector2.zero) return;

        // Deselect the current log
        ToggleLogSelect(terminalLogs[currentLogIndex]);
        
        // Determine arrow movement
        if (arrowMovement == Vector2.up)
        {
            ToggleLogSelect(GetNextLog(-1));
        }
        else if (arrowMovement == Vector2.down)
        {
            ToggleLogSelect(GetNextLog(1));
        }
    }

    private GameObject GetNextLog(int dir)
    {
        if (dir != 1 || dir != -1) return null;
        Debug.Log("Current Log Index: " + currentLogIndex);
        currentLogIndex = (currentLogIndex + dir) % terminalLogs.Length - 1;
        Debug.Log("New Log Index: " + currentLogIndex);
        return terminalLogs[currentLogIndex];
    }

    private void ToggleLogSelect(GameObject log)
    {
        if (log.GetComponentInChildren<Image>().color == Color.clear)
        {
            log.GetComponentInChildren<Image>().color = selectedLogColor;
            return;
        }
        log.GetComponentInChildren<Image>().color = Color.clear;
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

    private void ToggleElement(GameObject elementToToggle)
    {
        elementToToggle.SetActive(!elementToToggle.activeInHierarchy);
    }
}
