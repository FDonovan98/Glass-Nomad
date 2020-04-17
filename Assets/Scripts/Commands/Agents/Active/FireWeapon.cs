using UnityEngine;

[CreateAssetMenu(fileName = "DefaultFireWeapon", menuName = "Commands/Active/FireWeapon", order = 0)]
public class FireWeapon : ActiveCommandObject
{
    [SerializeField]
    private KeyCode primaryFire = KeyCode.Mouse0;
    protected override void OnEnable()
    {
        keyTable.Add("Primary Fire", primaryFire);
    }
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        if (agentInputHandler.isLocalAgent)
        {
            agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
        }
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        agentInputHandler.timeSinceLastShot += Time.deltaTime;

        if (CanFire(agentInputHandler))
        {
            if (Input.GetKeyDown(primaryFire))
            {
                ActuallyFire(agent, agentInputHandler);
            }
            else if (agentInputHandler.currentWeapon.fireMode == Weapon.FireType.FullAuto && Input.GetKey(primaryFire))
            {
                ActuallyFire(agent, agentInputHandler);
            }
        }
    }

    bool CanFire(AgentInputHandler agentInputHandler)
    {
        AgentController agentController = (AgentController)agentInputHandler;

        if (agentInputHandler.allowInput)
        {
            if (agentInputHandler.timeSinceLastShot > agentInputHandler.currentWeapon.fireRate)
            {
                if (agentInputHandler.currentWeapon.fireMode == Weapon.FireType.Melee || agentController.currentBulletsInMag > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void ActuallyFire(GameObject agent, AgentInputHandler agentInputHandler)
    {
        if (agentInputHandler.runCommandOnWeaponFired != null)
        {
            agentInputHandler.runCommandOnWeaponFired(agentInputHandler);
        }

        AgentController agentController = (AgentController)agentInputHandler;

        agentInputHandler.timeSinceLastShot = 0.0f;

        agentController.ChangeResourceCount(AgentController.ResourceType.MagazineAmmo, -1);
    }
}