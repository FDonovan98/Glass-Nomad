using UnityEngine;

using Photon.Pun;

public class AgentController : AgentInputHandler
{
    public GameObject[] gameObjectsToDisableForPhoton;
    public Behaviour[] componentsToDisableForPhoton;

    void Awake()
    {
        if (!photonView.IsMine && !PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
        {
            DisableObjectsForPhoton();
        }
    }

    void DisableObjectsForPhoton()
    {
        foreach (GameObject element in gameObjectsToDisableForPhoton)
        {
            element.SetActive(false);   
        }
        foreach (Behaviour element in componentsToDisableForPhoton)
        {
            element.enabled = false;   
        }
    }
}
