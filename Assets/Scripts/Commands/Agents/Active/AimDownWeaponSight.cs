using UnityEngine;

[CreateAssetMenu(fileName = "AimDownWeaponSight", menuName = "Commands/Active/Aim Down Weapon Sight", order = 0)]
public class AimDownWeaponSight : ActiveCommandObject
{
    [SerializeField]
    KeyCode aimDownSight = KeyCode.Mouse1;

    protected override void OnEnable()
    {
        keyTable.Add("Aim Down Sight", aimDownSight);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (agentValues.aDSIsAToggle)
        {
            if (Input.GetKeyDown(aimDownSight))
            {
                if (agentInputHandler.aDSCamera.enabled == true)
                {
                    DisableADS(agentInputHandler);
                }
                else
                {
                    EnableADS(agentInputHandler);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(aimDownSight))
            {
                EnableADS(agentInputHandler);
            }
            else if (Input.GetKeyUp(aimDownSight))
            {
                DisableADS(agentInputHandler);
            }
        }
    }

    void EnableADS(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.mainCamera.enabled = false;
        agentInputHandler.aDSCamera.enabled = true;
        agentInputHandler.agentCamera = agentInputHandler.aDSCamera;
    }

    void DisableADS(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.aDSCamera.enabled = false;
        agentInputHandler.mainCamera.enabled = true;
        agentInputHandler.agentCamera = agentInputHandler.mainCamera;
    }
}