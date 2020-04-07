using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    // Changes the video resolution.
    [SerializeField] private GameObject settingsButtons = null;

    // Changes the video resolution.
    [SerializeField] private TMP_Dropdown resolutionDropdown = null;

    // Changes the video quality.
    [SerializeField] private TMP_Dropdown qualityDropdown = null;

    // Changes the fullscreen mode.
    [SerializeField] private Toggle fullscreenToggle = null;

    // Changes the FOV of the camera.
    [SerializeField] private Slider fovSlider = null;

    // Changes the volume of the AudioListener.
    [SerializeField] private Slider volumeSlider = null;

    // Outputs all of the audio to the AudioMixer.
    [SerializeField] private AudioMixer audioMixer = null;

    // The camera that's used for FOV changes.
    [SerializeField] private Camera affectedCamera = null;

    // Retrieves all the available resolutions.
    private Resolution[] resolutions;

    // The file path where the settings file will be stored and loaded.
    private string settingsPath = "";

    private void Start()
    {
        settingsPath = Application.persistentDataPath + "/game_data";
        InitiateSettings();
    }

    /// <summary>
    /// Setups the dropdown menu options, and also sets the settings to the defaults.
    /// </summary>
    private void InitiateSettings()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        SaveLoadSettings.LoadData(settingsPath);
        if (affectedCamera != null)
        {
            fovSlider.value = affectedCamera.fieldOfView;
        }

        float volValue = 0f;
        if (audioMixer.GetFloat("volume", out volValue))
        {
            volumeSlider.value = volValue;
        }
        else
        {
            volumeSlider.value = 1f;
        }
        

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width
            && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        // Sets the default value of the dropdowns or toggles to the current settings.
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.SetValueWithoutNotify(currentResIndex);
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        fullscreenToggle.isOn = Screen.fullScreen;
    }
    
    /// <summary>
    /// Shows/hides a menu. It also saves the current settings.
    /// </summary>
    /// <param name="menu">The menu to toggle</param>
    public void ToggleOptionMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        settingsButtons.SetActive(!menu.activeSelf);
        SaveLoadSettings.SaveData(settingsPath, audioMixer);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetVolume(float volume)
    {
        //Since audio is logarithmic and the slider is linear we have to convert it appropriately.
        float volumeChangeValue = Mathf.Log10(volumeSlider.value) * 20;

        audioMixer.SetFloat("volume", volumeChangeValue);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Debug.Log("Changing screen resolution to: " + res);
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        Debug.Log("Changing quality to: " + qualityDropdown.captionText.text);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreenMode(bool isFullscreen)
    {
        Debug.Log("Changing fullscreen to: " + isFullscreen);
        Screen.fullScreen = isFullscreen;
    }

    public void SetFOV(float fov)
    {
        if (affectedCamera != null)
        {
            affectedCamera.fieldOfView = fov;
        }
    }
}