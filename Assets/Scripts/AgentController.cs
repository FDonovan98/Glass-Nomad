using UnityEngine;
using Photon.Pun;

public class AgentController : AgentInputHandler
{
    public Color alienVision;
    public bool specialVision = false;
    public enum ResourceType
    {
        Health,
        Ammo
    }
    
    public GameObject[] gameObjectsToDisableForPhoton;
    public Behaviour[] componentsToDisableForPhoton;

    private void Awake()
    {   
        runCommandOnWeaponFired += FireWeaponOverNet;

        if (specialVision && photonView.IsMine)
        {
            SpawnFadeFromBlack.Fade(Color.black, alienVision, 3, this);
        }

        if (photonView != null)
        {
            if (!photonView.IsMine && !PhotonNetwork.PhotonServerSettings.StartInOfflineMode)
            {
                DisableObjectsForPhoton();
            }
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

    public void ChangeResourceCount(ResourceType resourceType, int value)
    {

        if (resourceType == ResourceType.Ammo)
        {
            currentBulletsInMag = (int)Mathf.Clamp(currentBulletsInMag + value, 0.0f, currentWeapon.magSize);

            if (ammoUIText != null)
            {
                ammoUIText.text = "Ammo: " + currentBulletsInMag + " / " + currentTotalAmmo;
            }
        }
    }

    public void ChangeResourceCount(ResourceType resourceType, float value)
    {
        if (resourceType == ResourceType.Health)
        {
            currentHealth = Mathf.Clamp(currentHealth + value, 0.0f, agentValues.maxHealth);
            if (currentHealth <= 0)
            {
                AgentHasDied();
            }

            if (healthUIText != null)
            {
                healthUIText.text = "Health: " + Mathf.RoundToInt(currentHealth / agentValues.maxHealth * 100);
            }
        }
    }

    public void AgentHasDied()
    {
        // This line needs changing, ewwww.
        PhotonNetwork.Destroy(agent);
    }

    void FireWeaponOverNet(AgentInputHandler agentInputHandler)
    {
        photonView.RPC("Shoot", RpcTarget.All, agentInputHandler.agentCamera.transform.position, agentInputHandler.agentCamera.transform.forward, agentInputHandler.currentWeapon.range, agentInputHandler.currentWeapon.damage);
    }
}
