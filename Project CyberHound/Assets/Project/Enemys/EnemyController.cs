using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyNavigationController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyNavigationController navigationController;
    private PathPoint _nextPathPoint;

    public void Initialize(SpawnPoint spawnPoint)
    {
        _nextPathPoint = spawnPoint.nextPoint;
        
        navigationController.OnReachedDestination += NavigationControllerOnReachedDestination;
        navigationController.StartNavigation(_nextPathPoint.GetPoint());
    }

    Coroutine _hitFlashCoroutine;
    [SerializeField] private float maxFlashTime = .1f;
    private float _currentFlashTime;
    [SerializeField] private float flashIntensity = 10;
    [SerializeField] private Renderer[] renderers;
    public void OnHit()
    {
        _currentFlashTime = maxFlashTime;
        
        if(_hitFlashCoroutine == null)
            _hitFlashCoroutine = StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash()
    {
        do
        {
            _currentFlashTime -= Time.deltaTime;

            var lerpTime = _currentFlashTime / maxFlashTime;
            var intensity = (lerpTime * flashIntensity);

            foreach (var render in renderers)
            {
                render.material.color = Color.white * intensity;
            }

            yield return 0;
        } while (_currentFlashTime > 0);

        _hitFlashCoroutine = null;
    }

    public void OnDie()
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