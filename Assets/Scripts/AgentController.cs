using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

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
                isLocalAgent = false;
            }
        }
    }

    private void DisableObjectsForPhoton()
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

    /// <summary>
    /// Disables the player's input, enables rotations in the rigidbody, adds a random force to the
    /// rigidbody, and starts the 'Death' coroutine.
    /// </summary>
    public void AgentHasDied()
    {
        allowInput = false;
        agent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        agent.GetComponent<Rigidbody>().AddForceAtPosition(RandomForce(150f), transform.position);
        StartCoroutine(Death(agent));
    }
    
    /// <summary>
    /// Returns a vector with all axes having a random value between 0 and the 'velocity' parameter.
    /// </summary>
    /// <param name="velocity">The maximum random force.</param>
    /// <returns>Returns a vector with all axes having a random value between 0 and the 'velocity' parameter.</returns>
    private Vector3 RandomForce(float velocity)
    {
        return new Vector3(Random.Range(0, velocity), Random.Range(0, velocity), Random.Range(0, velocity));
    }
    
    private IEnumerator Death(GameObject player)
    {
        yield return new WaitForSeconds(3f);
        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PhotonNetwork.LeaveRoom();
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(player);
        }
    }

    private void FireWeaponOverNet(AgentInputHandler agentInputHandler)
    {
        RaycastHit hit;
        if (Physics.Raycast(agentCamera.transform.position, agentCamera.transform.forward, out hit, currentWeapon.range))
        {
            if (hit.transform.tag == "Player")
            {
                int targetPhotonID = hit.transform.GetComponent<PhotonView>().ViewID;
                photonView.RPC("PlayerWasHit", RpcTarget.All, targetPhotonID, hit.point, hit.normal, currentWeapon.damage);
            }
            else
            {          
                photonView.RPC("WallWasHit", RpcTarget.All, agentInputHandler.agentCamera.transform.position, agentInputHandler.agentCamera.transform.forward, agentInputHandler.currentWeapon.range, agentInputHandler.currentWeapon.damage);
            }
        }
    }
}
