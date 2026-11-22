using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    public AudioClip[] metalSteps;
    public AudioClip[] dirtSteps;
    public float stepDistance = 1.8f;   // metros entre pasos
    public float rayLength = 2f;

    AudioSource source;
    Vector3 lastPos;
    float accumulated;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f;        // 2D: son tus pasos
        lastPos = transform.position;
    }

    void Update()
    {
        Vector3 delta = transform.position - lastPos;
        delta.y = 0f;
        lastPos = transform.position;

        if (!Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down,
                out RaycastHit hit, rayLength, ~0, QueryTriggerInteraction.Ignore))
        {
            accumulated = 0f;            // en el aire
            return;
        }

        accumulated += delta.magnitude;
        if (accumulated < stepDistance) return;
        accumulated = 0f;

        AudioClip[] set = hit.collider.CompareTag("Metal") ? metalSteps : dirtSteps;
        if (set.Length == 0) return;

        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(set[Random.Range(0, set.Length)], Random.Range(0.8f, 1f));
    }
}