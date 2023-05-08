using System;
using UnityEngine;

namespace Project.Game_Entities.Enemies
{
    public abstract class DetectionModule : MonoBehaviour
    {
        public event Action<GameEntity> OnEnterDetection;
        public event Action<GameEntity> OnExitDetection;
        private SphereCollider _sphereCollider;
        [SerializeField] private float radius = 5;

        private void Start()
        {
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = radius;
        }

        protected abstract bool FilterDetectedEntities(GameEntity entity);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out GameEntity entity))
            {
                if (FilterDetectedEntities(entity))
                {
                    OnEnterDetection?.Invoke(entity);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out GameEntity entity))
            {
                if (FilterDetectedEntities(entity))
                {
                    OnExitDetection?.Invoke(entity);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
