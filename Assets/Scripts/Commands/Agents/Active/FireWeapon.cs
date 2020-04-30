using UnityEngine;

using Photon.Pun;

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

        if (agentInputHandler.allowInput && !agentController.isReloading)
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

        agentController.ChangeStat(ResourceType.MagazineAmmo, -1);

        FireWeaponOverNet(agentInputHandler);
    }
    
    private void FireWeaponOverNet(AgentInputHandler agentInputHandler)
    {
        RaycastHit hit;
        if (Physics.Raycast(agentInputHandler.agentCamera.transform.position, agentInputHandler.agentCamera.transform.forward, out hit, agentInputHandler.currentWeapon.range))
        {
            if (hit.transform.tag == "Player")
            {
                int targetPhotonID = hit.transform.GetComponent<PhotonView>().ViewID;

                agentInputHandler.photonView.RPC("PlayerWasHit", RpcTarget.All, targetPhotonID, hit.point, hit.normal, agentInputHandler.currentWeapon.damage);
            }
            else
            {          
                agentInputHandler.photonView.RPC("WallWasHit", RpcTarget.All, agentInputHandler.agentCamera.transform.position, agentInputHandler.agentCamera.transform.forward, agentInputHandler.currentWeapon.range, agentInputHandler.currentWeapon.damage);
            }
        }
    }
}