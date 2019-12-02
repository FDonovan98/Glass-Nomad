using UnityEngine;

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

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("SCN_Blockout");
        }
    }
}
