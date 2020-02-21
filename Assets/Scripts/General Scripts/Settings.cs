using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Settings
{
    public FullScreenMode fullscreen = FullScreenMode.Windowed;
    public int[] resolution = new int[2] { Screen.width, Screen.height };
    //public Resolution resolution = Screen.currentResolution; // This isn't saved.
    public int quality = QualitySettings.GetQualityLevel();
    public float fieldOfView = Camera.main.fieldOfView;
    public float volume = AudioListener.volume;

    public void UpdateSettings(FullScreenMode fs, int[] res, int qual, float fov)
    {
        Screen.SetResolution(res[0], res[1], fs);
        QualitySettings.SetQualityLevel(qual);
        Camera.main.fieldOfView = fov;
    }
}

public static class SaveLoadSettings
{
    private static Settings settings;

    public static void SaveData(string filePath)
    {
        Debug.Log("Saving settings...");
        settings = new Settings();
        string jsonData = JsonUtility.ToJson(settings, true);
        File.WriteAllText(filePath, jsonData);
    }

    public static void LoadData(string filePath)
    {
        Debug.Log("Loading settings...");
        try
        {
            settings = JsonUtility.FromJson<Settings>(File.ReadAllText(filePath));
            settings.UpdateSettings(settings.fullscreen, settings.resolution, settings.quality, settings.fieldOfView);
            Debug.Log("Settings loaded.");
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("File doesn't currently exist.");
        }
    }
}
