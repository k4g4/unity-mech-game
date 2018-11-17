﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MEC;

public class Waypoint : ScriptableObject
{
    public int type;
    public int apCost;
    public Vector3 pos;
    public GameObject waypointMarker;
    public Unit target;
    public int wepType;
    public Waypoint() { }
}

public class PlayerController : MonoBehaviour
{
    public int team = 0;
    public GameObject waypointMarker;

    bool isExecuting = false;
    public Unit selectedUnit;
    CameraController cc;
    UIController uic;
    WeaponSelect wepSelect;
    public List<Waypoint> waypoints = new List<Waypoint>();
    public List<Unit> teamOneList = new List<Unit>();
    public List<Unit> teamTwoList = new List<Unit>();
    GameObject canvas;
    GameObject apCost;
    bool isInverse = true;
    public int turnCount = 1;
    UnitStatus[] unitStatus;
    public GameObject mech,heavyMech;

    void Awake()
    {
        wepSelect = FindObjectOfType<WeaponSelect>();
        uic = FindObjectOfType<UIController>();
        cc = FindObjectOfType<CameraController>();
        canvas = GameObject.Find("Canvas");
        apCost = GameObject.Find("APCostText");
        wepSelect.gameObject.SetActive(false);
        SpawnUnits();
    }

    void SpawnUnits()
    {
        if (!GameController.instance)
            return;
        for(int i=0;i<GameController.instance.activeUnits.Count;i++)
        {
            GameObject clone;
            if (GameController.instance.activeUnits[i].unitType == 1)
                clone = Instantiate(mech);
            else
                clone = Instantiate(heavyMech);
            clone.transform.position = GameObject.Find("SpawnPoint").transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            Unit cloneUnit = clone.GetComponent<Unit>();
            cloneUnit.unitName = GameController.instance.activeUnits[i].unitName;
            cloneUnit.maxHealth = GameController.instance.activeUnits[i].maxHealth;
            cloneUnit.health = GameController.instance.activeUnits[i].health;
            cloneUnit.maxActionPoints = GameController.instance.activeUnits[i].maxActionPoints;
            cloneUnit.actionPoints = cloneUnit.maxActionPoints;
            for(int j=0;j<GameController.instance.activeUnits[i].weapons.Count;j++)
            {
                Debug.Log(GameController.instance.activeUnits[i].weapons[j].weaponType);
                GameObject wepClone = Instantiate(PartDict.instance.partDict[GameController.instance.activeUnits[i].weapons[j].weaponType]);
                wepClone.GetComponent<Weapon>().partPos = GameController.instance.activeUnits[i].weapons[j].weaponPos;
                cloneUnit.weapons.Add(wepClone.GetComponent<Weapon>());
            }
        }
    }

    void Start()
    {
        Unit[] units = FindObjectsOfType<Unit>();
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].gameObject.layer == 10)
            {
                teamOneList.Add(units[i]);
            }
            else if (units[i].gameObject.layer == 11)
            {
                teamTwoList.Add(units[i]);
            }
        }
    }

    public void RemoveUnit(Unit unit)
    {
        if(waypoints.Count>0)
        {
            for(int x=0;x<waypoints.Count;x++)
            {
                for (int i = 0; i < waypoints.Count; i++)
                {
                    if (waypoints[i].type == 1 && waypoints[i].target == unit)
                    {
                        RemoveWaypoint(i);
                    }
                }
            }
        }
        if (teamOneList.Contains(unit))
            teamOneList.Remove(unit);
        else
            teamTwoList.Remove(unit);
    }

    public void TryEndTurn()
    {
        if(!isExecuting && isInverse)
            EndTurn();
    }

    public void EndTurn() //Swap unit layers
    {
        AirstrikeScript[] airstrikes = FindObjectsOfType<AirstrikeScript>();
        foreach (AirstrikeScript atk in airstrikes)
        {
            atk.turn++;
        }

        for (int i=0;i<teamOneList.Count;i++)
        {
            if(isInverse)
                teamOneList[i].gameObject.layer = 11;
            else
                teamOneList[i].gameObject.layer = 10;
            teamOneList[i].actionPoints = teamOneList[i].maxActionPoints;
            teamOneList[i].us.UpdateInfo();
        }

        for (int i=0;i<teamTwoList.Count;i++)
        {
            if(isInverse)
                teamTwoList[i].gameObject.layer = 10;
            else
                teamTwoList[i].gameObject.layer = 11;
            teamTwoList[i].actionPoints = teamTwoList[i].maxActionPoints;
            teamTwoList[i].us.UpdateInfo();
        }
        ClearWaypoints();
        //cc.FlipCamera();
        if (isInverse)
            FindObjectOfType<EnemyController>().StartTurn();
        else
        {
            turnCount++;
            uic.SetTurnCounter(turnCount);
        }
        isInverse = !isInverse;
        Debug.Log("Ending turn");
    }

    void Update()
    {
        if (isExecuting)
        {
            if (apCost.activeInHierarchy)
                apCost.SetActive(false);
            Executing();
            return;
        }
        if (!isInverse || isAttackAnim)
            return;
        if (Input.GetKeyDown(KeyCode.Space)) //Hotkey for executing orders
        {
            if(!wepSelect.gameObject.activeInHierarchy)
                Execute();
        }

        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //Mouse click event as long as it's not over UI
        {
            OnClickCheck();
        }

        if(Input.GetMouseButtonDown(1)) //Remove waypoint/deselect
        {
            if(waypoints.Count>1)
            {
                selectedUnit.actionPoints += waypoints[waypoints.Count - 1].apCost;
                selectedUnit.us.UpdateInfo();
                if (selectedUnit.usp)
                    selectedUnit.usp.UpdateInfo();
                RemoveWaypoint(waypoints.Count - 1);
            }
            else if(selectedUnit)
            {
                RemoveWaypoint(0);
                selectedUnit = null;
                apCost.SetActive(false);
            }
            if (wepSelect.gameObject.activeInHierarchy)
                wepSelect.gameObject.SetActive(false);
        }

        if(selectedUnit && !isExecuting) //Mouse following AP indicator
        {
            if (!apCost.activeInHierarchy)
                apCost.SetActive(true);
            APIndicator();
        }

    }

    void APIndicator()
    {
        if (waypoints.Count < 1)
            return;

        apCost.transform.position = Input.mousePosition;
        int cost = 0;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.gameObject.layer == 9) //hover on floor
            {
                Waypoint lastMovePoint = GetLastMovePoint();
                cost = Mathf.RoundToInt(Vector3.Distance(lastMovePoint.pos, hit.point));
                apCost.GetComponent<Text>().text = "AP Cost: " + cost;
            }
            else if (hit.transform.gameObject.layer == 11) //hover on enemy
            {
                apCost.GetComponent<Text>().text = "AP Cost: " + 10;
            }
        }
    }

    bool isAttackAnim = false;

    void Executing() //Logic for unit movement and attack order
    {
        if (waypoints.Count > 0)
        {
            if (waypoints[0].type == 0)
            {
                cc.FollowUnit(selectedUnit);
                selectedUnit.Move(waypoints[0].pos);
                if (Vector3.Distance(selectedUnit.transform.position, waypoints[0].pos) < 0.2f)
                {
                    selectedUnit.isWalking = false;
                    RemoveWaypoint(0);
                }
            }
            else if (waypoints[0].type == 1)
            {
                if (!isAttackAnim)
                {
                    if (!waypoints[0].target || waypoints[0].target.health < 1)
                    {
                        selectedUnit.actionPoints += waypoints[0].apCost;
                        selectedUnit.us.UpdateInfo();
                        if (selectedUnit.usp)
                            selectedUnit.usp.UpdateInfo();
                        RemoveWaypoint(0);
                    }
                    else
                    {
                        Timing.RunCoroutine(AttackAnim());
                        isAttackAnim = true;
                    }
                }
            }
        }
        else
        {
            selectedUnit.IsWalking(false);
            selectedUnit = null;
            isExecuting = false;
            cc.ResetCamera();
            ShowUnitInfo(true);
        }
    }

    public bool attackContinue = false;

    IEnumerator<float> AttackAnim()
    {
        isExecuting = false;
        selectedUnit.Attack(selectedUnit, waypoints[0].target, waypoints[0].wepType);
        cc.AttackCamera(selectedUnit, waypoints[0].target, selectedUnit.weapons[waypoints[0].wepType].weaponType);
        if(waypoints.Count>1)
        {
            if(waypoints[1].type==0)
            {
                cc.ResetCamera();
            }
        }
        RemoveWaypoint(0);
        while(!attackContinue)
        {
            yield return Timing.WaitForOneFrame;
        }
        //yield return Timing.WaitForSeconds(7f); //change to outside modifiable
        isAttackAnim = false;
        isExecuting = true;
        attackContinue = false;
        if(waypoints.Count<1 || waypoints[0].type==0)
            cc.ResetCamera();
    }

    public void Execute()
    {
        if(waypoints.Count>0)
        {
            unitStatus = FindObjectsOfType<UnitStatus>();
            ShowUnitInfo(false);
            cc.originalPos = cc.transform.position;
            isExecuting = true;
            //selectedUnit.Move(waypoints[0].pos);
        }
    }

    public Waypoint GetLastMovePoint()
    {
        for (int i = waypoints.Count - 1; i > 0; i--)
        {
            if (waypoints[i].type == 0)
            {
                return waypoints[i];
            }
        }
        return waypoints[0];
    }

    public void ShowUnitInfo(bool show)
    {
        for(int i=0;i< unitStatus.Length;i++)
        {
            if(unitStatus[i])
                unitStatus[i].gameObject.SetActive(show);
        }
    }

    public void AddWaypoint(int type, int apCost, Vector3 pos, string info, Unit target) //Creates a waypoint based on type
    {
        Waypoint lastMovePoint = GetLastMovePoint();
        //lastMovePoint.pos = lastMovePoint.pos;
        Waypoint temp = ScriptableObject.CreateInstance<Waypoint>();
        GameObject marker = Instantiate(waypointMarker);
        //marker.transform.SetParent(canvas.transform);
        temp.type = type;
        temp.apCost = apCost;
        temp.pos = pos;
        temp.waypointMarker = marker;
        waypoints.Add(temp);
        //selectedUnit.Move(hit.point);
        selectedUnit.actionPoints -= temp.apCost;
        selectedUnit.us.SetAP(temp.apCost);
        if(selectedUnit.usp)
            selectedUnit.usp.UpdateInfo();
        marker.GetComponent<WaypointMarker>().pos = temp.pos;
        marker.GetComponent<WaypointMarker>().SetInfo(info);
        if (type == 0)
        {
            lastMovePoint.waypointMarker.GetComponent<WaypointMarker>().next = marker.GetComponent<WaypointMarker>();
            marker.GetComponent<WaypointMarker>().SetColor(true);
        }
        else if (type == 1)
        {
            lastMovePoint.waypointMarker.GetComponent<WaypointMarker>().atk = marker.GetComponent<WaypointMarker>();
            marker.GetComponent<WaypointMarker>().SetColor(false);
        }
        if (target)
            temp.target = target;
    }

    public void RemoveWaypoint(int i)
    {
        Waypoint temp = waypoints[i];
        Destroy(temp.waypointMarker);
        Destroy(temp);
        waypoints.RemoveAt(i);
    }

    void OnClickCheck()
    {
        if (isExecuting)
            return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f)) 
        {
            if(selectedUnit)
            {
                if (team == 0) //TODO : Swap team layers when hotseat multiplayer to avoid double code.
                {
                    if (hit.transform.gameObject.layer == 9) //Clicked on floor
                    {

                        Waypoint lastMovePoint = GetLastMovePoint();
                        Waypoint temp = new Waypoint();
                        temp.pos = lastMovePoint.pos + Vector3.up * 0.5f;
                        //lastMovePoint.pos = lastMovePoint.pos + Vector3.up * 0.5f;
                        if (HasPointsToMove(selectedUnit, temp.pos, hit.point)) //Change to check on a 2d plane only later
                        {
                            if (!Physics.Raycast(temp.pos, (hit.point + Vector3.up * 0.5f) - temp.pos, Vector3.Distance(temp.pos, hit.point + Vector3.up * 0.5f), 1 << 9 | 1<<12) && hit.transform.tag != "NoWalk")
                            {
                                string info = waypoints.Count + " : MOVE\nAP Cost : " + Mathf.RoundToInt(Vector3.Distance(temp.pos, hit.point)) + "\nAP Rem : " + selectedUnit.actionPoints;
                                AddWaypoint(0, Mathf.RoundToInt(Vector3.Distance(temp.pos, hit.point)), hit.point + Vector3.up * 0.5f, info, null); //check vector up displacement, increases by half meter every click
                            }
                            else
                            {
                                Debug.Log("There is something in the way");
                            }
                        }
                        else
                        {
                            Debug.Log("There are not enough points to move");
                        }
                    }
                    else if (hit.transform.gameObject.layer == 10) //Clicked on Friendly
                    {
                        SelectUnit(hit.transform.GetComponent<Unit>());
                    }
                    else if (hit.transform.gameObject.layer == 11) //Clicked on Enemy
                    {
                        /*
                        if (HasPointsToAttack(selectedUnit)) //add create waypoint method
                        {
                        }
                        else
                        {
                            Debug.Log("There are not enough points to attack");
                        }*/
                        Waypoint lastMovePoint = GetLastMovePoint();
                        Waypoint temp = new Waypoint();
                        temp.pos = lastMovePoint.pos + Vector3.up * 0.5f;

                        if (!Physics.Raycast(temp.pos, hit.transform.position - temp.pos, Vector3.Distance(temp.pos, hit.transform.position), 1 << 9))
                        {
                            string info = waypoints.Count + " : ATK\nAP Cost : " + 10 + "\nAP Rem : " + selectedUnit.actionPoints;
                            wepSelect.gameObject.SetActive(true);
                            wepSelect.SetWeaponText(selectedUnit,hit.transform.GetComponent<Unit>());
                            wepSelect.transform.position = Input.mousePosition;
                            AddWaypoint(1, 0, hit.transform.position + Vector3.up * 0.5f, info, hit.transform.GetComponent<Unit>());
                            //Attack(selectedUnit, hit.transform.GetComponent<Unit>());
                        }
                        else
                        {
                            Debug.Log("There is something in the way");
                        }
                    }
                }
            }
            else //See if object clicked is eligible to be selected
            {
                if (team == 0 && hit.transform.gameObject.layer == 10 || team == 1 && hit.transform.gameObject.layer == 11)
                {
                    apCost.SetActive(true);
                    SelectUnit(hit.transform.GetComponent<Unit>());
                }
            }
        }
    }

    public void SetAttackType(int type)
    {
        selectedUnit.actionPoints -= selectedUnit.weapons[type].apCost;
        waypoints[waypoints.Count-1].apCost = selectedUnit.weapons[type].apCost;
        waypoints[waypoints.Count-1].wepType = type;
        selectedUnit.us.UpdateInfo();
        if (selectedUnit.usp)
            selectedUnit.usp.UpdateInfo();
    }

    public void SelectUnit(Unit unit)
    {
        ClearWaypoints();
        selectedUnit = unit;
        Waypoint temp = ScriptableObject.CreateInstance<Waypoint>();
        GameObject marker = Instantiate(waypointMarker);
        marker.transform.SetParent(canvas.transform);
        temp.waypointMarker = marker;
        temp.type = 0;
        temp.apCost = 0;
        temp.pos = selectedUnit.transform.position;
        waypoints.Add(temp);
        marker.GetComponent<WaypointMarker>().pos = temp.pos;
        marker.GetComponent<WaypointMarker>().SetInfo(waypoints.Count + " : START\nAP : " + selectedUnit.actionPoints);
        uic.selectedUnit = unit;
        Debug.Log("Unit Selected");
    }

    void ClearWaypoints()
    {
        while(waypoints.Count>0)
        {
            if(selectedUnit)
            {
                selectedUnit.actionPoints += waypoints[0].apCost;
                selectedUnit.us.UpdateInfo();
                if (selectedUnit.usp)
                    selectedUnit.usp.UpdateInfo();
            }
            Waypoint temp = waypoints[0];
            Destroy(temp.waypointMarker);
            Destroy(temp);
            waypoints.RemoveAt(0);
        }
    }

    bool HasPointsToMove(Unit unit, Vector3 pos, Vector3 dest) // Check to see if there are enough points to move (Change to waypoint system calculation later)
    {
        float dist = Mathf.RoundToInt(Vector3.Distance(pos, dest));
        return unit.actionPoints >= dist;
    }

   public bool HasPointsToAttack(Unit unit,int weapon)
    {
        return unit.actionPoints >= unit.weapons[weapon].apCost; //arbitrary value, change to check weapon AP usage later
    }
}
