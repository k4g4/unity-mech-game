using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CINE_CameraScroll : MonoBehaviour
{
    public float speed = 5f;

    void Update ()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);	
	}
}
