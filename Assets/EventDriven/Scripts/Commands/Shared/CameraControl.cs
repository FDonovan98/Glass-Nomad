using UnityEngine;

[CreateAssetMenu(fileName = "DefaultCameraMovement", menuName = "Commands/Camera Controls")]
public class CameraControl : CommandObject
{
    protected override void OnEnable()
    {

    }

    public override void Execute(GameObject agent, MovementValues movementValues)
    {
        Vector3 mouseRotationInput = GetMouseInput(agent);

        // Agent Rotation.
        Vector3 agentRotation = new Vector3(0.0f, mouseRotationInput.x, 0.0f);
        agentRotation *= movementValues.mouseSensitivity;
        agent.transform.Rotate(agentRotation);

        // Camera Rotation.
        Camera agentCamera = agent.GetComponentInChildren<Camera>();
        Quaternion cameraTargetRotation = agentCamera.transform.localRotation;

        float cameraRotation = -mouseRotationInput.y * movementValues.mouseSensitivity;
        cameraTargetRotation *= Quaternion.Euler(cameraRotation, 0.0f, 0.0f);
        cameraTargetRotation = ClampRotationAroundXAxis(cameraTargetRotation, movementValues);

        agentCamera.transform.localRotation = cameraTargetRotation;
    }

    private Vector3 GetMouseInput(GameObject agent)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        return new Vector3(mouseX, mouseY, 0);
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q, MovementValues movementValues)
    {
        // Quaternion is 4x4 matrix.
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -movementValues.yRotationClamp, movementValues.yRotationClamp);

        // Updates x.
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
