using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] Transform playerBody;   
    [SerializeField] float sensitivity = 2f;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Left/right: turn the whole player
        playerBody.Rotate(Vector3.up * mouseX);
    }
}