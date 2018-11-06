using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenarioZeroA : MonoBehaviour
{
    PlayerController pc;
    GameObject resultScreen;

    void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
    }

	void Start ()
    {
		
	}
}
