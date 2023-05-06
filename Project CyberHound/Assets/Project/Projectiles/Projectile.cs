﻿using System;
using UnityEngine;

namespace Project
{
    public class Projectile : MonoBehaviour
    {
        public int damage;

        public virtual void OnCollide(DamageTaker damageTaker)
        {
            if (damageTaker is EnemyHealth health)
            {
                Destroy(gameObject);
            }
            else
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