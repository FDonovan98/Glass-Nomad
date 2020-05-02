using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using System;
using System.Threading.Tasks;
using TriggerExtensionMethods;
using UnityEngine.UI;

public class FinalObjective : ObjectiveInteraction
{
    [Header("Final Objective")]
    // The time to wait before starting the countdown.
    [SerializeField] private float waitTimer = 10f;

    // The time the marines have to escape.
    [SerializeField] private float timer = 90f;

    // The evacuation zone.
    [SerializeField] private EvacZone evacZone = null;

    // The gameover text component.
    private TMP_Text gameover = null;

    /// <summary>
    /// Assigns the game over text component and marks the interaction and objective as completed.
    /// If the objective has been successfully marked as completed, then we start the countdown timer.
    /// </summary>
    /// /// <param name="player"></param>
    [PunRPC]
    protected override void InteractionComplete()
    {
        base.InteractionComplete();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                playerInteracting = player;
            }
        }
        gameover = playerInteracting.GetComponent<AgentController>().transform.parent.GetChild(1).gameObject.FindComponentWithTag<TMP_Text>("Gameover Text");
    }

    protected override void ObjectiveComplete()
    {
        StartTimer();
    }

    /// <summary>
    /// Enables the alien's objective caption text, as the marines should already be active, and waits
    /// a short amount of time, for the text to finishing writing to the HUD. Then, the actual countdown
    /// timer until the evacuation starts; constantly writing the time left to the HUD. Once the timer
    /// has finished, it initiates the game over sequence.
    /// </summary>
    /// <returns>Nothing</returns>
    private async void StartTimer()
    {
        // Wait for text to finish displaying and audio to start playing
        await Task.Delay(TimeSpan.FromSeconds(waitTimer));

        int currSecond = 0;
        for (float i = 0; i <= timer; i += Time.deltaTime)
        {
            if (currSecond != Mathf.FloorToInt(i))
            {
                currSecond = Mathf.FloorToInt(i);
                if (debug) Debug.Log("TIMER : " + currSecond);
                captionText.text = "<mark=#000000aa>" + currSecond.ToString() + "</mark>";
                await Task.Delay(TimeSpan.FromSeconds(0.9f));
                captionText.text = "";
            }
        }

        photonView.RPC("StartGameOverSequence", RpcTarget.All);
    }

    /// <summary>
    /// Determines which side won the game, and proceeds to display which team won to the HUD.
    /// Then, it switches everyone to the main menu.
    /// </summary>
    [PunRPC]
    public void StartGameOverSequence()
    {
        Debug.Log("MARINE COUNT IN EVAC: " + evacZone.numberOfMarinesInEvac);
        if (evacZone.numberOfMarinesInEvac > 0)
        {
            gameover.text = "Marines won!";
        }
        else
        {
            gameover.text = "Alien won!";
        }
        gameover.gameObject.transform.parent.GetChild(1).gameObject.SetActive(true);
        SwitchToMainMenu();
    }

    /// <summary>
    /// Leaves the photon room, unlocks the cursor, and changes the scene back to the lobby.
    /// </summary>
    /// <returns></returns>
    private async void SwitchToMainMenu()
    {
        await Task.Delay(TimeSpan.FromSeconds(5f));
        PhotonNetwork.LeaveRoom();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("SCN_Lobby");
    }
}
