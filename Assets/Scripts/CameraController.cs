using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class CameraController : MonoBehaviour
{
    public float speed;
    bool isFollowing = false;
    public Vector3 originalPos;
    Vector3 targetPos;
    Quaternion targetRot;

	void Update ()
    {
        transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        if(isFollowing)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos,Time.deltaTime*2f);
            transform.GetChild(0).transform.rotation = Quaternion.Lerp(transform.GetChild(0).transform.rotation, targetRot, Time.deltaTime * 2f);

        }
    }

    public void FlipCamera()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y + 180, 0));
    }

    public void FollowUnit(Unit unit)
    {
        transform.position = Vector3.Lerp(transform.position, unit.transform.position + Vector3.up * 2 - Vector3.forward, speed * Time.deltaTime);
    }

    public void AttackCamera(Unit attacker, Unit defender, int type)
    {
        switch(type)
        {
            case 0:
                int rand = Random.Range(0, 2);
                if(rand == 0)
                    Timing.RunCoroutine(GunCameraOne(attacker, defender));
                else if(rand == 1)
                    Timing.RunCoroutine(GunCameraTwo(attacker, defender));
                break;
            case 1:
                transform.position = attacker.transform.position + Vector3.up * 1 - Vector3.forward * 2;
                transform.GetChild(0).transform.LookAt(defender.transform);
                break;
            default:
                Debug.Log("Missing attack camera type");
                break;
        }
    }

    IEnumerator<float> GunCameraOne(Unit atk, Unit def)
    {
        isFollowing = true;
        targetPos = atk.transform.position + Vector3.up*0.5f - atk.transform.forward * 2;
        targetRot = Quaternion.LookRotation(def.transform.position - atk.transform.position);
        yield return Timing.WaitForSeconds(3f);
        targetPos = def.transform.position + Vector3.up*0.5f - atk.transform.forward * 2;
        yield return Timing.WaitForSeconds(3f);
        isFollowing = false;
    }

    IEnumerator<float> GunCameraTwo(Unit atk, Unit def)
    {
        isFollowing = false;
        transform.position = atk.transform.position - Vector3.up * 0.1f - atk.transform.right * 1 + atk.transform.forward * 0.5f;
        transform.GetChild(0).transform.LookAt(atk.transform.position);
        yield return Timing.WaitForSeconds(3f);
        isFollowing = true;
        targetPos = def.transform.position + Vector3.up * 0.5f - atk.transform.forward * 2;
        targetRot = Quaternion.LookRotation(def.transform.position - atk.transform.position);
        yield return Timing.WaitForSeconds(3f);
        isFollowing = false;
    }

    public void ResetCamera()
    {
        transform.position = originalPos;
        transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(65, 0, 0));
    }
}
    