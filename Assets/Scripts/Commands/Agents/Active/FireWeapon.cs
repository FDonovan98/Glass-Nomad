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
        if (Physics.Raycast(agentInputHandler.agentCamera.transform.position, WeaponDirection(agentInputHandler.agentCamera.transform.forward, agentInputHandler), out hit, agentInputHandler.currentWeapon.range))
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

    Vector3 WeaponDirection(Vector3 originalDirection, AgentInputHandler agentInputHandler)
    {
        return CalculateWeaponSpread(originalDirection, agentInputHandler);
    }

    Vector3 CalculateWeaponSpread(Vector3 direction, AgentInputHandler agentInputHandler)
    {
        float theta = Mathf.Deg2Rad;
        
        if (!agentInputHandler.isADS)
        {
            theta *= agentInputHandler.currentRecoilValue * agentInputHandler.currentWeapon.maxSpreadAngle;

        }
        else
        {
            theta *= agentInputHandler.currentRecoilValue * agentInputHandler.currentWeapon.maxADSSpreadAngle;
        }
            
        // float[] rand = 
        // {
        //     Random.Range(0.0f, 1.0f),
        //     Random.Range(0.0f, 1.0f)
        // };

        // rand[0] *= theta;
        // rand[1] *= theta;

        // RotateX(ref direction, rand[0]);
        // RotateY(ref direction, rand[1]);
        RotateX(ref direction, theta);
        RotateY(ref direction, theta);

        return direction;
    }

    // Source code from: https://forum.unity.com/threads/vector-rotation.33215/
    public static void RotateX(ref Vector3 v, float angle )
    {
        float sin = Mathf.Sin( angle );
        float cos = Mathf.Cos( angle );
       
        float ty = v.y;
        float tz = v.z;
        v.y = (cos * ty) - (sin * tz);
        v.z = (cos * tz) + (sin * ty);
    }
   
    public static void RotateY(ref Vector3 v, float angle )
    {
        float sin = Mathf.Sin( angle );
        float cos = Mathf.Cos( angle );
       
        float tx = v.x;
        float tz = v.z;
        v.x = (cos * tx) + (sin * tz);
        v.z = (cos * tz) - (sin * tx);
    }
}