using UnityEngine;

public class PlayerDetectGnome : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask targetLayer;

    private void Update()
    {
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, distance, targetLayer))
        {
            OnTargetDetected(hit);
        }
    }

    private void OnTargetDetected(RaycastHit hit)
    {
        hit.collider.gameObject.SetActive(false);
        RunAwayDirector.instance.GoNextStateAndExecute();
    }

    private void OnDrawGizmosSelected()
    {
        if (head == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(head.position, head.forward * distance);
    }
}
