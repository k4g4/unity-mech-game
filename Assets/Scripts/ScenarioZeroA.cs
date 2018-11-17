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
    public GameObject enemyACMech, enemyShotgunMech,enemyBoss;
    public GameObject airstrikeNode;


    void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        enemySpawnPoint = GameObject.Find("EnemySpawnPoint");
        resultScreen = GameObject.Find("ResultScreen");
        resultScreen.SetActive(false);
    }

	void Start ()
    {
        Timing.RunCoroutine(WaveEvents());
	}
    void SpawnAirstrikes(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject clone = Instantiate(airstrikeNode);
            clone.transform.position = GameObject.Find("AirstrikeZone").transform.position;
            clone.transform.position += new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
            clone.GetComponent<AirstrikeScript>().radius = Random.Range(1f, 10f);
        }
    }

    IEnumerator<float> WaveEvents()
    {
        while (pc.turnCount != 2) { yield return 0; }
        SpawnAirstrikes(3);

        while (pc.turnCount != 3) { yield return 0; }

        for (int i = 0; i < 4; i++)
        {
            GameObject clone = Instantiate(enemyShotgunMech);
            clone.transform.position = enemySpawnPoint.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            pc.teamTwoList.Add(clone.GetComponent<Unit>());
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP1"));
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP2"));
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP3"));
        }
        SpawnAirstrikes(5);
        while (pc.turnCount != 4) { yield return 0; }
        
        for (int i = 0; i < 1; i++)
        {
            GameObject clone = Instantiate(enemyBoss);
            clone.transform.position = enemySpawnPoint.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            pc.teamTwoList.Add(clone.GetComponent<Unit>());
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP1"));
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP2"));
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP3"));
        }

        SpawnAirstrikes(3);
        while (pc.turnCount != 5){ yield return 0; }

        SpawnAirstrikes(10);
        //resultScreen.SetActive(true);
        //yield return Timing.WaitForSeconds(5f);
        //SceneManager.LoadScene(0);
    }
}