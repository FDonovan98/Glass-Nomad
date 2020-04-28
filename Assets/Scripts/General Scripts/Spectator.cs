using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spectator : MonoBehaviourPunCallbacks
{
    [SerializeField] private KeyCode toggleFreeCamKey = KeyCode.G;
    [SerializeField] private KeyCode backPlayerKey = KeyCode.Q;
    [SerializeField] private KeyCode forwardPlayerKey = KeyCode.E;
    [SerializeField] private float speed = 50f;
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private List<GameObject> gameObjectsToRemove = new List<GameObject>();
    [SerializeField] private List<Component> componentsToRemove = new List<Component>();
    private List<GameObject> playerList = new List<GameObject>();
    public int playerIndex = 0;
    private Camera cam = null;
    private bool freeCamera = true;

    private new void OnEnable()
    {
        if (GetComponentInChildren<AgentController>().agentValues.name == "AlienAgentValues") photonView.RPC("SendPlayersToLobby", RpcTarget.All);
        RemoveComponents();
        RemoveGameObjects();
        cam = GetComponentInChildren<Camera>();
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Cursor.lockState = CursorLockMode.Locked;
        GetAlivePlayers();
    }

    private void RemoveComponents()
    {
        foreach (Component comp in componentsToRemove)
        {
            Destroy(comp);
        }
    }

    private void RemoveGameObjects()
    {
        foreach (GameObject go in gameObjectsToRemove)
        {
            Destroy(go);
        }
    }

    private void GetAlivePlayers()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in allPlayers)
        {
            AgentController agent = player.GetComponentInChildren<AgentController>();
            if (agent != null && agent.enabled)
            {
                if (agent.agentValues.name == "MarineAgentValues" && player != this.gameObject)
                {
                    playerList.Add(player);
                }
            }
        }

        if (playerList.Count == 0) // There are no more players alive.
        {
            photonView.RPC("SendPlayersToLobby", RpcTarget.All);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleFreeCamKey)) ToggleFreeCamera();
        
        if (freeCamera)
        {
            FreeCamera();
        }
        else
        {
            if (Input.GetKeyDown(backPlayerKey)) SelectNewPlayer(-1);
            if (Input.GetKeyDown(forwardPlayerKey)) SelectNewPlayer(1);
            FollowPlayer();
        }
    }

    private Vector3 GetMouseInput()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        return new Vector3(-y, x, 0) * mouseSensitivity;
    }

    private Vector3 GetMovementInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        return new Vector3(x, 0, y) * speed;
    }

    private void SelectNewPlayer(int increment)
    {
        playerList[playerIndex].GetComponentInChildren<Camera>().enabled = false;
        playerIndex += increment;

        if (playerIndex > playerList.Count - 1)
        {
            playerIndex = 0;
        }
        else if (playerIndex < 0)
        {
            playerIndex = playerList.Count - 1;
        }
        playerList[playerIndex].GetComponentInChildren<Camera>().enabled = true;
    }

    private void ToggleFreeCamera()
    {
        freeCamera = !freeCamera;
        cam.enabled = freeCamera;
        playerList[playerIndex].GetComponentInChildren<Camera>().enabled = !freeCamera;
    }

    private void FreeCamera()
    {
        transform.position += cam.transform.TransformDirection(GetMovementInput());
        transform.Rotate(new Vector3(0, GetMouseInput().y, 0));
        cam.transform.Rotate(new Vector3(GetMouseInput().x, 0, 0));
    }

    private void FollowPlayer()
    {
        cam.enabled = freeCamera;
        playerList[playerIndex].GetComponentInChildren<Camera>().enabled = true;
    }

    [PunRPC]
    private void SendPlayersToLobby()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("SCN_Lobby");
    }
}
