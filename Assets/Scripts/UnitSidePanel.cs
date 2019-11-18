using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSidePanel : MonoBehaviour
{
    Text unitName, health, ap;
    public Unit unit;

    void Awake()
    {
        unitName = transform.GetChild(0).GetComponent<Text>();
        health = transform.GetChild(1).GetComponent<Text>();
        ap = transform.GetChild(2).GetComponent<Text>();
    }

    void Start()
    {
        unitName.text = unit.unitName;
    }

    public void UpdateInfo()
    {
        health.text = "HP:"+unit.health;
        ap.text = "AP"+unit.actionPoints;
    }
}
