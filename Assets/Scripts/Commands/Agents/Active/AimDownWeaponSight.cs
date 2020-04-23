using UnityEngine;

[CreateAssetMenu(fileName = "AimDownWeaponSight", menuName = "Commands/Active/Aim Down Weapon Sight", order = 0)]
public class AimDownWeaponSight : ActiveCommandObject
{
    [SerializeField]
    private string aimDownSight = "Fire2";

    protected override void OnEnable()
    {
        keyTable.Add("Aim Down Sight", aimDownSight);
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
        if (agentValues.aDSIsAToggle)
        {
            if (Input.GetAxis(aimDownSight) > 0)
            {
                if (agentInputHandler.aDSCamera.enabled == true)
                {
                    ToggleADS(agentInputHandler, false);
                }
                else
                {
                    ToggleADS(agentInputHandler, true);
                }
            }
        }
        else
        {
            if (Input.GetAxis(aimDownSight) > 0)
            {
                ToggleADS(agentInputHandler, true);
            }
            else if (Input.GetAxis(aimDownSight) <= 0))
            {
                ToggleADS(agentInputHandler, false);
            }
        }
    }

    void ToggleADS(AgentInputHandler agentInputHandler, bool toggle)
    {
        agentInputHandler.isADS = toggle;

        if (toggle)
        {
            agentInputHandler.agentCamera = agentInputHandler.aDSCamera;
        }
        else
        {
            agentInputHandler.agentCamera = agentInputHandler.mainCamera;
        }

        agentInputHandler.HUDCanvas.worldCamera = agentInputHandler.agentCamera;

        agentInputHandler.mainCamera.enabled = !toggle;
        agentInputHandler.aDSCamera.enabled = toggle;   
    }
}