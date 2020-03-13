using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText = null;
    [SerializeField] private TMP_Text currentClipText = null;
    [SerializeField] private TMP_Text remainingClipsText = null;
    [SerializeField] private Slider oxygenSlider = null;
    [SerializeField] private TMP_Text oxygenPercentage = null;
    [SerializeField] private TMP_Text oxygenWarning = null;

    private float warningTriggerAmount = 0.3f;
    private PlayerAttack clientAttackScript = null;
    private float warningFlashTimer = 1f;
    private float time = 1f;
    
    public void UpdateUI(PlayerAttack attackScript = null)
    {
        if (!attackScript.photonView.IsMine)
        {
            return;
        }

        if (clientAttackScript == null && attackScript != null)
        {
            clientAttackScript = attackScript;
        }
        
        remainingClipsText.text = "Remaining clips: " + attackScript.resourcesScript.currentWeapon.magsLeft;
        currentClipText.text = "Current clip: " + attackScript.resourcesScript.currentWeapon.bulletsInCurrentMag + "/" + attackScript.resourcesScript.currentWeapon.magSize;
        healthText.text = "Health: " + attackScript.resourcesScript.currentHealth;
        oxygenSlider.value = attackScript.resourcesScript.oxygenAmountSeconds / attackScript.resourcesScript.maxOxygenAmountSeconds * 100; // Gets the percentage
        oxygenPercentage.text = Mathf.Ceil(oxygenSlider.value).ToString();

        //Layer 9 is AlienCharacter. Alien's don't breathe :/
        if (attackScript.gameObject.layer == 9 && oxygenSlider.IsActive())
        {
            oxygenSlider.gameObject.SetActive(false);
            remainingClipsText.gameObject.SetActive(false);
            currentClipText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (clientAttackScript == null)
        {
            return;
        }
        if (!clientAttackScript.photonView.IsMine)
        {
            return;
        }

        if (clientAttackScript.resourcesScript.oxygenAmountSeconds / clientAttackScript.resourcesScript.maxOxygenAmountSeconds <= warningTriggerAmount)
        {
            FlashWarning();
        }
        else
        {
            time = warningTriggerAmount;
            oxygenWarning.gameObject.SetActive(false);
        }
    }

    private void FlashWarning()
    {
        time += Time.deltaTime;
        if (time >= warningFlashTimer)
        {
            oxygenWarning.gameObject.SetActive(!oxygenWarning.gameObject.activeSelf);
            time = 0f;
        }
    }
}
