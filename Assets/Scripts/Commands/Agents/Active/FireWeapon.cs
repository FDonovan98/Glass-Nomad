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
        if (agentInputHandler.allowInput)
        {
            if (agentInputHandler.timeSinceLastShot > agentInputHandler.currentWeapon.fireRate)
            {
                if (agentInputHandler.currentBulletsInMag > 0 || agentInputHandler.currentWeapon.fireMode == Weapon.FireType.Melee)
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

        agentInputHandler.timeSinceLastShot = 0.0f;

        AgentController agentController = agent.GetComponent<AgentController>();
        agentController.ChangeResourceCount(AgentController.ResourceType.Ammo, -1);

        Debug.Log(agentInputHandler.currentBulletsInMag);
    }
}