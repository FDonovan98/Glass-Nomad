using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string lobbySceneName = "SCN_Lobby"; // Used to change scene when we leave a room.
    [SerializeField] private Vector3 alienSpawnPoint = Vector3.zero; // Used to spawn the alien.
    [SerializeField] private Vector3 marineSpawnPoint = Vector3.zero; // Used to spawn the marines.

    #region devtools
    [Header("Developer Tools")]
    [SerializeField] private bool singlePlayerMarine = false; // Used to test the marine player, in testing.
    #endregion

    private void Start()
    {
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
