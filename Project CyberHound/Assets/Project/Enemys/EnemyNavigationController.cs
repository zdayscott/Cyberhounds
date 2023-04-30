using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigationController : MonoBehaviour
{
    public event Action OnReachedDestination;

    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent.isActiveAndEnabled == false || agent.pathPending) return;
        
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.1f)
            {
                CompletePath();
            }
        }
    }

    private void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartNavigation(Vector3 target)
    {
        agent.enabled = true;
        var canPath = agent.SetDestination(target);

        if (canPath == false)
        {
            Debug.Log($"Agent {gameObject.name} cannot navigate to point {target}");
            agent.enabled = false;
        }
    }
    
    private void CompletePath()
    {
        agent.enabled = false;
        OnReachedDestination?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, agent.destination);
    }
}
