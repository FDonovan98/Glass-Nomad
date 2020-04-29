using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Spectator : MonoBehaviourPunCallbacks
{
    [Header("Spectator Settings")]
    [SerializeField] private KeyCode toggleFreeCamKey = KeyCode.G;
    [SerializeField] private KeyCode backPlayerKey = KeyCode.Q;
    [SerializeField] private KeyCode forwardPlayerKey = KeyCode.E;
    [SerializeField] private float speed = 50f;
    [SerializeField] private float mouseSensitivity = 10f;

    [Header("Objects & Components")]
    [SerializeField] private List<GameObject> gameObjectsToRemove = new List<GameObject>();
    [SerializeField] private List<Component> componentsToRemove = new List<Component>();

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private KeyCode openMenuKey = KeyCode.Escape;
    [SerializeField] private KeyCode openMenuKeyInEditor = KeyCode.Comma;

    [Header("Other")]
    [SerializeField] private TMP_Text captionText = null;
    public int playerIndex = 0;
    private List<GameObject> playerList = new List<GameObject>();
    private Camera cam = null;
    private bool freeCamera = true;
    private bool inputEnabled = true;

    private new void OnEnable()
    {
        if (GetComponentInChildren<AgentController>().agentValues.name == "AlienAgentValues")
        {
            photonView.RPC("SendPlayersToLobby", RpcTarget.All);
            return;
        }

        RemoveComponents();
        RemoveGameObjects();
        cam = GetComponentInChildren<Camera>();
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Cursor.lockState = CursorLockMode.Locked;
        GetAlivePlayers();
        captionText.text = "<- Q    G to toggle follow     E ->";
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
        CheckIfMenuButtonWasPressed();

        if (!inputEnabled) return;

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

    private void CheckIfMenuButtonWasPressed()
    {
        #if UNITY_EDITOR
            //Press the openMenuKeyInEditor to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(openMenuKeyInEditor))
            {
                ToggleCursorAndMenu();
            } 
        #elif UNITY_STANDALONE_WIN
            //Press the openMenuKey to unlock the cursor. If it's unlocked, lock it again
            if (Input.GetKeyDown(openMenuKey))
            {
                ToggleCursorAndMenu();
            } 
        #endif
    }

    private void ToggleCursorAndMenu()
    {
        bool displayMenu = Cursor.lockState == CursorLockMode.Locked ? true : false;
        Cursor.lockState = displayMenu ? CursorLockMode.None : CursorLockMode.Locked;
        ToggleMenu(displayMenu);
    }

    private void ToggleMenu(bool toggle)
    {
        pauseMenu.SetActive(toggle);
        inputEnabled = !toggle;
        Cursor.visible = toggle;
    }
}
