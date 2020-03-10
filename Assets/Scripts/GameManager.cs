using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Changes to this scene when we leave a room.
    [SerializeField] private string lobbySceneName = "SCN_Lobby";

    // Spawn points for the alien and marines.
    [SerializeField] private GameObject alienSpawnPoint = null;
    [SerializeField] private GameObject marineSpawnPoint = null;

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

    // Used by PlayerMovement to access the pause menu gameobject.
    public GameObject pauseMenu;

    // Retrieves all the available resolutions.
    private Resolution[] resolutions;

    // Should we switch a marine to the alien, when the alien dies.
    public bool switchToAlien = false;

    private string settingsPath;

    private void Start()
    {
        if (!PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
        {
            SpawnLocalPlayer();
        }
        else
        {
            Debug.Log("Spawning an alien, hopefully.");
            Instantiate(Resources.Load("Alien (Cylinder)", typeof(GameObject)), alienSpawnPoint.transform.position, new Quaternion());
        }

        settingsPath = Application.persistentDataPath + "/game_data";
        SetupResolutionDropdown();
    }

    private void SpawnLocalPlayer()
    {
        PlayersInLobby lobbyRoom = GameObject.Find("Lobby").GetComponent<PlayersInLobby>();
        if (lobbyRoom.IsPlayerAlien(PhotonNetwork.NickName))
        {
            PhotonNetwork.Instantiate("Alien (Cylinder)", alienSpawnPoint.transform.position, new Quaternion());
        }
        else
        {
            PhotonNetwork.Instantiate("Marine (Cylinder)", GetRandomSpawnPoint(), new Quaternion());
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 pos = marineSpawnPoint.transform.position;
        // Vector2 circle = UnityEngine.Random.insideUnitCircle * 3;
        // pos.x += circle.x;
        // pos.z += circle.y;
        Debug.Log(pos);
        return pos;
    }

    private void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        SaveLoadSettings.LoadData(settingsPath);
        fovSlider.value = Camera.main.fieldOfView;
        volumeSlider.value = AudioListener.volume;

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

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(lobbySceneName);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("{0} entered the game room", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("You are the master client");
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("{0} left the game room", other.NickName); // seen when other disconnects
    }

    /// <summary>
    /// Changes the model of the new master client, when the old one leaves.
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (switchToAlien)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("CHANGING MODEL");

                GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject element in playerObjects)
                {
                    if (element.GetComponent<PhotonView>().IsMine)
                    {
                        Vector3 playerPos = alienSpawnPoint.transform.position;
                        Quaternion playerRot = element.transform.rotation;
                        string prefabName;

                        if (element.GetComponent<AlienController>() != null)
                        {
                            prefabName = "Marine (Cylinder)";
                        }
                        else
                        {
                            prefabName = "Alien (Cylinder)";
                        }

                        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                        PhotonNetwork.Instantiate(prefabName, playerPos, playerRot);

                        return;
                    }
                }
            }
        }
    }

#region menu_options
    public void ToggleOptionMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        SaveLoadSettings.SaveData(settingsPath);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
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
        Camera.main.fieldOfView = fov;
    }

#endregion
}
