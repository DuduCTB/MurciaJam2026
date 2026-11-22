using UnityEngine;
using UnityEngine.InputSystem;

public class MusicDebug : MonoBehaviour
{
    void Update()
    {
        var k = Keyboard.current;
        if (k == null) return;
        if (k.digit1Key.wasPressedThisFrame) MusicManager.Instance.PlayCalm();
        if (k.digit2Key.wasPressedThisFrame) MusicManager.Instance.PlayEmergency();
        if (k.digit3Key.wasPressedThisFrame) MusicManager.Instance.PlayUneasy();
    }
}
