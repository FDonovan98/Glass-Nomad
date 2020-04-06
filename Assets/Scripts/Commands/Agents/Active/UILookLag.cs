using UnityEngine;

[CreateAssetMenu(fileName = "UILookLag", menuName = "Commands/Active/UILookLag", order = 0)]
public class UILookLag : ActiveCommandObject
{
    protected override void OnEnable()
    {

    }

    public override void RunCommandOnStart(AgentInputHandler agentInputHandler)
    {
        agentInputHandler.runCommandOnCameraMovement += RunCommandOnCameraMovement;
    }

    void RunCommandOnCameraMovement(Vector3 cameraMovement, AgentInputHandler agentInputHandler, AgentValues agentValues)
    {
        if (agentValues.lagUIInX && agentInputHandler.currentUIXLagTime < agentValues.UIXLagTime)
        {
            if (agentInputHandler.currentUIXLagTime < agentValues.UIXLagTime)
            {
                //agentInputHandler.HUD.transform.position -= cameraMovement.x;
                agentInputHandler.currentUIXLagTime += Time.deltaTime;
            }
            else
            {
                //agentInputHandler.HUD.transform.position = Vector3.Lerp(agentInputHandler.HUD.transform.position, Vect)
            }
        }
    }
}