using System.Collections;
using UnityEngine;

public class SingleShotWeapon : Weapon
{
    protected override IEnumerator FireWeapon()
    {
        timeFiring += Time.deltaTime;

        FireShot();
        
        do
        {
            yield return new WaitForSeconds(1/fireRate);
        } while (isFiring);

        _firingCoroutine = null;    
    }
}