using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public GameObject footprint;
    GameObject leftFoot, rightFoot;

    void Start()
    {
        leftFoot = transform.GetChild(0).transform.GetChild(1).gameObject;
        rightFoot = transform.GetChild(0).transform.GetChild(2).gameObject;
    }

    public void RightFoot()
    {
        GameObject clone = Instantiate(footprint);
        clone.transform.position = rightFoot.transform.position - Vector3.up * 0.02f; ;

        RaycastHit hit;
        if(Physics.Raycast(rightFoot.transform.position+Vector3.up, Vector3.down,out hit,10f,1<<9))
        {
            //clone.transform.LookAt(hit.transform.position + Vector3.up, hit.normal);
            clone.transform.rotation = Quaternion.Euler(new Vector3(0, transform.parent.rotation.eulerAngles.y, 0));
            clone.transform.rotation = Quaternion.FromToRotation(clone.transform.up, hit.normal) * clone.transform.rotation;
        }

    }

    public void LeftFoot()
    {
        GameObject clone = Instantiate(footprint);
        clone.transform.position = leftFoot.transform.position - Vector3.up*0.02f;

        RaycastHit hit;
        if (Physics.Raycast(leftFoot.transform.position + Vector3.up, Vector3.down, out hit, 10f, 1 << 9))
        {
            //clone.transform.LookAt(hit.transform.position + Vector3.up, hit.normal);

            clone.transform.rotation = Quaternion.Euler(new Vector3(0, transform.parent.rotation.eulerAngles.y, 0));
            clone.transform.rotation = Quaternion.FromToRotation(clone.transform.up, hit.normal) * clone.transform.rotation;
        }
    }
}
