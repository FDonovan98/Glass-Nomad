using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class FinalObjective : TriggerInteractionScript
{
    // The time to wait before starting the countdown.
    [SerializeField] private float waitTimer = 10f;

    // The time the marines have to escape.
    [SerializeField] private float timer = 30f;

    // The evacuation zone.
    [SerializeField] private EvacZone evacZone = null;

    // The gameover text
    private TMP_Text gameover = null;

    protected override void InteractionComplete(GameObject player)
    {
        gameover = player.GetComponent<AgentController>().transform.GetChild(2).GetChild(0).GetChild(1).GetComponentInChildren<TMP_Text>();
        base.InteractionComplete(player);
        if (!Objectives.IsObjectiveComplete(objectiveName)) return;
        StartCoroutine(StartTimer());
    }

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

    private IEnumerator SwitchToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LeaveRoom();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("SCN_Lobby");
    }
}
