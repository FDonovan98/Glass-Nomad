using UnityEngine;

[CreateAssetMenu(fileName = "DefaultWeaponEffects", menuName = "Commands/Passive/Weapon Effects", order = 0)]
public class WeaponEffects : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnWeaponFired += RunCommandOnWeaponFired;
    }

    void RunCommandOnWeaponFired(AgentInputHandler agentInputHandler)
    {
        PlayGunshot(agentInputHandler);
        MuzzleFlash(agentInputHandler);
    }

    void PlayGunshot(AgentInputHandler agentInputHandler)
    {
       if (agentInputHandler.currentWeapon.weaponSound != null)
        {
            AudioSource weaponAudioSource = agentInputHandler.weaponObject.GetComponent<AudioSource>();

            if (weaponAudioSource == null)
            {
                weaponAudioSource = agentInputHandler.weaponObject.AddComponent(typeof(AudioSource)) as AudioSource;
            }

            weaponAudioSource.clip = agentInputHandler.currentWeapon.weaponSound;
            weaponAudioSource.Play();
        } 
        else
        {
            Debug.LogAssertion(agentInputHandler.currentWeapon.name + " is missing a gunshot sound");
        }
    }

    void MuzzleFlash(AgentInputHandler agentInputHandler)
    {
        if (agentInputHandler.weaponMuzzleFlash != null)
        {
            agentInputHandler.weaponMuzzleFlash.Play();
        }
        else
        {
            Debug.LogAssertion(agentInputHandler.currentWeapon.name + " has no muzzle flash");
        }
    }
}