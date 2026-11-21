using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    // El delta del Input System viene en píxeles; este factor lo acerca a la escala del Input Manager antiguo
    private const float DeltaScale = 0.1f;

  

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue() * DeltaScale;

        float mouseX = delta.x * GameContext.instance.mouseSensibility;
        float mouseY = delta.y * GameContext.instance.mouseSensibility;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
