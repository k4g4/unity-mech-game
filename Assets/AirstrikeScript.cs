using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeScript : MonoBehaviour
{

    public float radius = 5f;
    public int turn = 0;

    public GameObject airstrikeMissile;
	// Use this for initialization
	void Start ()
    {
        transform.GetChild(0).localScale = new Vector3(radius*10, radius*10, radius*10);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(turn == 2)
        {
            Hit();
        }
	}

    public void Hit()
    {
        for(int i=0;i<10; i++)
        {
            GameObject clone = Instantiate(airstrikeMissile);
            clone.transform.position = transform.position + new Vector3(Random.Range(-radius, radius), Random.Range(20f, 60f) + 20f, Random.Range(-radius, radius));
            clone.GetComponent<AirstrikeMissileScript>().yHeight = transform.position.y;
        }
        Collider[] collisions = Physics.OverlapSphere(transform.position, radius, 1 << 10 | 1 << 11);
        for(int i=0;i<collisions.Length;i++)
        {
            collisions[i].GetComponent<Unit>().Damage(40);
        }
        Destroy(gameObject);
    }
}
