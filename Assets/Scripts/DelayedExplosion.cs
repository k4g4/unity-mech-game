using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class DelayedExplosion : MonoBehaviour
{
    public float timeDelay;
    GameObject explosion;

    void Awake()
    {
        explosion = transform.GetChild(0).gameObject;
        explosion.SetActive(false);
    }

	void Start ()
    {
        Timing.RunCoroutine(Delay());	
	}

    IEnumerator<float> Delay()
    {
        yield return Timing.WaitForSeconds(timeDelay);
        explosion.SetActive(true);
    }
	
}