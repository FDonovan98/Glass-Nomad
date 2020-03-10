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
    [SerializeField] private TMP_Text gameoverText = null;

    protected override void InteractionComplete(GameObject player)
    {
        base.InteractionComplete(player);

        if (!Objectives.IsObjectiveComplete(objectiveName)) return;

        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        // Wait for text to finish displaying and audio to start playing
        for (float i = 0; i < waitTimer; i += Time.deltaTime)
        {
            yield return null;
        }

        for (float i = 0; i < timer; i += Time.deltaTime)
        {
            Debug.Log("TIMER : " + i);
            yield return null;
        }
        
        StartGameOverSequence();
    }

    private void StartGameOverSequence()
    {
        Debug.Log("MARINE COUNT IN EVAC: " + evacZone.gameObject.GetComponent<EvacZone>().numberOfMarinesInEvac);
        if (evacZone.gameObject.GetComponent<EvacZone>().numberOfMarinesInEvac > 0)
        {
            gameoverText.text = "Marines won!";
        }
        else
        {
            gameoverText.text = "Alien won!";
        }
        gameoverText.gameObject.SetActive(true);
        StartCoroutine(SwitchToMainMenu());
    }

    private IEnumerator SwitchToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("SCN_Lobby");
    }
}
