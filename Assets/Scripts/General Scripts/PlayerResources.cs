using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerResources
{
    public enum PlayerResource
    {
        Health,
        Ammo,
        Magazines,
        OxygenLevel
    }
    
    public int maxHealth;
    public int currentHealth;
    public float fillAmount;
    public GameObject player;
    public UIBehaviour hudCanvas;

    // Oxygen shenanigans
    public float maxOxygenAmountSeconds = 300f;
    public float oxygenAmountSeconds;

    [HideInInspector] public int magsLeft;
    [HideInInspector] public int bulletsInCurrentMag;

    private MonoBehaviour monoBehaviour;

    public PlayerResources(GameObject attachedPlayer, MonoBehaviour instance, int playerMaxHealth = 100)
    {
        maxHealth = playerMaxHealth;
        currentHealth = maxHealth;
        player = attachedPlayer;
        hudCanvas = GameObject.Find("EMP_UI").GetComponentInChildren<UIBehaviour>();
        oxygenAmountSeconds = maxOxygenAmountSeconds;
        monoBehaviour = instance;
    }

    private void UpdateFillAmount()
    {
        fillAmount = (float)currentHealth / maxHealth;
    }

    //
    public void UpdatePlayerResource(PlayerResource playerResource, float value)
    {
        if (playerResource == PlayerResource.OxygenLevel)
        {
            oxygenAmountSeconds += value;
            if (oxygenAmountSeconds < 0)
            {
                oxygenAmountSeconds = 0;
            }
            if (oxygenAmountSeconds > maxOxygenAmountSeconds)
            {
                oxygenAmountSeconds = maxOxygenAmountSeconds;
            }
        }
        else if (playerResource == PlayerResource.Ammo)
        {
            bulletsInCurrentMag += (int)value;
            if (bulletsInCurrentMag > player.GetComponent<PlayerAttack>().currentWeapon.magSize)
            {
                bulletsInCurrentMag = player.GetComponent<PlayerAttack>().currentWeapon.magSize;
            }
            if (bulletsInCurrentMag < 0)
            {
                bulletsInCurrentMag = 0;
            }
        }
        else if (playerResource == PlayerResource.Magazines)
        {
            magsLeft += (int)value;
            if (magsLeft < 0)
            {
                magsLeft = 0;
            }
        }
        else if (playerResource == PlayerResource.Health)
        {
            if (currentHealth < -(int)value)
            {
                if (player.GetComponent<MarineMovement>() != null)
                    player.GetComponent<MarineMovement>().Ragdoll();
                else if (player.GetComponent<AlienController>() != null)
                    monoBehaviour.StartCoroutine(Death(player));


            }
            else
            {
                currentHealth += (int)value;
            }

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            UpdateFillAmount();
        }
        hudCanvas.UpdateUI(player.GetComponent<PlayerAttack>());
    }

    public IEnumerator Death(GameObject player)
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.Destroy(player);
        PhotonNetwork.LeaveRoom();
    }
}
