using UnityEngine;
using UnityEngine.AI;

public class GnomeAI : MonoBehaviour
{

    [Header("References")]

    public NavMeshAgent gnomeAgent;

    public Pickable pickGnome;

    private void Awake()
    {
        gnomeAgent = GetComponent<NavMeshAgent>();
    }


    public void SetDestination(Transform newPosition)
    {
        gnomeAgent.SetDestination(newPosition.position);
    }

    public void TeleportGnome(Transform newPosition)
    {
        gnomeAgent.enabled = false;
        transform.position = newPosition.position;
        gnomeAgent.enabled = true;
    }

    public void ClearDestination()
    {
        gnomeAgent.ResetPath();
    }

}
