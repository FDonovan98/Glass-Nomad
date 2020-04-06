using UnityEngine;

[CreateAssetMenu(fileName = "UILookLag", menuName = "Commands/Passive/UILookLag", order = 0)]
public class UILookLag : PassiveCommandObject
{
    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnCameraMovement += RunCommandOnCameraMovement;
    }

    void RunCommandOnCameraMovement(Vector3 cameraMovement, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        for (int i = 0; i < 2; i++)
        {
            if (agentValues.lagUIInAxis[i])
            {
                if (i == 0)
                {
                    agentInputHandler.UIOffset.x -= cameraMovement[i];
                }
                else
                {
                    agentInputHandler.UIOffset.y -= cameraMovement[i];
                }

                if (i == 0)
                {
                    agentInputHandler.UIOffset.x = Mathf.Lerp(agentInputHandler.UIOffset.x, 0.0f, agentValues.UICatchupSpeed[i]);
                }
                else
                {
                    agentInputHandler.UIOffset.y = Mathf.Lerp(agentInputHandler.UIOffset.y, 0.0f, agentValues.UICatchupSpeed[i]);
                }
                
                SetHudPosition(agentInputHandler);
            }
        }
    }

    void SetHudPosition(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.HUD.transform.position = agentInputHandler.HUD.transform.parent.position + agentInputHandler.UIOffset;
    }
}