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
        pc.SetAttackType(0);
        gameObject.SetActive(false);
    }

    void OnClickTwo()
    {
        pc.SetAttackType(1);
        gameObject.SetActive(false);
    }

    void OnClickThree()
    {
        pc.SetAttackType(2);
        gameObject.SetActive(false);
    }

    void OnClickFour()
    {
        pc.SetAttackType(3);
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
