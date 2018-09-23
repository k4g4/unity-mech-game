﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Weapon : MonoBehaviour
{
    public string wepName = "Untitled";
    public int maxDmg; 
    public int accuracy; //out of 100
    public int weaponType = 0;
    public int burstFire = 10;
    public float fireDelay = 0.1f;
    public int partPos = 0; //0 = LArm, 1 = RArm, 2 = LShoulder, 3 = RShoulder
    UIController uic;

    void Awake()
    {
        uic = FindObjectOfType<UIController>();
    }
    
    //ParticleSystem ps;

	void Start ()
    {
        /*
        switch (weaponType)
        {
            case 0:
                ps = transform.GetChild(0).GetComponent<ParticleSystem>();
                break;
            default:
                break;
        }
        */
    }

    public void Fire(Unit tgt)
    {
        switch(weaponType)
        {
            case 0:
                Timing.RunCoroutine(FireGun(tgt));
                break;
            case 1:
                Timing.RunCoroutine(FireShotgun(tgt));
                break;
            default:
                break;
        }
    }

    IEnumerator<float> FireGun(Unit tgt)
    {
        yield return Timing.WaitForSeconds(2f);
        for (int i=0;i<burstFire;i++)
        {
            foreach(Transform child in transform)
            {
                if (child.GetComponent<ParticleSystem>())
                    child.GetComponent<ParticleSystem>().Emit(1);
            }
            //ps.Emit(1);
            int rand = Random.Range(0, 100);
            if(rand<accuracy)
            {
                int damage = Random.Range(maxDmg / 2, maxDmg);
                tgt.health -= damage;
                uic.Damage(tgt, damage + "");
            }
            else
            {
                uic.Damage(tgt, "miss");
            }
            yield return Timing.WaitForSeconds(fireDelay);
        }
    }

    IEnumerator<float> FireShotgun(Unit tgt)
    {
        yield return Timing.WaitForSeconds(2f);
        transform.GetChild(0).GetComponent<ParticleSystem>().Emit(1); //Muzzle
        transform.GetChild(1).GetComponent<ParticleSystem>().Emit(burstFire); //Emitter
        transform.GetChild(2).GetComponent<ParticleSystem>().Emit(2);
        for (int i = 0; i < burstFire; i++)
        {
            //ps.Emit(1);
            int rand = Random.Range(0, 100);
            if (rand < accuracy)
            {
                int damage = Random.Range(maxDmg / 2, maxDmg);
                tgt.health -= damage;
                uic.Damage(tgt, damage+"");
                Debug.Log("hit");
            }
            else
            {
                uic.Damage(tgt, "miss");
            }
            yield return Timing.WaitForSeconds(fireDelay);
        }
    }
}
