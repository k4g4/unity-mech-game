using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CINE_MechWalk : MonoBehaviour
{
    public bool isWalking = true;
    public float speed = 5f;
    // Use this for initialization
    void Start()
    {
        if(isWalking)
            transform.GetChild(0).GetComponent<Animator>().SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(isWalking)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 5, 1 << 9))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + 0.5f, transform.position.z);
            }
        }
    }
}
