using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Changes to this scene when we leave a room.
    [SerializeField] private string lobbySceneName = "SCN_Lobby";

    // Spawn points for the alien.
    [SerializeField] private GameObject alienSpawnPoint = null;

    // Spawn points for the marines.
    [SerializeField] private GameObject marineSpawnPoint = null;

    // Should we switch a marine to the alien, when the alien dies.
    [SerializeField] private  bool switchToAlien = false;

    // The radius of the marines spawn.
    [SerializeField] private float radius = 8f;
    

    /// <summary>
    /// Determines whether the game is in offline mode or not. If the game is offline, then we spawn an alien, 
    /// otherwise we hand over control to the 'SpawnLocalPlayer' function.
    /// </summary>
    private void Start()
    {
        if (!PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
        {
            SpawnLocalPlayer();
        }
        else
        {
            Instantiate(Resources.Load("Alien", typeof(GameObject)), alienSpawnPoint.transform.position, new Quaternion());
        }
    }

    /// <summary>
    /// Retrieves the Lobby gameobject, and fetches the information of the local player (using their
    /// nickname). From this, we determine if the player was marked as an alien in the lobby. If it
    /// was marked as an alien, then we spawn an alien; else we spawn a marine.
    /// </summary>
    private void SpawnLocalPlayer()
    {
        PlayersInLobby lobbyRoom = GameObject.Find("Lobby").GetComponent<PlayersInLobby>();

        if (lobbyRoom.IsPlayerAlien(PhotonNetwork.NickName))
        {
            PhotonNetwork.Instantiate("Alien", alienSpawnPoint.transform.position, new Quaternion());
        }
        else
        {
            GameObject marine = PhotonNetwork.Instantiate("Marine", GetRandomSpawnPoint(), new Quaternion());
            AgentController agentController = marine.GetComponentInChildren<AgentController>();
            agentController.ChangeWeapon(lobbyRoom.localPlayer.primaryWeapon);
            agentController.ChangeArmour(lobbyRoom.localPlayer.selectedArmour);
            agentController.ChangeMaterial(lobbyRoom.localPlayer.selectedMaterial, lobbyRoom.localPlayer.selectedMaterialIndex);
        }
    }

    /// <summary>
    /// Gets a random Vector2 point inside of a circle of radius of 8.
    /// This Vector2 point is added to the original marine spawn point.
    /// </summary>
    /// <returns>A random Vector3 position</returns>
    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 pos = marineSpawnPoint.transform.position;
        Vector2 circle = UnityEngine.Random.insideUnitCircle * radius;
        pos.x += circle.x - (radius / 2);
        pos.z += circle.y - (radius / 2);
        Debug.Log(pos);
        return pos;
    }
    
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(lobbySceneName);
    }
    
    public override void OnPlayerEnteredRoom(Player other)
    {
        // not seen if you're the player connecting
        Debug.LogFormat("{0} entered the game room", other.NickName);

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
    /// <param name="newMasterClient">The player that is to be switched to master.</param>
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

                        if (element.GetComponent<AgentController>().agentValues.name == "AlienAgentValues")
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
}
