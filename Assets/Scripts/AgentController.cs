using UnityEngine;

using Photon.Pun;

public class AgentController : AgentInputHandler
{
    public enum ResourceType
    {
        Health,
        Ammo
    }
    
    public GameObject[] gameObjectsToDisableForPhoton;
    public Behaviour[] componentsToDisableForPhoton;

    void Awake()
    {
        healthUIText.text = "Health: " + Mathf.RoundToInt(currentHealth / agentValues.maxHealth * 100);
        
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

    public void ChangeResourceCount(ResourceType resourceType, int value)
    {
        if (resourceType == ResourceType.Health)
        {
            currentHealth = (int)Mathf.Clamp(currentHealth + value, 0.0f, agentValues.maxHealth);
            if (currentHealth == 0)
            {
                AgentHasDied();
            }

            healthUIText.text = "Health: " + Mathf.RoundToInt(currentHealth / agentValues.maxHealth * 100);
        }

        if (resourceType == ResourceType.Ammo)
        {
            currentBulletsInMag = (int)Mathf.Clamp(currentBulletsInMag + value, 0.0f, currentWeapon.magSize);

            ammoUIText.text = "Ammo: " + currentBulletsInMag + " / " + currentTotalAmmo;
        }
    }

    public void AgentHasDied()
    {
        // This line needs changing, ewwww.
        PhotonNetwork.Destroy(agent);
    }
}
