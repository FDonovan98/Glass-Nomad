using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
<<<<<<< HEAD
=======
using UnityEngine.UI;
using System.Collections;
using System;
>>>>>>> master

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region variable-declarations

    [SerializeField] private string gameSceneName = "SCN_Blockout"; // Changes scene when we are join a room.
    [SerializeField] private GameObject controlPanel = null; // Shows/hides the play button and input field.
    [SerializeField] private GameObject progressLabel = null; // Displays "Connecting..." to once the Connect() funtion is called.
    [SerializeField] private GameObject playerItemPrefab = null; // Displays the players in the lobby.
    [SerializeField] private GameObject inLobbyPanel = null; // Displays the lobby buttons when you join a room.
    [SerializeField] private Transform playerListPanel = null; // Contains all the playeritem prefabs.
    [SerializeField] private Image screenFader = null; // Fades the screen to black, when entering the game.
    [SerializeField] private GameObject title = null; // Disables the title when in a lobby.
    [SerializeField] private GameObject loadoutDropdowns = null; // Displays the loadout dropdowns.

    private const string playerNamePrefKey = "Player Name";
    private byte maxPlayersPerRoom = 5; // Sets a limit to the number of players in a room.
    private string gameVersion = "1"; // Separates users from each other by gameVersion.
    private bool isConnection = false; // Stop us from immediately joining the room if we leave it.

    #endregion
    
    public PlayersInLobby lobbyRoom = null;

    private void Awake()
    {
        // Means we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Makes sure the correct elements of the UI are visible.
    private void Start()
    {
        ToggleMenuItems(false);
    }

    public void Connect()
    {
        // Checks the player's input is empty - returns if it is.
        if (PlayerPrefs.GetString(playerNamePrefKey) == string.Empty)
        {
            return;
        }

        // Switches which UI elements are visable.
        ToggleMenuItems(true);

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

        OnLoadGameClick();
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
        ToggleMenuItems(false);
        inLobbyPanel.SetActive(false);
        loadoutDropdowns.SetActive(false);
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
        progressLabel.SetActive(false);
        inLobbyPanel.SetActive(true);
        loadoutDropdowns.SetActive(true);

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

    public void LeaveRoom()
    {
        // Leave the lobby and reset the UI.
        PhotonNetwork.LeaveRoom();
        isConnection = false;

        inLobbyPanel.SetActive(false);
        loadoutDropdowns.SetActive(false);
        ToggleMenuItems(false);

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
<<<<<<< HEAD
                Debug.Log("MASTER IN ROOM:: " + player.NickName);
                go.GetComponentInChildren<TMP_Text>().text = "Room owner: " + player.NickName;
            }
            else
            {
                Debug.Log("PLAYER IN ROOM:: " + player.NickName);
                go.GetComponentInChildren<TMP_Text>().text = player.NickName;
=======
                go.GetComponentInChildren<TMP_Text>().text = "Room owner: " + player.NickName;
            }
            else
            {
                go.GetComponentInChildren<TMP_Text>().text = player.NickName;
            }

            if (lobbyRoom.IsPlayerAlien(player.NickName))
            {
                go.GetComponent<Image>().color = Color.green;
            }
            else
            {
                go.GetComponent<Image>().color = Color.white;
>>>>>>> master
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

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ToggleMenuItems(bool toggle)
    {
        progressLabel.SetActive(toggle);
        controlPanel.SetActive(!toggle);
        title.SetActive(!toggle);
    }    

    [PunRPC]
    private void MasterClientClickedLoadGame()
    {        
        // Fade screen.
        StartCoroutine(FadeScreenToBlack());
    }

    private void ScreenFadeFinished()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(gameSceneName);
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
