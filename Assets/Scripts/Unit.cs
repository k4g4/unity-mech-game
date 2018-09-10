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

    List<Weapon> weapons = new List<Weapon>();

    Vector3 movePos = Vector3.zero;

    void Awake()
    {
        foreach(Transform child in transform)
        {
            if(child.GetComponent<Weapon>())
            {
                weapons.Add(child.GetComponent<Weapon>());
            }
        }
    }

    void Update()
    {
        if(movePos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePos, 5 * Time.deltaTime);
        }
    }

    public void Move(Vector3 targetPos)
    {
        transform.LookAt(targetPos); //Temporary fix, need to lock and lerp rotation
        movePos = targetPos;
    }

    public void Attack(Unit attacker, Unit defender) //Just reduces health, add things like accuracy and rng later
    {
        transform.LookAt(defender.transform.position); //Temporary fix, need to lock and lerp rotation
        weapons[0].Fire(defender);
        //defender.health -= attacker.attack;
        Debug.Log(attacker.unitName + " Attacks " + defender.unitName + " For " + attacker.attack + "\n" + defender.unitName + " Has " + defender.health + " HP left");
    }
}
