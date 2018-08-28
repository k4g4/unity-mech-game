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
        transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
        movePos = targetPos;
    }

    public void Attack(Unit defender) //Just reduces health, add things like accuracy and rng later
    {
        transform.LookAt(new Vector3(defender.transform.position.x, transform.position.y, defender.transform.position.z));
        defender.health -= attack;
        Debug.Log(unitName + " Attacks " + defender.unitName + " For " + attack + "\n" + defender.unitName + " Has " + defender.health + " HP left");
    }
}
