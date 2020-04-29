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

    [SerializeField] private GameObject marineControls = null;
    [SerializeField] private GameObject alienControls = null;

    // Retrieves all the available resolutions.
    private Resolution[] resolutions;

    // The file path where the settings file will be stored and loaded.
    private string settingsPath = "";

    private void Start()
    {
        settingsPath = Application.persistentDataPath + "/game_data.json";
        InitiateSettings();
    }

    /// <summary>
    /// Setups the dropdown menu options, and also sets the settings to the defaults.
    /// </summary>
    public void InitiateSettings()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        if (affectedCamera != null)
        {
            fovSlider.value = affectedCamera.fieldOfView;
        }

        if (audioMixer.GetFloat("volume", out float volValue))
        {
            volValue = mapLogarithmicToLinear(volValue, -80.0f, 20.0f, 0.001f, 1.0f);

            volumeSlider.value = volValue;
        }
        else
        {
            volumeSlider.value = 1.0f;
        }
        
        LoadResolutions(ref options);
    }

    private void LoadResolutions(ref List<string> options)
    {

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width
            && resolutions[i].height == Screen.height)
            {
                currentResIndex = i;
            }
        }

        SetDefaults(ref options, currentResIndex);
    }

    private void SetDefaults(ref List<string> options, int currentResIndex)
    {
        // Sets the default value of the dropdowns or toggles to the current settings.
        resolutionDropdown.AddOptions(options);
        Debug.Log("Current Res: " + resolutions[currentResIndex]);
        resolutionDropdown.SetValueWithoutNotify(currentResIndex);
        if (Screen.fullScreen)  fullscreenToggle.isOn = Screen.fullScreen;
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
    }
    
    /// <summary>
    /// Shows/hides a menu. It also saves the current settings.
    /// </summary>
    /// <param name="menu">The menu to toggle</param>
    public void ToggleOptionMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        settingsButtons.SetActive(!menu.activeSelf);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetVolume(float volume)
    {
        //Since audio is logarithmic and the slider is linear we have to convert it appropriately.
        float newSliderValue = mapLinearToLogarithmic(volumeSlider.value, 0.001f, 1.0f, -80.0f, 20.0f);

        audioMixer.SetFloat("volume", newSliderValue);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Debug.Log("Changing screen resolution to: " + res);
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        Debug.Log("Changing quality to: " + QualitySettings.names[qualityIndex]);
        QualitySettings.SetQualityLevel(qualityIndex);
        
    }

    public void SetFullscreenMode(bool isFullscreen)
    {
        Debug.Log("Changing fullscreen to: " + isFullscreen);
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        if (resolutions.Length > 0) resolutionDropdown.value = resolutions.Length - 1;
        Resolution res = resolutions[resolutions.Length - 1];
        // Forces fullscreen mode as it may get overriden by the SetResolution method
        Screen.SetResolution(res.width, res.height, isFullscreen);
    }

    public void SetFOV(float fov)
    {
        if (affectedCamera != null)
        {
            affectedCamera.fieldOfView = fov;
        }
    }

    public void ChangeControlsView(int sideToView) // 0 = marine, 1 = alien
    {
        bool displayMarineControls = sideToView == 0 ? true : false;
        marineControls.SetActive(displayMarineControls);
        alienControls.SetActive(!displayMarineControls);
    }

    // Remaps one set of values to another.
    // Edited from source: https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    float remapValues(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (((value - fromMin) / (fromMax - fromMin)) * (toMax - toMin)) + toMin;
    }

    float mapLinearToLogarithmic(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fraction = ((Mathf.Log10(value) - Mathf.Log10(fromMin)) / (Mathf.Log10(fromMax) - Mathf.Log10(fromMin)));
        
        return fraction * (toMax - toMin) + toMin;
    }

    float mapLogarithmicToLinear(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float numerator = (Mathf.Log10(toMax) - Mathf.Log10(toMin)) * (value - fromMin);
        float denominator = fromMax - fromMin;

        return Mathf.Pow(10, numerator / denominator) * toMin;
    }
}