using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int team = 0;
    Unit selectedUnit;

	void Start ()
    {
		
	}

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnClickCheck();
        }
        if(Input.GetMouseButtonDown(1))
        {
            selectedUnit = null;
        }
    }

    void OnClickCheck()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f)) 
        {
            if(selectedUnit)
            {
                if (team == 0)
                {
                    if (hit.transform.gameObject.layer == 9) //Clicked on floor
                    {
                        if(HasPointsToMove(selectedUnit,selectedUnit.transform.position,hit.point)) //Change to check on a 2d plane only later
                        {
                            selectedUnit.Move(hit.point);
                            selectedUnit.actionPoints -= Mathf.RoundToInt(Vector3.Distance(selectedUnit.transform.position, hit.point));
                            Debug.Log("Move Order Given");
                        }
                        else
                        {
                            Debug.Log("There are not enough points to move");
                        }
                    }
                    else if (hit.transform.gameObject.layer == 11) //Clicked on Enemy
                    {
                        if (HasPointsToAttack(selectedUnit))
                        {
                            Attack(selectedUnit, hit.transform.GetComponent<Unit>());
                            selectedUnit.actionPoints -= 10;
                            Debug.Log("Attack Order Given");
                        }
                        else
                        {
                            Debug.Log("There are not enough points to attack");
                        }
                    }
                }
            }
            else //See if object clicked is eligible to be selected
            {
                if (team == 0 && hit.transform.gameObject.layer == 10 || team == 1 && hit.transform.gameObject.layer == 11)
                {
                    selectedUnit = hit.transform.GetComponent<Unit>();
                    Debug.Log("Unit Selected");
                }
            }
        }
    }

    bool HasPointsToMove(Unit unit, Vector3 pos, Vector3 dest) // Check to see if there are enough points to move (Change to waypoint system calculation later)
    {
        float dist = Mathf.RoundToInt(Vector3.Distance(pos, dest));
        return unit.actionPoints > dist;
    }

    bool HasPointsToAttack(Unit unit)
    {
        return unit.actionPoints >= 10; //arbitrary value, change to check weapon AP usage later
    }

    void Attack(Unit attacker, Unit defender)
    {
        defender.health -= attacker.attack;
        Debug.Log(attacker.unitName + " Attacks " + defender.unitName + " For " + attacker.attack + "\n" + defender.unitName + " Has " + defender.health + " HP left");
    }
}
