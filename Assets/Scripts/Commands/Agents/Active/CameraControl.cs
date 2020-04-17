using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCameraMovement", menuName = "Commands/Active/Camera Controls")]
public class CameraControl : ActiveCommandObject
{
    protected override void OnEnable()
    {

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
        if (agentInputHandler.allowInput)
        {
            Vector3 mouseRotationInput = GetMouseInput(agent);


            if (mouseRotationInput != Vector3.zero)
            {
                mouseRotationInput *= agentValues.mouseSensitivity;
              
                if (agentInputHandler.runCommandOnCameraMovement != null)
                {
                    agentInputHandler.runCommandOnCameraMovement(mouseRotationInput, agentInputHandler, agentValues);
                }

                // Agent Rotation.
                Vector3 agentRotation = new Vector3(0.0f, mouseRotationInput.x, 0.0f);
                agent.transform.Rotate(agentRotation);

                // Camera Rotation.
                Quaternion cameraTargetRotation = agentInputHandler.mainCamera.transform.localRotation;

                float cameraRotation = -mouseRotationInput.y;
                cameraTargetRotation *= Quaternion.Euler(cameraRotation, 0.0f, 0.0f);
                cameraTargetRotation = ClampRotationAroundXAxis(cameraTargetRotation, agentValues);

                agentInputHandler.mainCamera.transform.localRotation = cameraTargetRotation;
            }
        }
    }

    private Vector3 GetMouseInput(GameObject agent)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        return new Vector3(mouseX, mouseY, 0);
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q, AgentValues agentValues)
    {
        // Quaternion is 4x4 matrix.
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -agentValues.yRotationClamp, agentValues.yRotationClamp);

        // Updates x.
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
