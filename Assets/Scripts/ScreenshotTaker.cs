using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenshotTaker : MonoBehaviour
{
    public int superSize = 1;   // 2 = el doble de resolución

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.f12Key.wasPressedThisFrame)
        {
            string file = "Captura_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            ScreenCapture.CaptureScreenshot(file, superSize);
            Debug.Log("Captura guardada: " + file);
        }
    }
}

