using UnityEngine;
using UnityEngine.AI;

public class GnomeAI : MonoBehaviour
{

    [Header("References")]

    private NavMeshAgent gnomeAgent;

    private void Awake()
    {
        gnomeAgent = GetComponent<NavMeshAgent>();
    }


    public void SetDestination(Transform newPosition)
    {
        gnomeAgent.SetDestination(newPosition.position);
    }
}
