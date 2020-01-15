using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
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
}
