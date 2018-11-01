using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    Button weaponOne, weaponTwo, weaponThree, weaponFour;
    Text weaponOneText, weaponTwoText, weaponThreeText, weaponFourText;
    PlayerController pc;

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
        gameObject.SetActive(false);
    }

    public void SetWeaponText(Unit unit)
    {
        weaponOneText.text = unit.weapons[0].wepName;
        weaponTwoText.text = unit.weapons[1].wepName;
        weaponThreeText.text = unit.weapons[2].wepName;
        weaponFourText.text = unit.weapons[3].wepName;
    }
}
