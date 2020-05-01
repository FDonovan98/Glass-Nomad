using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TerminalManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioSource fanAudioSource = null;
    [SerializeField] private AudioClip startUpSound = null;
    [SerializeField] private AudioClip logOffSound = null;
    [SerializeField] private AudioClip fanWhirlSound = null;
    [SerializeField] private AudioClip typingSound = null;
    [SerializeField] private AudioClip[] keyboardFoleySounds;
    [SerializeField] private AudioClip enterPressedSound = null;
    
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
    [SerializeField] private GameObject[] logTitles;
    [SerializeField] private GameObject[] logDescriptions;
    [SerializeField] private AudioClip[] logAudios;
    [SerializeField] private AudioClip logSwitchSound = null;
    [SerializeField] private Color selectedLogColor = Color.white;
    [SerializeField] private TMP_Text logIndex = null;
    private int currentLogIndex = 0;

    [Header("Other")]
    [SerializeField] private Animator scanLineAnimator;
    [SerializeField] private bool debug = false;
    private bool menuControlsEnabled = false;

    // Private Variables

    private async void Start()
    {
        PlayAudioClip(startUpSound);
        
        await Task.Delay(TimeSpan.FromSeconds(3f));

        if (fanWhirlSound != null)
        {
            fanAudioSource.clip = fanWhirlSound;
            fanAudioSource.Play();
        }

        ToggleElement(logInUI);

        if (scanLineAnimator != null)
        {
            scanLineAnimator.enabled = true;
        }

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

    private void PlayAudioClip(AudioClip audioClip, bool shouldStopCurrentAudio = false)
    {
        if (audioClip == null) return;
        if (shouldStopCurrentAudio) audioSource.Stop();
        audioSource.PlayOneShot(audioClip);
    }

    private void ToggleElement(GameObject elementToToggle)
    {
        elementToToggle.SetActive(!elementToToggle.activeInHierarchy);
    }

    private void OnKeypressed()
    {
        if (keyboardFoleySounds.Length == 0) return;
        AudioClip keyboardClip = keyboardFoleySounds[UnityEngine.Random.Range(0, keyboardFoleySounds.Length - 1)];
        if (keyboardClip != null) PlayAudioClip(keyboardClip);
    }

    private void TabPressed()
    {
        if (usernameField.isFocused) EventSystem.current.SetSelectedGameObject(passwordField.gameObject);
        else if (passwordField.isFocused) EventSystem.current.SetSelectedGameObject(usernameField.gameObject);
    }

    public void CheckUsernameField(string usernameText)
    {
        OnKeypressed();
        usernameCorrect = false;
        if (usernameText != correctUsername) return;
        if (debug) Debug.Log("Username correct.");
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
        if (debug) Debug.Log("Password correct.");
        passwordCorrect = true;

        if (usernameCorrect)
        {
            LogInSuccessful();
        }
    }

    public void EnterKeyPressed()
    {
        if (debug) Debug.Log("Enter");
        PlayAudioClip(enterPressedSound);
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
        ToggleLogSelect(GetNextLog(0));
        ToggleLogDescription(logDescriptions[currentLogIndex]);
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
        if (!menuControlsEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) TabPressed();
            if (Input.GetKeyDown(KeyCode.Return)) EnterKeyPressed(); 
            return;
        }

        if (Input.anyKeyDown) OnKeypressed();

        MoveLogSelection(GetArrowInput());

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnterKeyPressed();
            PlayAudioClip(logAudios[currentLogIndex], true);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) LogOff();
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
        
        PlayAudioClip(logSwitchSound);

        // Deselect the current log
        ToggleLogSelect(logTitles[currentLogIndex]);
        ToggleLogDescription(logDescriptions[currentLogIndex]);

        // Determine arrow movement
        if (arrowMovement == Vector2.up)
        {
            ToggleLogSelect(GetNextLog(-1));
        }
        else if (arrowMovement == Vector2.down)
        {
            ToggleLogSelect(GetNextLog(1));
        }

        if (debug) Debug.Log("Current log selected: " + logTitles[currentLogIndex].name);

        ToggleLogDescription(logDescriptions[currentLogIndex]);
        logIndex.text = (currentLogIndex + 1).ToString();
    }

    private GameObject GetNextLog(int dir)
    {
        currentLogIndex += dir;
        if (currentLogIndex < 0)
        {
            currentLogIndex = 0;
        }
        else if (currentLogIndex > logTitles.Length - 1)
        {
            currentLogIndex = logTitles.Length - 1;
        }

        return logTitles[currentLogIndex];
    }

    private void ToggleLogSelect(GameObject log)
    {
        if (log.GetComponent<Image>().color != selectedLogColor)
        {
            if (debug) Debug.Log("Selecting log: " + log.name);
            log.GetComponent<Image>().color = selectedLogColor;
            return;
        }
        if (debug) Debug.Log("Deselecting log: " + log.name);
        log.GetComponent<Image>().color = Color.clear;
    }

    private void ToggleLogDescription(GameObject log)
    {
        log.SetActive(!log.activeInHierarchy);
    }

    private async void LogOff()
    {
        ToggleElement(mainTerminalUI);
        PlayAudioClip(logOffSound);
        // Play CRT off animation?
        // Fade to black
        await Task.Delay(TimeSpan.FromSeconds(3f));
        SceneManager.LoadScene("SCN_Lobby");
    }

    // private async void TextScroll(string textToType, TMP_Text textElement, double timePerLetter = 0.1f, double timeToDisappear = 1f)
    // {
    //     string currText = "";
    //     foreach (Char letter in textToType.ToCharArray())
    //     {
    //         currText += letter;
    //         textElement.text = "<mark=#000000aa>" + currText + "</mark>";
    //         await Task.Delay(TimeSpan.FromSeconds(timePerLetter));
    //     }
    //     await Task.Delay(TimeSpan.FromSeconds(timeToDisappear));
    //     textElement.text = "";
    // }
}
