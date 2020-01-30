using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Used to change scene when we leave a room.
    [SerializeField] private string lobbySceneName = "SCN_Lobby";
    
    // Used to spawn the alien and marines.
    [SerializeField] private GameObject alienSpawnPoint = null;
    [SerializeField] private GameObject marineSpawnPoint = null;

    // Used to change the video resolution.
    [SerializeField] private TMP_Dropdown resolutionDropdown = null;

    // Used to change the video quality.
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    
    // Used by PlayerMovement to access the pause menu gameobject.
    public GameObject pauseMenu;

    // Used to retrieve all the available resolutions.
    private Resolution[] resolutions;
    
    // Used to change the FOV of the camera.
    private Camera cam;

    /// <summary>
    /// Spawns the alien and marine prefabs.
    /// Sets up the resolution dropdown settings in the pause menu, and assigns the camera.
    /// Resolution code referenced from: https://www.youtube.com/watch?v=YOaYQrN1oYQ
    /// </summary>
    private void Start()
    {
        // Spawns a Alien prefab if the player is the master client, otherwise it spawns a Marine prefab.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Alien (Cylinder)", alienSpawnPoint.transform.position, new Quaternion());
        }
        else
        {
            PhotonNetwork.Instantiate("Marine (Cylinder)", marineSpawnPoint.transform.position, new Quaternion());
        }

        // Setting up the resolution options
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

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

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
        Debug.LogFormat("{0} entered the game room", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("You are the master client");
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("{0} left the game room", other.NickName);
    }

    /// <summary>
    /// Changes the model of the new master client, when the old one leaves.
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
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

    #region menu_options
    
    public void ToggleOptionMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
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
        cam.fieldOfView = fov;
    }

    #endregion
}
