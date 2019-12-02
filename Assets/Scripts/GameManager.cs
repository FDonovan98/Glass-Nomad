using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string lobbySceneName = "SCN_Launcher"; // Used to change scene when we leave a room.
    private GameObject player; // Used to change the player's name tag, above their head.

    private void Start()
    {
        player = PhotonNetwork.Instantiate("Alien (Cylinder)", new Vector3(0.0f, 1.0f, 0.0f), new Quaternion());
        Debug.Log(PhotonNetwork.CountOfPlayers.ToString() + " player(s) in game");
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
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        player.GetComponentInChildren<Text>().text = other.NickName; // Sets the name tag above the player to its nickname.

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }
}
