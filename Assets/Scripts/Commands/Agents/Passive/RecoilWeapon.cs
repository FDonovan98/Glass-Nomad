using UnityEngine;

[CreateAssetMenu(fileName = "DefaultRecoilWeapon", menuName = "Commands/Passive/RecoilWeapon", order = 0)]
public class RecoilWeapon : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnWeaponFired += RunCommandOnWeaponFired;
        agentInputHandler.runCommandOnUpdate += RunCommandOnUpdate;
    }

    void RunCommandOnWeaponFired(AgentInputHandler agentInputHandler)
    {
        Debug.Log("recoil");
        float timeDelta;
        timeDelta = agentInputHandler.currentWeapon.upForceStep;
        HandleRecoil(agentInputHandler, timeDelta);
    }

    void RunCommandOnUpdate(GameObject agent, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        float timeDelta;
        timeDelta = -Time.deltaTime / agentInputHandler.currentWeapon.downForceDuration;

        HandleRecoil(agentInputHandler, timeDelta);
    }

    void HandleRecoil(AgentInputHandler agentInputHandler, float timeDelta)
    {

        AnimationCurve weaponRecoilCurveUp = agentInputHandler.currentWeapon.recoilCurveUp;
        AnimationCurve weaponRecoilCurveDown = agentInputHandler.currentWeapon.recoilCurveDown;

        float valueDelta;

        if (timeDelta > 0)
        {
            valueDelta = weaponRecoilCurveUp.Evaluate(agentInputHandler.currentRecoilValue + timeDelta) - weaponRecoilCurveUp.Evaluate(agentInputHandler.currentRecoilValue);
        }
        else
        {
            valueDelta = weaponRecoilCurveDown.Evaluate(agentInputHandler.currentRecoilValue + timeDelta) - weaponRecoilCurveDown.Evaluate(agentInputHandler.currentRecoilValue);
        }


        valueDelta *= -agentInputHandler.currentWeapon.recoilForce;

        agentInputHandler.agentCamera.transform.Rotate(valueDelta, 0.0f, 0.0f);

        // Prevents index errors.
        agentInputHandler.currentRecoilValue += timeDelta;
        agentInputHandler.currentRecoilValue = Mathf.Clamp(agentInputHandler.currentRecoilValue, 0.0f, 1.0f - agentInputHandler.currentWeapon.upForceStep);
    }
}