using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Weapon : MonoBehaviour
{
    public GameObject missile;
    public GameObject hitFX;
    public string wepName = "Untitled";
    public int weaponID;
    public int maxDmg; 
    public int accuracy; //out of 100
    public int weaponType = 0;
    public int burstFire = 10;
    public float fireDelay = 0.1f;
    public int range = 10;
    public int partPos = 0; //0 = LArm, 1 = RArm, 2 = LShoulder, 3 = RShoulder
    public float hitOffset = 0.5f;
    public int apCost = 10;
    UIController uic;
    PlayerController pc;
    CameraController cc;
    AudioSource audSoc;
    public AudioClip shootSFX;

    void Awake()
    {
        uic = FindObjectOfType<UIController>();
        pc = FindObjectOfType<PlayerController>();
        cc = FindObjectOfType<CameraController>();
        audSoc = GetComponent<AudioSource>();
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
            case 2:
                Timing.RunCoroutine(FireGuidedMissile(tgt));
                break;
            default:
                break;
        }
    }

    public int AccuracyCalc(Unit tgt)
    {
        int ans = Mathf.RoundToInt(accuracy + (range - Vector3.Distance(tgt.transform.position,transform.position))*2);
        return ans;
    }

    public int AccuracyCalc(Vector3 pos,Vector3 targetPos)
    {
        int ans = Mathf.RoundToInt(accuracy + (range - Vector3.Distance(pos, targetPos)) * 2);
        return ans;
    }


    IEnumerator<float> FireGuidedMissile(Unit tgt)
    {
        yield return Timing.WaitForSeconds(2f);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ParticleSystem>())
                child.GetComponent<ParticleSystem>().Emit(1);
        }
        audSoc.PlayOneShot(shootSFX);
        GameObject clone = Instantiate(missile);
        clone.GetComponent<MissileScript>().target = tgt;
        clone.GetComponent<MissileScript>().weapon = GetComponent<Weapon>();
        clone.transform.position = transform.position;
        

    }

    IEnumerator<float> FireGun(Unit tgt)
    {
        yield return Timing.WaitForSeconds(2f);
        for (int i=0;i<burstFire;i++)
        {
            audSoc.PlayOneShot(shootSFX);
            audSoc.pitch = Random.Range(0.8f, 1.2f);
            foreach (Transform child in transform)
            {
                if (child.GetComponent<ParticleSystem>())
                {
                    child.GetComponent<ParticleSystem>().Emit(1);
                }
            }
            HitCalc(tgt);
            //ps.Emit(1);
            yield return Timing.WaitForSeconds(fireDelay);
        }
        yield return Timing.WaitForSeconds(4f);
        pc.attackContinue = true;
    }

    public void HitCalc(Unit tgt)
    {
        int rand = Random.Range(0, 100);
        if (rand < AccuracyCalc(tgt))
        {
            int damage = Random.Range(maxDmg / 2, maxDmg);
            tgt.Damage(damage);
            tgt.us.Damage(damage);
            tgt.isHit();
            uic.Damage(tgt, damage + "");
            if (hitFX)
            {
                GameObject clone = Instantiate(hitFX);
                clone.transform.position = tgt.transform.position + new Vector3(Random.Range(-hitOffset, hitOffset), Random.Range(-hitOffset, hitOffset), Random.Range(-hitOffset, hitOffset));
            }
        }
        else
        {
            uic.Damage(tgt, "miss");
        }
    }

    IEnumerator<float> FireShotgun(Unit tgt)
    {
        yield return Timing.WaitForSeconds(2f);
        transform.GetChild(0).GetComponent<ParticleSystem>().Emit(1); //Muzzle
        transform.GetChild(1).GetComponent<ParticleSystem>().Emit(burstFire); //Emitter
        transform.GetChild(2).GetComponent<ParticleSystem>().Emit(2);
        audSoc.PlayOneShot(shootSFX);
        for (int i = 0; i < burstFire; i++)
        {
            HitCalc(tgt);
            yield return Timing.WaitForSeconds(fireDelay);
        }
        yield return Timing.WaitForSeconds(4f);
        pc.attackContinue = true;
    }
}
