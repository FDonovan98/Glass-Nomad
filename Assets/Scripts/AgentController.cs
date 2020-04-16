using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

using UnityEngine.UI;

public class AgentController : AgentInputHandler
{
    public GameObject deathScreen;
    public Color alienVision;
    public bool specialVision = false;

    [Header("UI")]
    public TextMeshProUGUI healthUIText;
    public TextMeshProUGUI ammoUIText;
    public GameObject oxygenDisplay;

    [Header("Current Stats")]
    [ReadOnly]
    public float currentHealth = 0.0f;
    [ReadOnly]
    public float currentOxygen = 0.0f;
    [ReadOnly]
    public int currentTotalAmmo = 0;
    [ReadOnly]
    public int currentBulletsInMag = 0;

    public enum ResourceType
    {
        Health,
        Ammo,
        Oxygen
    }
    
    public GameObject[] gameObjectsToDisableForPhoton;
    public Behaviour[] componentsToDisableForPhoton;

    private void Awake()
    { 
        if (agentValues != null)
        {
            currentOxygen = agentValues.maxOxygen;
            currentHealth = agentValues.maxHealth;
        }

        if (currentWeapon != null)
        {
            currentBulletsInMag = currentWeapon.bulletsInCurrentMag;
            currentTotalAmmo = currentWeapon.magSize * 3;
            timeSinceLastShot = currentWeapon.fireRate;
        }

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
                UpdateUI(ResourceType.Ammo);
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
                UpdateUI(ResourceType.Health);
            }
        }
        else if (resourceType == ResourceType.Oxygen)
        {
            currentOxygen = Mathf.Clamp(currentOxygen + value, 0.0f, agentValues.maxOxygen);

            if (currentOxygen == 0.0f)
            {
                ChangeResourceCount(AgentController.ResourceType.Health, -(agentValues.suffocationDamage * Time.deltaTime));
            }

            if (oxygenDisplay != null)
            {
                UpdateUI(ResourceType.Oxygen);
            }
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

    void UpdateUI()
    {
        ammoUIText.text = "Ammo: " + currentBulletsInMag + " / " + currentTotalAmmo;
        healthUIText.text = "Health: " + Mathf.RoundToInt(currentHealth / agentValues.maxHealth * 100);

        Slider oxygenSlider = oxygenDisplay.GetComponentInChildren<Slider>();
        TextMeshProUGUI oxygenText = oxygenDisplay.GetComponentInChildren<TextMeshProUGUI>();
        oxygenSlider.value = currentOxygen / agentValues.maxOxygen * 100;
        oxygenText.text = (Mathf.Round(currentOxygen / agentValues.maxOxygen * 100)).ToString();
    }

    void UpdateUI(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Ammo:
                ammoUIText.text = "Ammo: " + currentBulletsInMag + " / " + currentTotalAmmo;
                break;
            case ResourceType.Health:
                healthUIText.text = "Health: " + Mathf.RoundToInt(currentHealth / agentValues.maxHealth * 100);
                break;
            case ResourceType.Oxygen:
                Slider oxygenSlider = oxygenDisplay.GetComponentInChildren<Slider>();
                TextMeshProUGUI oxygenText = oxygenDisplay.GetComponentInChildren<TextMeshProUGUI>();
                oxygenSlider.value = currentOxygen / agentValues.maxOxygen * 100;
                oxygenText.text = (Mathf.Round(currentOxygen / agentValues.maxOxygen * 100)).ToString();
                break;
            default:
                Debug.LogWarning(gameObject.name + " tried to update UI of unrecognized type.");
                break;
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
        
        deathScreen.SetActive(true);

        yield return new WaitForSeconds(3f);

        deathScreen.SetActive(false);
        agent.GetComponent<PhotonView>().ObservedComponents.Clear();
        agent.GetComponent<Spectator>().enabled = true;
    }

}
