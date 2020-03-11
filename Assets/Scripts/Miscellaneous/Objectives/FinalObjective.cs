using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class FinalObjective : TriggerInteractionScript
{
    [SerializeField] private float waitTimer = 10f;
    [SerializeField] private float timer = 30f;
    [SerializeField] private Collider evacZone = null;
    [SerializeField] private GameObject gameover = null;

    protected override void InteractionComplete(GameObject player)
    {
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
        Debug.Log("MARINE COUNT IN EVAC: " + evacZone.gameObject.GetComponent<EvacZone>().numberOfMarinesInEvac);
        if (evacZone.gameObject.GetComponent<EvacZone>().numberOfMarinesInEvac > 0)
        {
            gameover.GetComponent<TMP_Text>().text = "Marines won!";
        }
        else
        {
            gameover.GetComponent<TMP_Text>().text = "Alien won!";
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
