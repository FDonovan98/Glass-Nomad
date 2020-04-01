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
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
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
                if (agentInputHandler.currentBulletsInMag > 0)
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
            Debug.Log("runCommandOnWeaponFired");
            agentInputHandler.runCommandOnWeaponFired(agentInputHandler);
        }

        agentInputHandler.timeSinceLastShot = 0.0f;

        AgentController agentController = agent.GetComponent<AgentController>();
        agentController.ChangeResourceCount(AgentController.ResourceType.Ammo, -1);

        Debug.Log(agentInputHandler.currentBulletsInMag);

        MonoBehaviourPunCallbacks agentMonoPun = agent.GetComponent<MonoBehaviourPunCallbacks>();
        
        agentMonoPun.photonView.RPC("Shoot", RpcTarget.All, agentInputHandler.agentCamera.transform.position, agentInputHandler.agentCamera.transform.forward, agentInputHandler.currentWeapon.range, agentInputHandler.currentWeapon.damage);
    }

    [PunRPC]
    public static void Shoot(Vector3 cameraPos, Vector3 cameraForward, float weaponRange, int weaponDamage)
    {     
        Debug.Log("shoot");
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraForward, out hit, weaponRange))
        {
            Debug.Log(hit.transform.gameObject.name + " has been hit");
            AgentInputHandler targetAgentInputHandler = hit.transform.gameObject.GetComponent<AgentInputHandler>();

            Debug.Log(targetAgentInputHandler);
            
            if (targetAgentInputHandler.runCommandOnAgentHasBeenHit != null)
            {
                Debug.Log("hit feedback");
                targetAgentInputHandler.runCommandOnAgentHasBeenHit(targetAgentInputHandler, hit.point, weaponDamage);
            }
        }
    }
}