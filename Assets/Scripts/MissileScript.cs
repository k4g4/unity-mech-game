using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class MissileScript : MonoBehaviour
{
    public float speed;

    [HideInInspector]
    public Weapon weapon;
    public Unit target;
    PlayerController pc;
    CameraController cc;

    bool isMoving = true;
    void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        cc = FindObjectOfType<CameraController>();
    }

    void Update()
    {

        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            cc.FollowUnit(gameObject);

            if(Vector3.Distance(transform.position,target.transform.position)<0.2f)
            {
                weapon.HitCalc(target);
                foreach(Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                isMoving = false;
                Timing.RunCoroutine(ResetTimer());
            }
        }
    }

    IEnumerator<float> ResetTimer()
    {
        yield return Timing.WaitForSeconds(2f);
        pc.attackContinue = true;
        cc.ResetCamera();
        Destroy(gameObject);
    }
}
