using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "DefaultWeaponEffects", menuName = "Commands/Passive/Weapon Effects", order = 0)]
public class WeaponEffects : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnWeaponFired += RunCommandOnWeaponFired;
    }

    private void RunCommandOnWeaponFired(AgentInputHandler agentInputHandler)
    {
        PhotonView agentsPhotonView = agentInputHandler.GetComponent<PhotonView>();
        agentsPhotonView.RPC("PlayGunshot", RpcTarget.All, agentsPhotonView.ViewID);
        agentsPhotonView.RPC("MuzzleFlash", RpcTarget.All, agentsPhotonView.ViewID);
    }
}