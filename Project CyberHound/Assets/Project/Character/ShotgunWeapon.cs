using System.Collections;
using UnityEngine;

public class ShotgunWeapon : Weapon
{
    [SerializeField]
    private int shotsPerBurst = 12;
    protected override IEnumerator FireWeapon()
    {
        timeFiring = timeToMaxRecoil;
        for (var i = 0; i < 12; i++)
        {
            FireShot();
        }
        
        do
        {
            yield return new WaitForSeconds(1/fireRate);
        } while (isFiring);

        _firingCoroutine = null;    
    }
}