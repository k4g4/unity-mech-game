using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    GameObject mouseInfoBoxBG;
    Text mouseInfoBoxText,unitInfoText;
    public Unit selectedUnit;
    public GameObject hitDamage;
    Canvas canvas;
    

    void Awake()
    {
        mouseInfoBoxBG = GameObject.Find("MouseInfoBoxBG");
        mouseInfoBoxText = GameObject.Find("MouseInfoBoxText").GetComponent<Text>();
        unitInfoText = GameObject.Find("UnitInfoText").GetComponent<Text>();
        canvas = FindObjectOfType<Canvas>();
    }

	
    public void SetMouseInfoActive(bool on)
    {
        mouseInfoBoxBG.SetActive(on);
    }

    public void SetMouseInfo(string text)
    {
        mouseInfoBoxText.text = text;
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
	}

    public void Damage(Unit tgt, string dam)
    {
        GameObject clone = Instantiate(hitDamage, canvas.transform);
        hitDamage.GetComponent<RectTransform>().position = Camera.main.WorldToViewportPoint(tgt.transform.position) + new Vector3(Random.Range(-40f,40f),Random.Range(0f,80f),Random.Range(-40f,40f));
        hitDamage.GetComponent<Text>().text = dam;
    }

    void MouseRaycast() //Check what mouse is hitting
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f))
        {
            mouseInfoBoxBG.transform.position = Input.mousePosition + new Vector3(120,-50,0);

            if (hit.transform.GetComponent<Unit>())
            {
                Unit unit = hit.transform.GetComponent<Unit>();
                mouseInfoBoxBG.SetActive(true);
                mouseInfoBoxText.text = "Name: " + unit.name + "\nHealth: " + unit.health + "\nAP: " + unit.actionPoints + "\nAttack: " + unit.attack;
            }
            else if(mouseInfoBoxBG.gameObject.activeInHierarchy)
            {
                mouseInfoBoxBG.SetActive(false);
            }
        }
    }

}
