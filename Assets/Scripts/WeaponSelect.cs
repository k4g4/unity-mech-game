using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    Button weaponOne, weaponTwo, weaponThree, weaponFour;
    Text weaponOneText, weaponTwoText, weaponThreeText, weaponFourText;
    Text weaponOneAP, weaponTwoAP, weaponThreeAP, weaponFourAP;
    Text weaponOneAcc, weaponTwoAcc, weaponThreeAcc, weaponFourAcc;
    PlayerController pc;
    List<Text> weaponText = new List<Text>();

    void Awake()
    {
        pc = FindObjectOfType<PlayerController>();

        weaponOne = transform.GetChild(0).GetComponent<Button>();
        weaponTwo = transform.GetChild(1).GetComponent<Button>();
        weaponThree = transform.GetChild(2).GetComponent<Button>();
        weaponFour = transform.GetChild(3).GetComponent<Button>();

        weaponOneText = weaponOne.transform.GetChild(0).GetComponent<Text>();
        weaponTwoText = weaponTwo.transform.GetChild(0).GetComponent<Text>();
        weaponThreeText = weaponThree.transform.GetChild(0).GetComponent<Text>();
        weaponFourText = weaponFour.transform.GetChild(0).GetComponent<Text>();
        weaponText.Add(weaponOneText);
        weaponText.Add(weaponTwoText);
        weaponText.Add(weaponThreeText);
        weaponText.Add(weaponFourText);

        weaponOneAP = transform.GetChild(4).GetChild(0).GetComponent<Text>();
        weaponTwoAP = transform.GetChild(5).GetChild(0).GetComponent<Text>();
        weaponThreeAP = transform.GetChild(6).GetChild(0).GetComponent<Text>();
        weaponFourAP = transform.GetChild(7).GetChild(0).GetComponent<Text>();
        weaponText.Add(weaponOneAP);
        weaponText.Add(weaponTwoAP);
        weaponText.Add(weaponThreeAP);
        weaponText.Add(weaponFourAP);

        weaponOneAcc = transform.GetChild(8).GetChild(0).GetComponent<Text>();
        weaponTwoAcc = transform.GetChild(9).GetChild(0).GetComponent<Text>();
        weaponThreeAcc = transform.GetChild(10).GetChild(0).GetComponent<Text>();
        weaponFourAcc = transform.GetChild(11).GetChild(0).GetComponent<Text>();
        weaponText.Add(weaponOneAcc);
        weaponText.Add(weaponTwoAcc);
        weaponText.Add(weaponThreeAcc);
        weaponText.Add(weaponFourAcc);

        weaponOne.onClick.AddListener(() => OnClickOne());
        weaponTwo.onClick.AddListener(() => OnClickTwo());
        weaponThree.onClick.AddListener(() => OnClickThree());
        weaponFour.onClick.AddListener(() => OnClickFour());
    }

    void OnClickOne()
    {
        SelectWeapon(0);
    }

    void OnClickTwo()
    {
        SelectWeapon(1);
    }

    void OnClickThree()
    {
        SelectWeapon(2);
    }

    void OnClickFour()
    {
        SelectWeapon(3);
    }

    void SelectWeapon(int i)
    {
        if (pc.HasPointsToAttack(pc.selectedUnit, i))
        {
            pc.SetAttackType(i);
        }
        else
        {
            pc.RemoveWaypoint(pc.waypoints.Count - 1);
        }
        gameObject.SetActive(false);
    }

    void ResetText()
    {
        for(int i=0;i<4;i++)
        {
            weaponText[i].text = "N/A";
            weaponText[i + 4].text = "N/A";
            weaponText[i + 8].text = "N/A";
        }
    }

    public void SetWeaponText(Unit unit,Unit target)
    {
        ResetText();
        for(int i=0;i<unit.weapons.Count;i++)
        {
            weaponText[i].text = unit.weapons[i].wepName;
            weaponText[i+4].text = unit.weapons[i].apCost + "";
            if(pc.GetLastMovePoint())
                weaponText[i + 8].text = unit.weapons[i].AccuracyCalc(pc.GetLastMovePoint().pos,target.transform.position) + "";
            else
                weaponText[i + 8].text = unit.weapons[i].AccuracyCalc(unit.transform.position,target.transform.position) + "";
        }
    }
}
