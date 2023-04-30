using System;
using UnityEngine;

namespace Project
{
    public class Projectile : MonoBehaviour
    {
        public int damage;

        public void OnCollide(DamageTaker damageTaker)
        {
            if (damageTaker is EnemyHealth health)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Destroy(gameObject, 2f);
        }
    }
}