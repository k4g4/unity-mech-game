using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EnemyController : MonoBehaviour
{
    PlayerController pc;
    CameraController cc;

	void Awake ()
    {
        cc = FindObjectOfType<CameraController>();
        pc = FindObjectOfType<PlayerController>();
	}

    void Start()
    {
        for(int i=0;i<pc.teamTwoList.Count;i++)
        {
            pc.teamTwoList[i].aiWaypoints.Add(GameObject.Find("WP1"));
            pc.teamTwoList[i].aiWaypoints.Add(GameObject.Find("WP2"));
            pc.teamTwoList[i].aiWaypoints.Add(GameObject.Find("WP3"));

        }
    }
	
    public void StartTurn()
    {
        Timing.RunCoroutine(WaypointDelay());
    }

    IEnumerator<float> WaypointDelay()
    {
        for (int x = 0; x < pc.teamTwoList.Count; x++)
        {
            pc.SelectUnit(pc.teamTwoList[x]);
            if (pc.selectedUnit.aiWaypoints.Count>0)
            {
                GameObject moveArea = pc.selectedUnit.aiWaypoints[0];
                RaycastHit hit;
                if(Physics.Raycast(moveArea.transform.position+Vector3.up+new Vector3(Random.Range(-5f,5f),0,Random.Range(-5f,5f)) ,Vector3.down,out hit,5,1<<9))
                {
                    pc.AddWaypoint(0, Mathf.RoundToInt(Vector3.Distance(pc.teamTwoList[x].transform.position, hit.point)), hit.point + Vector3.up * 0.5f, "move", null); //check vector up displacement, increases by half meter every click
                }
                pc.selectedUnit.aiWaypoints.RemoveAt(0);
            }
            List<Unit> targetable = new List<Unit>();
            Collider[] collisions = Physics.OverlapSphere(pc.GetLastMovePoint().pos, 20f, 1 << 11);
            for (int i = 0; i < collisions.Length; i++)
            {
                Debug.DrawRay(pc.selectedUnit.transform.position, collisions[i].transform.position - pc.selectedUnit.transform.position);
                if (!Physics.Raycast(pc.selectedUnit.transform.position, collisions[i].transform.position - pc.selectedUnit.transform.position, Vector3.Distance(pc.selectedUnit.transform.position, collisions[i].transform.position), 1 << 9))
                {
                    targetable.Add(collisions[i].GetComponent<Unit>());
                }
            }
            if(targetable.Count>0)
            {
                for (int i = 0; i < Mathf.CeilToInt(targetable.Count/2); i++)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                        continue;
                    int tgt = Random.Range(0, targetable.Count);
                    pc.AddWaypoint(1, 10, targetable[tgt].transform.position + Vector3.up * 0.5f, "enemy", targetable[tgt]);
                    pc.SetAttackType(Random.Range(0, pc.teamTwoList[x].weapons.Count));
                    yield return Timing.WaitForSeconds(1f);

                }
            }
            if(pc.waypoints.Count>0)
                pc.Execute();
            while(pc.waypoints.Count>0 || cc.isAnimating)
            {
                yield return 0;
            }
            yield return Timing.WaitForSeconds(.5f);
        }
        Debug.Log("enemy ending turn");
        yield return Timing.WaitForOneFrame;
        pc.EndTurn();
    }
}
