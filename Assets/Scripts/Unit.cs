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
}
