using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultReload", menuName = "Commands/Active/ReloadWeapon", order = 0)]
public class ReloadWeapon : ActiveCommandObject
{
    [SerializeField]
    KeyCode reloadKey = KeyCode.R;

    protected override void OnEnable()
    {
        keyTable.Add("Reload", reloadKey);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    private void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (Input.GetKeyDown(reloadKey))
        {
            if (CanReload(agentInputHandler))
            {
                AudioSource weaponAudioSource = agentInputHandler.weaponObject.GetComponent<AudioSource>();
                weaponAudioSource.PlayOneShot(agentInputHandler.currentWeapon.reloadSound);

                agentInputHandler.StartCoroutine(Reload(weaponAudioSource.clip.length, agentInputHandler));
            }
        }
    }

    private bool CanReload(AgentInputHandler agentInputHandler)
    {
        if (agentInputHandler.currentTotalAmmo > 0)
        {
            if (agentInputHandler.currentBulletsInMag < agentInputHandler.currentWeapon.magSize)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator Reload(float reloadTime, AgentInputHandler agentInputHandler)
    {
        yield return new WaitForSeconds(reloadTime);
        int bulletsUsed;

        bulletsUsed = agentInputHandler.currentWeapon.magSize - agentInputHandler.currentBulletsInMag;

        if (bulletsUsed > agentInputHandler.currentTotalAmmo)
        {
            bulletsUsed = agentInputHandler.currentTotalAmmo;
        }

        agentInputHandler.currentBulletsInMag += bulletsUsed;
        agentInputHandler.currentTotalAmmo -= bulletsUsed;

        agentInputHandler.ammoUIText.text = "Ammo: " + agentInputHandler.currentBulletsInMag + " / " + agentInputHandler.currentTotalAmmo;
    }
}