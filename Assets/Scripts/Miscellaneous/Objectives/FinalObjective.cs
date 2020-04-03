using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class FinalObjective : ObjectiveInteraction
{
    // The time to wait before starting the countdown.
    [SerializeField] private float waitTimer = 10f;

    // The time the marines have to escape.
    [SerializeField] private float timer = 30f;

    // The evacuation zone.
    [SerializeField] private EvacZone evacZone = null;

    // The gameover text component.
    private TMP_Text gameover = null;

    protected override void InteractionComplete(GameObject player)
    {
        base.InteractionComplete(player);
        gameover = player.GetComponent<AgentController>().transform.GetChild(2).GetChild(0).GetChild(1).GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// Assigns the game over text component and marks the interaction and objective as completed.
    /// If the objective has been successfully marked as completed, then we start the countdown timer.
    /// </summary>
    /// /// <param name="player"></param>
    protected override void ObjectiveComplete()
    {
        StartCoroutine(StartTimer());
    }

    /// <summary>
    /// Enables the alien's objective caption text, as the marines should already be active, and waits
    /// a short amount of time, for the text to finishing writing to the HUD. Then, the actual countdown
    /// timer until the evacuation starts; constantly writing the time left to the HUD. Once the timer
    /// has finished, it initiates the game over sequence.
    /// </summary>
    /// <returns>Nothing</returns>
    private IEnumerator StartTimer()
    {
        // Turns the alien's caption text on.
        Objectives.captionText.gameObject.SetActive(true);

        // Wait for text to finish displaying and audio to start playing
        for (float i = 0; i < waitTimer; i += Time.deltaTime)
        {
            yield return null;
        }

        int currSecond = 0;

        for (float i = 0; i < timer; i += Time.deltaTime)
        {
            if (currSecond != Mathf.FloorToInt(i))
            {
                currSecond = Mathf.FloorToInt(i);
                Debug.Log("TIMER : " + currSecond);
                Objectives.WriteTextToHud(currSecond.ToString(), 0f, 0.9f);
            }
            yield return null;
        }
        
        Objectives.WriteTextToHud(timer.ToString(), Time.deltaTime, 0.9f);
        StartGameOverSequence();
    }

    /// <summary>
    /// Determines which side won the game, and proceeds to display which team won to the HUD.
    /// Then, it switches everyone to the main menu.
    /// </summary>
    private void StartGameOverSequence()
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
        gameover.gameObject.SetActive(true);
        StartCoroutine(SwitchToMainMenu());
    }

    /// <summary>
    /// Leaves the photon room, unlocks the cursor, and changes the scene back to the lobby.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LeaveRoom();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("SCN_Lobby");
    }
}
