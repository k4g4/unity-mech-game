using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Weapon : MonoBehaviour
{
    public int maxDmg; 
    public int accuracy; //out of 100
    public int weaponType = 0;
    public int burstFire = 10;
    public float fireDelay = 0.1f;

    ParticleSystem ps;

	void Start ()
    {
        switch (weaponType)
        {
            case 0:
                ps = transform.GetChild(0).GetComponent<ParticleSystem>();
                break;
            default:
                break;
        }
    }

    public void Fire(Unit tgt)
    {
        switch(weaponType)
        {
            case 0:
                Timing.RunCoroutine(FireGun(tgt));
                break;
            default:
                break;
        }
    }

    IEnumerator<float> FireGun(Unit tgt)
    {
        yield return Timing.WaitForSeconds(1f);
        for (int i=0;i<burstFire;i++)
        {
            ps.Emit(1);
            int rand = Random.Range(0, 100);
            if(rand<accuracy)
            {
                tgt.health -= Random.Range(maxDmg / 2, maxDmg);
                Debug.Log("hit");
            }
            yield return Timing.WaitForSeconds(fireDelay);
        }
    }
}
