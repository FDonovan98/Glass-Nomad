using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Changes to this scene when we leave a room.
    [SerializeField] private string lobbySceneName = "SCN_Lobby";

    // Spawn points for the alien.
    [SerializeField] private GameObject alienSpawnPoint = null;

    // Spawn points for the marines.
    [SerializeField] private GameObject marineSpawnPoint = null;

    // Used by PlayerMovement to access the pause menu gameobject.
    public GameObject pauseMenu;

    // Should we switch a marine to the alien, when the alien dies.
    public bool switchToAlien = false;

    private void Start()
    {
        if (!PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
        {
            SpawnLocalPlayer();
        }
        else
        {
            Debug.Log("Spawning an alien, hopefully.");
            Instantiate(Resources.Load("Alien (Cylinder)", typeof(GameObject)), alienSpawnPoint.transform.position, new Quaternion());
        }
    }

    private void SpawnLocalPlayer()
    {
        PlayersInLobby lobbyRoom = GameObject.Find("Lobby").GetComponent<PlayersInLobby>();
        if (lobbyRoom.IsPlayerAlien(PhotonNetwork.NickName))
        {
            PhotonNetwork.Instantiate("Alien (Cylinder)", alienSpawnPoint.transform.position, new Quaternion());
        }
        else
        {
            PhotonNetwork.Instantiate("Marine (Cylinder)", GetRandomSpawnPoint(), new Quaternion());
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        int radius = 8;
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

    /// <summary>
    /// Changes the model of the new master client, when the old one leaves.
    /// </summary>
    /// <param name="newMasterClient"></param>
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
}
