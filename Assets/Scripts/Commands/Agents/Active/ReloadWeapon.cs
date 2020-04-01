using UnityEngine;

[CreateAssetMenu(fileName = "DefaultReload", menuName = "Commands/Active/Reload", order = 0)]
public class Reload : ActiveCommandObject
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

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (Input.GetKeyDown(reloadKey))
        {
            if (CanReload(agentInputHandler))
            {
                int bulletsUsed;

                bulletsUsed = agentInputHandler.currentWeapon.magSize - agentInputHandler.currentBulletsInMag;

                if (bulletsUsed > agentInputHandler.currentTotalAmmo)
                {
                    bulletsUsed = agentInputHandler.currentTotalAmmo;
                }

                agentInputHandler.currentBulletsInMag += bulletsUsed;
                agentInputHandler.currentTotalAmmo -= bulletsUsed;
            }
        }
    }

    bool CanReload(AgentInputHandler agentInputHandler)
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
}