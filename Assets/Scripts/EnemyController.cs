﻿using System.Collections;
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
                if(Physics.Raycast(moveArea.transform.position+Vector3.up+new Vector3(Random.Range(-5,5),0,Random.Range(-5,5)) ,Vector3.down,out hit,5,1<<9))
                {
                    pc.AddWaypoint(0, Mathf.RoundToInt(Vector3.Distance(pc.teamTwoList[x].transform.position, hit.point)), hit.point + Vector3.up * 0.5f, "move", null); //check vector up displacement, increases by half meter every click
                }
                pc.selectedUnit.aiWaypoints.RemoveAt(0);
            }
            List<Unit> targetable = new List<Unit>();
            for (int i = 0; i < pc.teamOneList.Count; i++)
            {
                if (!Physics.Raycast(pc.teamTwoList[x].transform.position + Vector3.up * 0.5f, pc.teamOneList[i].transform.position + Vector3.up * 0.5f - pc.teamTwoList[x].transform.position + Vector3.up * 0.5f, Vector3.Distance(pc.teamTwoList[x].transform.position, pc.teamOneList[i].transform.position), 1 << 9))
                {
                    targetable.Add(pc.teamOneList[i]);
                }
            }
            if(targetable.Count>0)
            {
                for (int i = 0; i < targetable.Count/2; i++)
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
            pc.Execute();
            while(pc.waypoints.Count>0 || cc.isAnimating)
            {
                yield return 0;
            }
            yield return Timing.WaitForSeconds(1f);
        }
        Debug.Log("enemy ending turn");
        pc.EndTurn();
    }
}