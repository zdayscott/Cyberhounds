using System.Linq;
using UnityEngine;

namespace Project
{
    public class ExplosiveProjectile : Projectile
    {
        public float blastRadius;
        public GameObject explosionParticles;
        
        public override void OnCollide(DamageTaker damageTaker)
        {
            Explode();
        }

        public void Explode()
        {
            var position = transform.position;
            if(explosionParticles)
                Instantiate(explosionParticles, position, Quaternion.identity);
            
            var sphereCastAll = Physics.OverlapSphere(position, blastRadius, LayerMask.NameToLayer("Enemy"));
            if(sphereCastAll.Any())
            {
                foreach (var collider1 in sphereCastAll)
                {
                    if(collider1.TryGetComponent(out DamageTaker damageTaker))
                        damageTaker.TakeDamage(damage);
                }
            }
            
            Destroy(gameObject);
        }
    }
}