using System.Collections;
using UnityEngine;

public class BurstWeapon : Weapon
{
    [SerializeField]
    private int shotsPerBurst = 3;
    protected override IEnumerator FireWeapon()
    {
        timeFiring += Time.deltaTime;

        for (int i = 0; i < shotsPerBurst; i++)
        {
            timeFiring += Time.deltaTime;
            FireShot();
            yield return new WaitForSeconds(1/fireRate);
        }
        
        do
        {
            yield return new WaitForSeconds(1/fireRate);
        } while (isFiring);

        _firingCoroutine = null;    
    }
}