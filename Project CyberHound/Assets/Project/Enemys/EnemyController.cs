using UnityEngine;

[RequireComponent(typeof(EnemyNavigationController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyNavigationController navigationController;
    [SerializeField] private EnemyHealth health;
    private PathPoint _nextPathPoint;

    public void Initialize(SpawnPoint spawnPoint)
    {
        _nextPathPoint = spawnPoint.nextPoint;
        
        navigationController.OnReachedDestination += NavigationControllerOnReachedDestination;
        navigationController.StartNavigation(_nextPathPoint.GetPoint());
        health.OnDie += HealthOnDie;
    }

    private void HealthOnDie()
    {
        Destroy(gameObject);
    }

    private void NavigationControllerOnReachedDestination()
    {
        _nextPathPoint = _nextPathPoint.GetNext();
        
         if(_nextPathPoint)
             navigationController.StartNavigation(_nextPathPoint.GetPoint());
    }

    private void Reset()
    {
        navigationController = GetComponent<EnemyNavigationController>();
    }
}