using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [SerializeField] private bool onlyDetectLooks = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !onlyDetectLooks)
        {
            RunAwayDirector.instance.GoNextStateAndExecute();
            Destroy(gameObject);
        }
    }
}
