using System;
using Project;
using Unity.Mathematics;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    [SerializeField] private int maxHeath;
    private int _currentHealth;

    private bool _isDead;

    public event Action OnDie;

    private void OnCollisionEnter(Collision collision)
    {
        Collide(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collide(other);
    }

    private void Collide(Collider other)
    {
        Debug.Log($"Trigger enter! {other.gameObject.name}");

        if (_isDead) return;

        if (other.TryGetComponent(out Projectile projectile))
        {
            _currentHealth -= projectile.damage;
            _currentHealth = math.clamp(_currentHealth, 0, maxHeath);

            projectile.OnCollide(this);

            if (_currentHealth == 0)
            {
                _isDead = true;
                OnDie?.Invoke();
            }
        }
    }
}