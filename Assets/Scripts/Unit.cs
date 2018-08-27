using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public int weaponType = 0;
    public int damage;
    public int accuracy;
}


public class Unit : MonoBehaviour
{
    public string unitName;
    public int health;
    public int maxHealth;
    public int actionPoints;
    public int maxActionPoints;
    public int attack;

    Vector3 movePos = Vector3.zero;

    void Update()
    {
        if(movePos != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePos, 5 * Time.deltaTime);
        }
    }

    public void Move(Vector3 targetPos)
    {
        movePos = targetPos;
    }

    public void Attack(Unit attacker, Unit defender) //Just reduces health, add things like accuracy and rng later
    {

        defender.health -= attacker.attack;
        Debug.Log(attacker.unitName + " Attacks " + defender.unitName + " For " + attacker.attack + "\n" + defender.unitName + " Has " + defender.health + " HP left");
    }
}
