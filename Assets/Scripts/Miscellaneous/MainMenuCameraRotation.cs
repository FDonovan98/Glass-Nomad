using UnityEngine;

public class MainMenuCameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;

    private void Update()
    {
        // Constantly rotates the camera, while you are in the main menu.
        transform.localEulerAngles += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
