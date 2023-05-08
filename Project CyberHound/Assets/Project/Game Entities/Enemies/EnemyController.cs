using System;
using System.Collections;
using Project.Projectiles;
using UnityEngine;

namespace Project.Game_Entities.Enemies
{
    [RequireComponent(typeof(EnemyNavigationController))]
    public class EnemyController : GameEntity
    {
        [SerializeField] private EnemyNavigationController navigationController;
        private PathPoint _nextPathPoint;

        [SerializeField] private EnemyDetectionModule detectionModule;
        
        [SerializeField] private Transform cannonHead;
        [SerializeField] private Transform firePoint;
        [SerializeField] private ProjectileData projectileData;
        [SerializeField] private float cannonRotationSpeed;
        private float _fireRateCooldown;

        public void Initialize(SpawnPoint spawnPoint)
        {
            _nextPathPoint = spawnPoint.nextPoint;
        
            navigationController.OnReachedDestination += NavigationControllerOnReachedDestination;
            navigationController.StartNavigation(_nextPathPoint.GetPoint());
        }

        private void Start()
        {
            detectionModule.OnEnterDetection += DetectionModuleOnEnterDetection;
            detectionModule.OnExitDetection += DetectionModuleOnExitDetection;
        }

        private void FixedUpdate()
        {
            if (target)
            {
                AimAtTarget();

                if (_fireRateCooldown <= 0)
                {
                    if (IsTargeting())
                        FireAtTarget();
                }
                else
                {
                    _fireRateCooldown -= Time.deltaTime;
                }
            }
        }

        private void FireAtTarget()
        {
            var newProjectile = Instantiate(projectileData.projectile, firePoint.position, firePoint.rotation);
            var rb = newProjectile.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward*projectileData.firePower, ForceMode.VelocityChange);

            _fireRateCooldown = 1/projectileData.fireRate;
        }

        private bool IsTargeting()
        {
            var lookDir = target.transform.position - cannonHead.position;
            return Vector3.Dot(firePoint.forward, lookDir.normalized) >= .7f;
        }

        private void AimAtTarget()
        {
            var lookDir = target.transform.position - cannonHead.position;
            lookDir.Normalize();
            
            cannonHead.rotation = Quaternion.Slerp(cannonHead.rotation, Quaternion.LookRotation(lookDir), cannonRotationSpeed * Time.fixedDeltaTime);
        }

        private GameEntity target;

        private void DetectionModuleOnEnterDetection(GameEntity gameEntity)
        {
            target = gameEntity;
        }

        private void DetectionModuleOnExitDetection(GameEntity gameEntity)
        {
            if (target == gameEntity)
                target = null;
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
}