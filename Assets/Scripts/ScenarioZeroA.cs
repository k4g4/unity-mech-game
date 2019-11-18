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
    public AudioClip alarm;
    GameObject notificationText;

    void Awake()
    {
        notificationText = GameObject.Find("NotificationText");
        pc = FindObjectOfType<PlayerController>();
        enemySpawnPoint = GameObject.Find("EnemySpawnPoint");
        resultScreen = GameObject.Find("ResultScreen");
        resultScreen.SetActive(false);
        notificationText.SetActive(false);
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
            clone.GetComponent<AirstrikeScript>().radius = Random.Range(5f, 10f);
        }
    }

    void SpawnEnemy(GameObject enemy, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject clone = Instantiate(enemy);
            clone.transform.position = enemySpawnPoint.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            pc.teamTwoList.Add(clone.GetComponent<Unit>());
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP1"));
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP2"));
            clone.GetComponent<Unit>().aiWaypoints.Add(GameObject.Find("WP3"));
        }
    }

    IEnumerator<float> WaveEvents()
    {
        while (pc.turnCount != 2) { yield return 0; }
        SpawnAirstrikes(1);
        SpawnEnemy(enemyShotgunMech, 4);

        while (pc.turnCount != 3) { yield return 0; }
        SpawnAirstrikes(2);
  
        while (pc.turnCount != 4) { yield return 0; }
        GameObject.Find("CameraBox").transform.position = new Vector3(-8.8f, 30f, -80f);
        GameObject.Find("SoundController").GetComponent<AudioSource>().PlayOneShot(alarm,1f);
        SpawnEnemy(enemyBoss, 1);
        SpawnAirstrikes(3);

        notificationText.SetActive(true);
        notificationText.GetComponent<Text>().text = "-WARNING-\nLARGE ENEMY DETECTED";
        yield return Timing.WaitForSeconds(3f);
        notificationText.SetActive(false);

        while (pc.turnCount != 5){ yield return 0; }

        SpawnAirstrikes(3);
        //resultScreen.SetActive(true);
        //yield return Timing.WaitForSeconds(5f);
        //SceneManager.LoadScene(0);

        while (pc.turnCount != 7) { yield return 0; }

        SpawnAirstrikes(3);

    }
}