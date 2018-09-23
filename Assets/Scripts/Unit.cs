using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int health;
    public int maxHealth;
    public int actionPoints;
    public int maxActionPoints;
    public int attack;
    public float walkSpeed = 2f;

    public List<Weapon> weapons = new List<Weapon>();
    public bool isWalking = false;
    Vector3 movePos = Vector3.zero;
    Animator anim;

    void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {
        SetWeapons();
    }

    void SetWeapons()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Weapon>())
            {
                weapons.Add(child.GetComponent<Weapon>());
            }
        }

        for (int i=0;i<weapons.Count;i++)
        {
            switch (weapons[i].partPos)
            {
                case 0:
                    weapons[i].transform.SetParent(transform.Find("Model/Armature.001/body.001/LArm"));
                    weapons[i].transform.localPosition = Vector3.zero;
                    break;
                case 1:
                    weapons[i].transform.SetParent(transform.Find("Model/Armature.001/body.001/RArm"));
                    weapons[i].transform.localPosition = Vector3.zero;
                    break;
                case 2:
                    weapons[i].transform.SetParent(transform.Find("Model/Armature.001/body.001/LShoulder"));
                    weapons[i].transform.localPosition = Vector3.zero + Vector3.up * 0.0008f + Vector3.right * 0.0004f;
                    break;
                case 3:
                    weapons[i].transform.SetParent(transform.Find("Model/Armature.001/body.001/RShoulder"));
                    weapons[i].transform.localPosition = Vector3.zero + Vector3.up * 0.0008f + Vector3.left * 0.0004f;
                    break;
                default:
                    Debug.Log("Invalid part position");
                    break;
            }
        }
    }



    void Update()
    {
        if(isWalking)
        {
            //transform.position = Vector3.MoveTowards(transform.position, movePos, walkSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
            RaycastHit hit;
            if(Physics.Raycast(transform.position,-Vector3.up,out hit,5,1<<9))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + 0.5f, transform.position.z);
            }
        }
    }

    public void Move(Vector3 targetPos)
    {
        isWalking = true;
        IsWalking(true);
        transform.LookAt(targetPos); //Temporary fix, need to lock and lerp rotation
        movePos = targetPos;
    }

    public void IsWalking(bool walking)
    {
        anim.SetBool("isWalking", walking);
    }

    public void Attack(Unit attacker, Unit defender, int wep) //Just reduces health, add things like accuracy and rng later
    {
        IsWalking(false);
        transform.LookAt(defender.transform.position); //Temporary fix, need to lock and lerp rotation
        weapons[wep].Fire(defender);
        //defender.health -= attacker.attack;
        Debug.Log(attacker.unitName + " Attacks " + defender.unitName + " For " + attacker.attack + "\n" + defender.unitName + " Has " + defender.health + " HP left");
    }
}
