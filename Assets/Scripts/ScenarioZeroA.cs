using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MEC;

public class ScenarioZeroA : MonoBehaviour
{
    PlayerController pc;
    GameObject resultScreen,enemySpawnPoint;
    public GameObject enemyACMech, enemyShotgunMech;
    void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        enemySpawnPoint = GameObject.Find("EnemySpawnPoint");
    }

	void Start ()
    {
        Timing.RunCoroutine(WaveEvents());
	}

    IEnumerator<float> WaveEvents()
    {
        while(pc.turnCount != 2)
        {
            yield return 0;
        }

        while(pc.turnCount != 6)
        {
            yield return 0;
        }
    }
}
