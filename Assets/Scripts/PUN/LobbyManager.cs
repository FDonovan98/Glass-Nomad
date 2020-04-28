using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region variable-declarations

    [SerializeField] private string gameScene = null; // Changes scene when we are join a room.
    [SerializeField] private GameObject playerItemPrefab = null; // Displays the players in the lobby.
    [SerializeField] private GameObject inLobbyPanel = null; // Displays the lobby buttons when you join a room.
    [SerializeField] private Transform playerListPanel = null; // Contains all the playeritem prefabs.
    [SerializeField] private Image screenFader = null; // Fades the screen to black, when entering the game.

    private const string playerNamePrefKey = "Player Name";
    private byte maxPlayersPerRoom = 5; // Sets a limit to the number of players in a room.
    private string gameVersion = "1"; // Separates users from each other by gameVersion.
    private bool isConnection = false; // Stop us from immediately joining the room if we leave it.

    [SerializeField] private GameObject menuContainer = null;
    [SerializeField] private GameObject loadoutContainer = null;
    [SerializeField] private GameObject lobbyContainer = null;
    [SerializeField] private GameObject settingsContainer = null;

    [SerializeField] private Color playerAlienButtonColor = Color.clear;
    [SerializeField] private Color playerMarineButtonColor = Color.clear;
    
    public PlayersInLobby lobbyRoom = null;

    #endregion

    private void Awake()
    {
        // Means we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        menuContainer.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SaveLoadSettings.LoadData(Application.persistentDataPath + "/game_data");
    }
    
    public void OnPlayClick()
    {
        // Hide initial main menu items (title & control panel)
        // Show username input, loadout options and join lobby button
        menuContainer.SetActive(false);
        loadoutContainer.SetActive(true);
    }

    public void Connect()
    {
        // Checks the player's input is empty - returns if it is.
        if (PlayerPrefs.GetString(playerNamePrefKey) == "")
        {
            return;
        }

        // Switches which UI elements are visable.
        loadoutContainer.SetActive(false);
        lobbyContainer.SetActive(true);

        // The button has been pressed so we want the user to connect to a room.
        isConnection = true;

        // Checks if the client is aleady connected
        if (PhotonNetwork.IsConnected)
        {
            // Attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            Debug.Log("Connected, joining a random room");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // Otherwise, connect to Photon Online Server.
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");

        if (isConnection)
        {
            // Try and join a potential existing room, else, we'll be called back with OnJoinRandomFailed().
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected with reason:" + cause);
        menuContainer.SetActive(true);
        loadoutContainer.SetActive(false);
        lobbyContainer.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // We failed to join a random room, maybe none exists or they are all full. So we create a new room.
        Debug.Log("No room found, creating new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined successfully");
        menuContainer.SetActive(false);
        loadoutContainer.SetActive(false);
        lobbyContainer.SetActive(true);

        if (!PhotonNetwork.IsMasterClient)
        {
            inLobbyPanel.transform.GetChild(0).gameObject.SetActive(false);
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                lobbyRoom.PlayerJoinedLobby(player.NickName, false);
            }
        }
        else
        {
            // Master is initialised as the Alien.
            lobbyRoom.PlayerJoinedLobby(PhotonNetwork.NickName, true);
        }

        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("{0} entered the room", other.NickName); // not seen if you're the player connecting

        if (!other.IsMasterClient)
        {
            lobbyRoom.PlayerJoinedLobby(other.NickName, false);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("NewPlayerEnteredRoom", RpcTarget.All, lobbyRoom.GetPlayerNames(), lobbyRoom.GetPlayerBools());
        }

        UpdatePlayerList();
    }

    public void OnLeaveLobby()
    {
        // Leave the lobby and reset the UI.
        PhotonNetwork.LeaveRoom();
        isConnection = false;
        
        menuContainer.SetActive(false);
        loadoutContainer.SetActive(true);
        lobbyContainer.SetActive(false);

        // Delete all player items from the player list panel.
        for (int i = 0; i < playerListPanel.childCount; i++)
        {
            Destroy(playerListPanel.GetChild(i).gameObject);
        }

        lobbyRoom.ReconstructList(new string[] {}, new bool[] {});
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("{0} left the room", other.NickName); // seen when other disconnects
        lobbyRoom.PlayerLeftLobby(other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("You are the master client");
            inLobbyPanel.transform.GetChild(0).gameObject.SetActive(true);
        }

        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        for(int i = 0; i < playerListPanel.childCount; i++)
        {
            Destroy(playerListPanel.GetChild(i).gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject go = Instantiate(playerItemPrefab, playerListPanel);

            if (PhotonNetwork.IsMasterClient)
            {
                // Add the OnAlienChanged function to the OnClick event on the button.
                go.GetComponent<Button>().interactable = true;
                go.GetComponent<Button>().onClick.AddListener(() => photonView.RPC("OnAlienChanged", RpcTarget.All, lobbyRoom.GetPlayerNames(), lobbyRoom.GetPlayerBools(), go.GetComponentInChildren<TMP_Text>().text));
            }
            else
            {
                go.GetComponent<Button>().interactable = false;
            }

            if (player.IsMasterClient)
            {
                go.GetComponentInChildren<TMP_Text>().text = "Room owner: " + player.NickName;
            }
            else
            {
                go.GetComponentInChildren<TMP_Text>().text = player.NickName;
            }

            if (lobbyRoom.IsPlayerAlien(player.NickName))
            {
                ColorBlock colBlock = go.GetComponent<Button>().colors;
                colBlock.normalColor = playerAlienButtonColor;
                colBlock.disabledColor = playerAlienButtonColor;
                go.GetComponent<Button>().colors = colBlock;
            }
            else
            {
                ColorBlock colBlock = go.GetComponent<Button>().colors;
                colBlock.normalColor = playerMarineButtonColor;
                colBlock.disabledColor = playerMarineButtonColor;
                go.GetComponent<Button>().colors = colBlock;
            }
        }
    }

    public void OnLoadGameClick()
    {
        // Fade screen to black and change the scene.
        photonView.RPC("MasterClientClickedLoadGame", RpcTarget.All);
    }

    private IEnumerator FadeScreenToBlack()
    {
        screenFader.gameObject.SetActive(true);
        for (float t = 0; t <= 1f; t += Time.deltaTime)
        {
            screenFader.color = Color.Lerp(Color.clear, Color.black, t / 1f);
            yield return null;
        }
        screenFader.color = Color.black;

        // Close room, call the RPC, and change the scene.
        if (PhotonNetwork.IsMasterClient) ScreenFadeFinished();
    }

    public void OnSettingsClick()
    {
        menuContainer.SetActive(false);
        settingsContainer.SetActive(true);
    }

    public void OnExitLoadoutClick()
    {
        menuContainer.SetActive(true);
        settingsContainer.SetActive(false);
        loadoutContainer.SetActive(false);
        lobbyContainer.SetActive(false);
    }

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }    

    [PunRPC]
    private void MasterClientClickedLoadGame()
    {        
        // Fade screen.
        StartCoroutine(FadeScreenToBlack());
    }

    private void ScreenFadeFinished()
    {
        Debug.Log("LOADING SCENE: " + gameScene);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(gameScene);
    }

    [PunRPC]
    private void OnAlienChanged(string[] names, bool[] bools, string newAlien)
    {
        lobbyRoom.ReconstructList(names, bools);
        lobbyRoom.AlienChanged(newAlien);
        UpdatePlayerList();
    }

    [PunRPC]
    private void NewPlayerEnteredRoom(string[] names, bool[] bools)
    {
        lobbyRoom.ReconstructList(names, bools);
        UpdatePlayerList();
    }
}
