using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;

    private byte maxPlayersPerRoom = 5;
    private string gameVersion = "1";
    // Forces the user to only be connected to a game when they've pressed the connect button.
    private bool isConnection;


    #region testvariables

    public Transform playerListPanel;
    public GameObject playerItemPrefab;
    public GameObject loadGameButton;

    #endregion


    // Set to true so all clients have the same scene loaded.
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Makes sure the correct elements of the UI are visible.
    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void Connect()
    {
        // Switches which UI elements are visable.
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        // The button has been pressed so we want the user to connect to a room.
        isConnection = true;
        
        // Checks if the client is aleady connected
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Connected, joining a random room");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region teststuff
    
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }

        UpdatePlayerList();
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
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
            if (player.IsMasterClient)
            {
                Debug.Log("MASTER IN ROOM:: " + player.NickName);
                go.GetComponentInChildren<Text>().text = "Room owner: " + player.NickName;
            }
            else
            {
                Debug.Log("PLAYER IN ROOM:: " + player.NickName);
                go.GetComponentInChildren<Text>().text = player.NickName;
            }
        }
    }

    public void OnLoadGameClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("SCN_Blockout");
        }
    }

    #endregion

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");

        if (isConnection)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected");
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No room found, creating new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined successfully");
        progressLabel.SetActive(false);


        if (PhotonNetwork.IsMasterClient)
        {
            loadGameButton.SetActive(true);
        }

        UpdatePlayerList();
    }
}
