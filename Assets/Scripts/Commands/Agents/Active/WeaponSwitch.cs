using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSwitchWeapon", menuName = "Commands/Active/SwitchWeapon", order = 0)]
public class WeaponSwitch : ActiveCommandObject
{
    [SerializeField]
    KeyCode primaryWeaponKey = KeyCode.Alpha1;
    [SerializeField]
    KeyCode secondaryWeaponKey = KeyCode.Alpha2;

    protected override void OnEnable()
    {
        keyTable.Add("Select Primary Weapon", primaryWeaponKey);
        keyTable.Add("Select Secondary Weapon", secondaryWeaponKey);
    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    private void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (Input.GetKeyDown(primaryWeaponKey))
        {
            if (agentInputHandler.currentWeaponID != 0)
            {
                agentInputHandler.currentWeaponID = 0;
                SwitchWeapon(agentInputHandler);
            }
        }
        else if (Input.GetKeyDown(secondaryWeaponKey))
        {
            if (agentInputHandler.currentWeaponID != 1)
            {
                agentInputHandler.currentWeaponID = 1;
                SwitchWeapon(agentInputHandler);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            agentInputHandler.currentWeaponID += 1;
            if (agentInputHandler.currentWeaponID >= agentInputHandler.equipedWeapons.Length)
            {
                agentInputHandler.currentWeaponID = 0;
            }
            SwitchWeapon(agentInputHandler);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            agentInputHandler.currentWeaponID -= 1;
            if (agentInputHandler.currentWeaponID < 0)
            {
                agentInputHandler.currentWeaponID = agentInputHandler.equipedWeapons.Length - 1;
            }
            SwitchWeapon(agentInputHandler);
        }
    }

    private void SwitchWeapon(AgentInputHandler agentInputHandler)
    {
        MeshFilter mesh;
        mesh = agentInputHandler.weaponObject.GetComponent<MeshFilter>();

        agentInputHandler.currentWeapon = agentInputHandler.equipedWeapons[agentInputHandler.currentWeaponID];
        if (mesh != null && agentInputHandler.currentWeapon.weaponMesh != null)
        {
            mesh.mesh = agentInputHandler.currentWeapon.weaponMesh;
        }
        else
        {
            Debug.LogWarning("CurrentWeapon scriptable object is missing it's mesh.");
        }
    }
}
