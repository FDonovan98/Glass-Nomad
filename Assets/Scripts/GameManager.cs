using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    [SerializeField] private string lobbySceneName = "SCN_Lobby"; // Used to change scene when we leave a room.
    [SerializeField] private Vector3 alienSpawnPoint = Vector3.zero; // Used to spawn the alien.
    [SerializeField] private Vector3 marineSpawnPoint = Vector3.zero; // Used to spawn the marines.
    [SerializeField] private Dropdown resolutionDropdown = null; // Used to change the video resolution.
    [SerializeField] private Dropdown qualityDropdown = null; // Used to change the video quality.
    [SerializeField] private AudioMixer audioMixer = null; // Used to change the audio volume.
    public GameObject pauseMenu; // Used by PlayerMovement to access the pause menu gameobject.
    private Resolution[] resolutions; // Used to retrieve all the available resolutions.

    #region devtools
    [Header("Developer Tools")]
    [SerializeField] private bool singlePlayerMarine = false; // Used to test the marine player, in testing.
    #endregion

    private void Start()
    {
        // Dev tool
        if (singlePlayerMarine)
        {
            PhotonNetwork.Instantiate("Marine (Cylinder)", marineSpawnPoint, new Quaternion());
            return;
        }

        // Spawns a Alien prefab if the player is the master client, otherwise it spawns a Marine prefab.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Alien (Cylinder)", alienSpawnPoint, new Quaternion());
        }
        else
        {
            PhotonNetwork.Instantiate("Marine (Cylinder)", marineSpawnPoint, new Quaternion());
        }
        
        Debug.Log(PhotonNetwork.CountOfPlayers.ToString() + " player(s) in game");

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
                    Vector3 playerPos = alienSpawnPoint;
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
        audioMixer.SetFloat("volume", volume);
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

    #endregion
}
