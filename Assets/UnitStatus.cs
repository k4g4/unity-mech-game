using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatus : MonoBehaviour
{
    public Unit unit;
    public Image hp, ap;
    public Text hpText, apText;
    public Image team;
    int maxHP, curHP, maxAP, curAP;
    GameObject container;
    public Color32 blue, red;

	void Awake ()
    {

        container = transform.GetChild(0).gameObject;
        team = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        hp = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
        hpText = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>();
        ap = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();
        apText = transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>();
	}

    void Start()
    {
        if (unit.gameObject.layer == 10)
            team.color = blue;
        else
            team.color = red;
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + Vector3.forward + Vector3.up + Vector3.right);
        Vector3 viewPos = Camera.main.WorldToViewportPoint(unit.transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            container.SetActive(true);
        }
        else
        {
            container.SetActive(false);
        }
    }

    public void Damage(int dmg)
    {
        UpdateInfo();
    }

    public void SetAP(int cost)
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        maxHP = unit.maxHealth;
        curHP = unit.health;
        maxAP = unit.maxActionPoints;
        curAP = unit.actionPoints;
        if (curHP < 0)
            curHP = 0;
        if (curAP < 0)
            curAP = 0;
        hp.rectTransform.localScale = new Vector3((float)curHP / maxHP, 1f, 1f);
        ap.rectTransform.localScale = new Vector3((float)curAP / maxAP, 1f, 1f);
    }
}
