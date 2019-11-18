using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class CINE_Tank : MonoBehaviour
{
    public float fireDelay;
    public bool isFiring = true;
    public bool isDying = false;
    public bool isMoving = false;
    public bool detonate = false;
    public float detonateTime = 1f;
    ParticleSystem muzzle, shot, dust;
    public float speed = 1;
    public GameObject explosionFX;

    void Awake()
    {
        muzzle = transform.GetChild(0).GetComponent<ParticleSystem>();
        dust = transform.GetChild(1).GetComponent<ParticleSystem>();
        shot = transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    void Start ()
    {
        Timing.RunCoroutine(Init());
        if (detonate)
            Timing.RunCoroutine(Detonate());
    }

    void Update()
    {
        if(isMoving)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit, 5, 1 << 9))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, Time.deltaTime*2);

            }
        }
    }

    IEnumerator<float> Detonate()
    {
        yield return Timing.WaitForSeconds(detonateTime);
        isMoving = false;
        isFiring = false;
        transform.GetChild(4).GetComponent<Rigidbody>().isKinematic = false;
        transform.GetChild(4).GetComponent<Rigidbody>().AddExplosionForce(100, transform.position - Vector3.up, 3f);
        transform.GetChild(4).SetParent(null);
        if(explosionFX)
        {
            GameObject clone = Instantiate(explosionFX);
            clone.transform.position = transform.position;
        }
    }

    IEnumerator<float> Init()
    {
        yield return Timing.WaitForSeconds(fireDelay);
        if (isFiring)
            Timing.RunCoroutine(Shoot());
    }

    IEnumerator<float> Shoot()
    {
        muzzle.Emit(1);
        dust.Emit(50);
        shot.Emit(1);
        yield return Timing.WaitForSeconds(5f);
        Timing.RunCoroutine(Shoot());
    }
}
