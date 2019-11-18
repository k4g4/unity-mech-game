﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    GameObject mouseInfoBoxBG;
    Text mouseInfoBoxText,unitInfoText,turnCounter;
    public Unit selectedUnit;
    public GameObject hitDamage;
    Canvas canvas;
    GameObject pauseBG;
    bool isPaused = false;

    void Awake()
    {
        pauseBG = GameObject.Find("PauseBG");
        mouseInfoBoxBG = GameObject.Find("MouseInfoBoxBG");
        mouseInfoBoxText = GameObject.Find("MouseInfoBoxText").GetComponent<Text>();
        unitInfoText = GameObject.Find("UnitInfoText").GetComponent<Text>();
        turnCounter = GameObject.Find("TurnCounter").GetComponent<Text>();
        canvas = FindObjectOfType<Canvas>();
        pauseBG.SetActive(false);
    }

	
    public void SetMouseInfoActive(bool on)
    {
        mouseInfoBoxBG.SetActive(on);
    }

    public void SetMouseInfo(string text)
    {
        mouseInfoBoxText.text = text;
    }

    public void SetTurnCounter(int count)
    {
        turnCounter.text = "TURN\n" + count; 
    }

	void Update ()
    {
        MouseRaycast();
        if(selectedUnit)
        {
            unitInfoText.text = "Unit Info:"
                                + "\nName : " + selectedUnit.unitName
                                + "\nHealth : " + selectedUnit.health + "\\" + selectedUnit.maxHealth
                                + "\nAP : " + selectedUnit.actionPoints + "\\" + selectedUnit.maxActionPoints;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
	}

    public void TogglePause()
    {
        if(!isPaused)
        {
            pauseBG.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseBG.SetActive(false);
            Time.timeScale = 1;
        }
        isPaused = !isPaused;
    }

    public void Damage(Unit tgt, string dam)
    {
        if(hitDamage)
        {
            GameObject clone = Instantiate(hitDamage, canvas.transform);
            clone.GetComponent<RectTransform>().localPosition = Camera.main.WorldToViewportPoint(tgt.transform.position) + new Vector3(Random.Range(-40f, 40f), Random.Range(0f, 80f), Random.Range(-40f, 40f));
            clone.GetComponent<Text>().text = dam;
        }
    }

    void MouseRaycast() //Check what mouse is hitting
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f))
        {
            mouseInfoBoxBG.transform.position = Input.mousePosition + new Vector3(180,-100,0);

            if (hit.transform.GetComponent<Unit>())
            {
                Unit unit = hit.transform.GetComponent<Unit>();
                mouseInfoBoxBG.SetActive(true);
                mouseInfoBoxText.text = "Name: " + unit.unitName + "\nHealth: " + unit.health + "\nAP: " + unit.actionPoints + "\nAttack: " + unit.attack;
            }
            else if(mouseInfoBoxBG.gameObject.activeInHierarchy)
            {
                mouseInfoBoxBG.SetActive(false);
            }
        }
    }

}
