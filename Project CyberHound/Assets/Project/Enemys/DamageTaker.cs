using Project;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class DamageTaker : MonoBehaviour
{
    public int maxHeath;
    public int currentHealth;

    private bool _isDead;

    public UnityEvent<int> OnHit;
    public UnityEvent OnDie;

    private void Start()
    {
        currentHealth = maxHeath;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collide(collision.collider);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Collide(other);
    }*/

    private void Collide(Collider other)
    {
        if (_isDead) return;

        if (other.TryGetComponent(out Projectile projectile))
        {
            currentHealth -= projectile.damage;
            currentHealth = math.clamp(currentHealth, 0, maxHeath);

            projectile.OnCollide(this);

            OnHit?.Invoke(currentHealth);

            if (currentHealth == 0)
            {
                _isDead = true;
                OnDie?.Invoke();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        currentHealth -= damage;
        currentHealth = math.clamp(currentHealth, 0, maxHeath);
        
        OnHit?.Invoke(currentHealth);

        if (currentHealth == 0)
        {
            _isDead = true;
            OnDie?.Invoke();
        }
    }

}