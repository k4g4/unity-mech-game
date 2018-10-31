using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class EnemyController : MonoBehaviour
{
    PlayerController pc;

	void Awake ()
    {
        pc = FindObjectOfType<PlayerController>();
	}
	
    public void StartTurn()
    {
        Timing.RunCoroutine(WaypointDelay());
    }

    IEnumerator<float> WaypointDelay()
    {
        for(int x=0;x<pc.teamTwoList.Count;x++)
        {
            pc.SelectUnit(pc.teamTwoList[x]);
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
                for (int i = 0; i < targetable.Count; i++)
                {
                    int tgt = Random.Range(0, targetable.Count);
                    pc.AddWaypoint(1, 10, targetable[tgt].transform.position + Vector3.up * 0.5f, "enemy", targetable[tgt]);
                    pc.SetAttackType(Random.Range(0, pc.teamTwoList[x].weapons.Count));
                    yield return Timing.WaitForSeconds(1f);

                }
            }
            pc.Execute();
            yield return Timing.WaitForSeconds(18f);
        }
        Debug.Log("enemy ending turn");
        pc.EndTurn();
    }
}
