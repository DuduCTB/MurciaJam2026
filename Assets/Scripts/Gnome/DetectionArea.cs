using UnityEngine;

public class DetectionArea : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RunAwayDirector.instance.GoNextStateAndExecute();
            Destroy(gameObject);
        }
    }
}
