using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [Header("Projectile Logic")] 
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] protected float fireRate = 30;
    [SerializeField] private float firePower = 500f;
    [SerializeField] private Vector2 recoil = new Vector2(0.2f, 0.2f);
    [SerializeField] protected float timeToMaxRecoil = 1;

    protected Coroutine _firingCoroutine;
    protected bool isFiring;
    protected float timeFiring = 0f;
    
    private void Update()
    {
        if (!isFiring && timeFiring > 0)
            timeFiring -= Time.deltaTime * 2;
    }
    
    public void FireShot()
    {
        var go = Instantiate(projectile, firePoint.position, firePoint.rotation);

        if (go.TryGetComponent(out Rigidbody pRigidbody))
        {
            var currentRecoil = timeFiring / timeToMaxRecoil;
            var horizontal = Mathf.Lerp(0, recoil.x, currentRecoil);
            var vertical = Mathf.Lerp(0, recoil.y, currentRecoil);
            
            var randomVariance = new Vector2(Random.Range(-1 * horizontal, horizontal),Random.Range(-1 * vertical, vertical));
            pRigidbody.AddForce((firePoint.forward + firePoint.right * randomVariance.x + firePoint.up * randomVariance.y) * firePower, ForceMode.VelocityChange);
        }
    }
    
    protected virtual IEnumerator FireWeapon()
    {
        do
        {
            FireShot();
            timeFiring += Time.deltaTime;
            timeFiring = Mathf.Clamp(timeFiring, 0, timeToMaxRecoil);
            
            yield return new WaitForSeconds(1/fireRate);
        } while (isFiring);

        _firingCoroutine = null;
    }

    public void StartFiring()
    {
        isFiring = true;
        if (_firingCoroutine == null)
            _firingCoroutine = StartCoroutine(FireWeapon());
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}