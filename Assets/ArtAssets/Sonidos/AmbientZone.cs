using UnityEngine;
using UnityEngine.Audio;

public class AmbientZone : MonoBehaviour
{
    public AudioMixerSnapshot interior;
    public AudioMixerSnapshot exterior;
    public float fadeTime = 1.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) interior.TransitionTo(fadeTime);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) exterior.TransitionTo(fadeTime);
    }
}
