using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeMissileScript : MonoBehaviour
{
    public GameObject explosion;
    public float yHeight;
	// Use this for initialization
	void Start ()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.down * 30f*Time.deltaTime,Space.World);
	    if(transform.position.y <= yHeight)
        {
            GameObject clone = Instantiate(explosion);
            clone.transform.position = transform.position;
            Destroy(gameObject);
        }	
	}
}
