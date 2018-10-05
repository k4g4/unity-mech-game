using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class CINE_DoorRaise : MonoBehaviour
{
    public float speed;
    public float waitTime;
    public bool rise = false;

    void Start()
    {
        Timing.RunCoroutine(WaitTimer());
    }

    IEnumerator<float> WaitTimer()
    {
        yield return Timing.WaitForSeconds(waitTime);
        rise = true;
    }

	void Update ()
    {
		if(rise)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
	}
}
