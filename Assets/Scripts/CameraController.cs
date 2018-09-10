using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    bool isFollowing = false;
    public Vector3 originalPos;
    Vector3 targetPos;

	void Update ()
    {
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);
    }

    public void FollowUnit(Unit unit)
    {
        transform.position = Vector3.Lerp(transform.position, unit.transform.position + Vector3.up * 5 - Vector3.forward * 2, speed * Time.deltaTime);
    }

    public void AttackCamera(Unit attacker, Unit defender, int type)
    {
        transform.position = attacker.transform.position + Vector3.up * 1 - Vector3.forward * 2;
        transform.GetChild(0).transform.LookAt(defender.transform);
    }

    public void ResetCamera()
    {
        transform.position = originalPos;
        transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(65, 0, 0));
    }
}
