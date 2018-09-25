using System.Collections;
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
    Unit selectedUnit;
    CameraController cc;
    UIController uic;
    WeaponSelect wepSelect;
    List<Waypoint> waypoints = new List<Waypoint>();
    List<Unit> teamOneList = new List<Unit>();
    List<Unit> teamTwoList = new List<Unit>();
    GameObject canvas;
    GameObject apCost;
    bool isInverse = true;

    void Awake()
    {
        wepSelect = FindObjectOfType<WeaponSelect>();
        uic = FindObjectOfType<UIController>();
        cc = FindObjectOfType<CameraController>();
        canvas = GameObject.Find("Canvas");
        apCost = GameObject.Find("APCostText");
        wepSelect.gameObject.SetActive(false);
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

    public void EndTurn() //Swap unit layers
    {
        for(int i=0;i<teamOneList.Count;i++)
        {
            if(isInverse)
                teamOneList[i].gameObject.layer = 11;
            else
                teamOneList[i].gameObject.layer = 10;
            teamOneList[i].actionPoints = teamOneList[i].maxActionPoints;
        }

        for (int i=0;i<teamTwoList.Count;i++)
        {
            if(isInverse)
                teamTwoList[i].gameObject.layer = 10;
            else
                teamTwoList[i].gameObject.layer = 11;
            teamTwoList[i].actionPoints = teamTwoList[i].maxActionPoints;
        }

        isInverse = !isInverse;
        ClearWaypoints();
        //cc.FlipCamera();
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
                if (Vector3.Distance(selectedUnit.transform.position, waypoints[0].pos) < 0.1f)
                {
                    selectedUnit.isWalking = false;
                    RemoveWaypoint(0);
                }
            }
            else if (waypoints[0].type == 1)
            {
                if (!isAttackAnim)
                {
                    Timing.RunCoroutine(AttackAnim());
                    isAttackAnim = true;
                }
            }
        }
        else
        {
            selectedUnit.IsWalking(false);
            selectedUnit = null;
            isExecuting = false;
            cc.ResetCamera();
        }
    }

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
        yield return Timing.WaitForSeconds(7f);
        isAttackAnim = false;
        isExecuting = true;
    }

    public void Execute()
    {
        Debug.Log("Executing " + waypoints.Count);
        if(waypoints.Count>0)
        {
            cc.originalPos = cc.transform.position;
            isExecuting = true;
            //selectedUnit.Move(waypoints[0].pos);
        }
    }

    Waypoint GetLastMovePoint()
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

    void AddWaypoint(int type, int apCost, Vector3 pos, string info, Unit target) //Creates a waypoint based on type
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

    void RemoveWaypoint(int i)
    {
        Waypoint temp = waypoints[i];
        Destroy(temp.waypointMarker);
        Destroy(temp);
        waypoints.RemoveAt(i);
    }

    void OnClickCheck()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f)) 
        {
            if(selectedUnit)
            {
                if (team == 0) //TODO : Swap team layers when hotseat multiplayer to avoid double code.
                {
                    Debug.Log(hit.transform.gameObject.layer);
                    if (hit.transform.gameObject.layer == 9) //Clicked on floor
                    {

                        Waypoint lastMovePoint = GetLastMovePoint();
                        Waypoint temp = new Waypoint();
                        temp.pos = lastMovePoint.pos + Vector3.up * 0.5f;
                        //lastMovePoint.pos = lastMovePoint.pos + Vector3.up * 0.5f;
                        if (HasPointsToMove(selectedUnit, temp.pos, hit.point)) //Change to check on a 2d plane only later
                        {
                            if (!Physics.Raycast(temp.pos, (hit.point + Vector3.up * 0.5f) - temp.pos, Vector3.Distance(temp.pos, hit.point + Vector3.up * 0.5f), 1 << 9))
                            {
                                string info = waypoints.Count + " : MOVE\nAP Cost : " + Mathf.RoundToInt(Vector3.Distance(temp.pos, hit.point)) + "\nAP Rem : " + selectedUnit.actionPoints;
                                AddWaypoint(0, Mathf.RoundToInt(Vector3.Distance(temp.pos, hit.point)), hit.point + Vector3.up * 0.5f, info, null); //check vector up displacement, increases by half meter every click
                                Debug.Log("Move Order Set");
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
                        if (HasPointsToAttack(selectedUnit)) //add create waypoint method
                        {
                            Waypoint lastMovePoint = GetLastMovePoint();
                            Waypoint temp = new Waypoint();
                            temp.pos = lastMovePoint.pos + Vector3.up * 0.5f;

                            if (!Physics.Raycast(temp.pos,hit.transform.position - temp.pos, Vector3.Distance(temp.pos, hit.transform.position), 1 << 9))
                            {
                                string info = waypoints.Count + " : ATK\nAP Cost : " + 10 + "\nAP Rem : " + selectedUnit.actionPoints;
                                wepSelect.gameObject.SetActive(true);
                                wepSelect.SetWeaponText(selectedUnit);
                                wepSelect.transform.position = Input.mousePosition;
                                AddWaypoint(1, 10, hit.transform.position + Vector3.up * 0.5f, info, hit.transform.GetComponent<Unit>());
                                //Attack(selectedUnit, hit.transform.GetComponent<Unit>());
                                Debug.Log("Attack Order Given");
                            }
                            else
                            {
                                Debug.Log("There is something in the way");
                            }
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
                    apCost.SetActive(true);
                    SelectUnit(hit.transform.GetComponent<Unit>());
                }
            }
        }
    }

    public void SetAttackType(int type)
    {
        waypoints[waypoints.Count-1].wepType = type;
    }

    void SelectUnit(Unit unit)
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
            Waypoint temp = waypoints[0];
            Destroy(temp);
            waypoints.RemoveAt(0);
        }
    }

    bool HasPointsToMove(Unit unit, Vector3 pos, Vector3 dest) // Check to see if there are enough points to move (Change to waypoint system calculation later)
    {
        float dist = Mathf.RoundToInt(Vector3.Distance(pos, dest));
        return unit.actionPoints >= dist;
    }

    bool HasPointsToAttack(Unit unit)
    {
        return unit.actionPoints >= 10; //arbitrary value, change to check weapon AP usage later
    }
}
