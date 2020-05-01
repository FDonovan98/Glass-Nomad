using UnityEngine;
using Photon.Pun;

using System.Linq;
using System.Collections.Generic;

public class PunRPCs : MonoBehaviourPunCallbacks
{
    private AgentInputHandler GetInputHandler(int viewId)
    {
        return PhotonNetwork.GetPhotonView(viewId).GetComponent<AgentController>();
    }

    [PunRPC]
    public void WallWasHit(Vector3 cameraPos, Vector3 cameraForward, float weaponRange, int weaponDamage)
    {     
        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraForward, out hit, weaponRange))
        {
            AgentInputHandler targetAgentInputHandler = hit.transform.gameObject.GetComponent<AgentInputHandler>();

            Debug.Log(targetAgentInputHandler);
            
            if (targetAgentInputHandler != null)
            {
                if (targetAgentInputHandler.runCommandOnAgentHasBeenHit != null)
                {
                    targetAgentInputHandler.runCommandOnAgentHasBeenHit(targetAgentInputHandler, hit.point, hit.normal, weaponDamage);
                }
            }
            else
            {
                Debug.LogWarning(hit.transform.gameObject.name + " has no agentInputHandler");
            }
        }
    }

    [PunRPC]
    public void PlayerWasHit(int hitPlayerViewID, Vector3 hitPos, Vector3 hitNormal, int weaponDamage)
    {
        AgentInputHandler targetAgentInputHandler = GetInputHandler(hitPlayerViewID);

        if (targetAgentInputHandler != null)
        {
            if (targetAgentInputHandler.runCommandOnAgentHasBeenHit != null)
                {
                    targetAgentInputHandler.runCommandOnAgentHasBeenHit(targetAgentInputHandler, hitPos, hitNormal, weaponDamage);
                }
        }
        else
        {
            Debug.LogWarning(targetAgentInputHandler.gameObject.name + " has no agentInputHandler");
        }
    }

    [PunRPC]
    public void Toggle(int playersViewID)
    {
        AgentInputHandler agentInputHandler = GetInputHandler(playersViewID);
        agentInputHandler.behaviourToToggle.enabled = !agentInputHandler.behaviourToToggle.isActiveAndEnabled;

        if (agentInputHandler.toggleOnSound != null && agentInputHandler.mainAudioSource != null)
        {
            if (agentInputHandler.behaviourToToggle.isActiveAndEnabled)
            {
                agentInputHandler.mainAudioSource.PlayOneShot(agentInputHandler.toggleOnSound);
            }
            else
            {
                agentInputHandler.mainAudioSource.PlayOneShot(agentInputHandler.toggleOffSound);
            }
        }
    }

    [PunRPC]
    public void EmergencyRegenSmoke(int playersViewID)
    {
        AgentController agentController = PhotonNetwork.GetPhotonView(playersViewID).GetComponent<AgentController>();

        if (agentController.emergencyRegenActive)
        {
            agentController.emergencyRegenParticleSystems = Instantiate(agentController.emergencyRegenParticleSystem, agentController.agent.transform.position, Quaternion.identity, agentController.agent.transform);
        }
        else
        {
            Destroy(agentController.emergencyRegenParticleSystems);
        }
    }
    
    [PunRPC]
    public void PlayGunshot(int agentsViewID)
    {
        AgentInputHandler agentInputHandler = GetInputHandler(agentsViewID);
        Debug.Log("PlayGunshot: Sending to all.");
        if (agentInputHandler.currentWeapon.weaponSound != null)
        {
            AudioSource weaponAudioSource = agentInputHandler.weaponObject.GetComponentInChildren<AudioSource>();

            if (weaponAudioSource == null)
            {
                weaponAudioSource = agentInputHandler.weaponObject.AddComponent(typeof(AudioSource)) as AudioSource;
            }

            weaponAudioSource.PlayOneShot(agentInputHandler.currentWeapon.weaponSound);
        } 
        else
        {
            Debug.LogAssertion(agentInputHandler.currentWeapon.name + " is missing a gunshot sound");
        }
    }

    [PunRPC]
    public void MuzzleFlash(int agentsViewID)
    {
        AgentInputHandler agentInputHandler = GetInputHandler(agentsViewID);
        Debug.Log("MuzzleFlash: Sending to all.");
        if (agentInputHandler.weaponMuzzleFlash != null)
        {
            agentInputHandler.weaponMuzzleFlash.Play();
        }
        else
        {
            Debug.LogAssertion(agentInputHandler.currentWeapon.name + " has no muzzle flash");
        }
    }

    [PunRPC]
    public void PlayFootstep(int agentsViewID)
    {
        AgentInputHandler agentInputHandler = GetInputHandler(agentsViewID);
        if (agentInputHandler.footstepSource == null || agentInputHandler.footstepSource.isPlaying) return;
        agentInputHandler.footstepSource.clip = agentInputHandler.GetRandomFootstepClip();
        agentInputHandler.footstepSource.Play();
    }

    [PunRPC]
    public void CancelFootstep(int agentsViewID)
    {
        AgentInputHandler agentInputHandler = GetInputHandler(agentsViewID);
        agentInputHandler.footstepSource.Stop();
        agentInputHandler.footstepSource.clip = null;
    }

    [PunRPC]
    public void ChangeMaterial(int agentsViewID, int materialIndex)
    {
        AgentInputHandler agentInputHandler = GetInputHandler(agentsViewID);

        if (agentInputHandler.agentRenderer != null)
        {
            Material[] materials = Resources.LoadAll("Items", typeof(Material)).Cast<Material>().ToArray();
            List<Material> materialItems = new List<Material>();
            AddMaterialsToLists(ref materialItems, materials);

            agentInputHandler.agentRenderer.material = materialItems[materialIndex];
        }
    }

    void AddMaterialsToLists(ref List<Material> materialItems, Material[] materials)
    {
        foreach (Material element in materials)
        {
            materialItems.Add(element);
        }
    }
}