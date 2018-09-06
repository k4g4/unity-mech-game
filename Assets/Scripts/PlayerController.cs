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

    public Waypoint() { }
}

public class PlayerController : MonoBehaviour
{
    public int team = 0;
    public GameObject waypointMarker;

    bool isExecuting = false;
    Unit selectedUnit;
    CameraController cc;
    List<Waypoint> waypoints = new List<Waypoint>();
    List<Unit> teamOneList = new List<Unit>();
    List<Unit> teamTwoList = new List<Unit>();
    GameObject canvas;
    GameObject apCost;
    bool isInverse = true;

    void Awake()
    {
        cc = FindObjectOfType<CameraController>();
        canvas = GameObject.Find("Canvas");
        apCost = GameObject.Find("APCostText");
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
        Debug.Log("Ending turn");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) //Hotkey for executing orders
        {
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
                Waypoint temp = waypoints[waypoints.Count - 1];
                Destroy(temp.waypointMarker);
                Destroy(temp);
                waypoints.RemoveAt(waypoints.Count-1);
            }
            else
            {
                selectedUnit = null;
            }
        }

        if(selectedUnit && !isExecuting) //Mouse following AP indicator
        {
            APIndicator();
        }

        if(isExecuting)
        {
            Executing();
        }
    }

    void APIndicator()
    {
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
            if(waypoints[0].type==0)
            {
                cc.FollowUnit(selectedUnit);
                selectedUnit.Move(waypoints[0].pos);
                if (Vector3.Distance(selectedUnit.transform.position, waypoints[0].pos) < 0.01f)
                {
                    Waypoint temp = waypoints[0];
                    Destroy(temp.waypointMarker);
                    Destroy(temp);
                    waypoints.RemoveAt(0);
                }
            }
            else if(waypoints[0].type==1)
            {
                if(!isAttackAnim)
                {
                    Timing.RunCoroutine(AttackAnim());
                    isAttackAnim = true;
                }
            }
        }
        else
        {
            selectedUnit = null;
            isExecuting = false;
            cc.ResetCamera();
        }
    }

    IEnumerator<float> AttackAnim()
    {
        isExecuting = false;
        cc.AttackCamera(selectedUnit, waypoints[0].target, 0);
        selectedUnit.Attack(selectedUnit, waypoints[0].target);
        Waypoint temp = waypoints[0];
        Destroy(temp.waypointMarker);
        Destroy(temp);
        waypoints.RemoveAt(0);
        yield return Timing.WaitForSeconds(2f);
        cc.ResetCamera();
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
                    if (hit.transform.gameObject.layer == 9) //Clicked on floor
                    {

                        Waypoint lastMovePoint = GetLastMovePoint();
                        if (HasPointsToMove(selectedUnit, lastMovePoint.pos, hit.point)) //Change to check on a 2d plane only later
                        {
                            Waypoint temp = ScriptableObject.CreateInstance<Waypoint>();
                            GameObject marker = Instantiate(waypointMarker);
                            marker.transform.SetParent(canvas.transform);
                            temp.type = 0;
                            temp.apCost = Mathf.RoundToInt(Vector3.Distance(lastMovePoint.pos, hit.point));
                            temp.pos = hit.point + Vector3.up * 0.5f;
                            temp.waypointMarker = marker;
                            waypoints.Add(temp);
                            //selectedUnit.Move(hit.point);
                            selectedUnit.actionPoints -= temp.apCost;
                            marker.GetComponent<WaypointMarker>().pos = temp.pos;
                            marker.GetComponent<WaypointMarker>().SetInfo(waypoints.Count + " : MOVE\nAP Cost : " + temp.apCost + "\nAP Rem : " + selectedUnit.actionPoints);
                            lastMovePoint.waypointMarker.GetComponent<WaypointMarker>().next = marker.GetComponent<WaypointMarker>();
                            Debug.Log("Move Order Set");
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
                            lastMovePoint.pos = lastMovePoint.pos + Vector3.up * 0.5f;

                            if (!Physics.Raycast(lastMovePoint.pos,hit.transform.position - lastMovePoint.pos, Vector3.Distance(lastMovePoint.pos, hit.transform.position), 1 << 9))
                            {
                                Waypoint temp = ScriptableObject.CreateInstance<Waypoint>();
                                GameObject marker = Instantiate(waypointMarker);
                                marker.transform.SetParent(canvas.transform);
                                temp.type = 1;
                                temp.apCost = 10;
                                temp.waypointMarker = marker;
                                temp.target = hit.transform.GetComponent<Unit>();
                                waypoints.Add(temp);
                                //selectedUnit.Move(hit.point);
                                selectedUnit.actionPoints -= 10;
                                marker.GetComponent<WaypointMarker>().pos = hit.point;
                                marker.GetComponent<WaypointMarker>().SetInfo(waypoints.Count + " : ATK\nAP Cost : " + temp.apCost + "\nAP Rem : " + selectedUnit.actionPoints);
                                lastMovePoint.waypointMarker.GetComponent<WaypointMarker>().atk = marker.GetComponent<WaypointMarker>();

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
                    SelectUnit(hit.transform.GetComponent<Unit>());
                }
            }
        }
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
        temp.pos = selectedUnit.transform.position + Vector3.up * 0.5f;
        waypoints.Add(temp);
        marker.GetComponent<WaypointMarker>().pos = temp.pos;
        marker.GetComponent<WaypointMarker>().SetInfo(waypoints.Count + " : START\nAP : " + selectedUnit.actionPoints);
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
