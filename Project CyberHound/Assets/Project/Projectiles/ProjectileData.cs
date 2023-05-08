using UnityEngine;

namespace Project.Projectiles
{
    [CreateAssetMenu(menuName = "SO/Projectile Data", fileName = "New Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        public Projectile projectile;
        public float firePower;
        public float fireRate;
    }
}
