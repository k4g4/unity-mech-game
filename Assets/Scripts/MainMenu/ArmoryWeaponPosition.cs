using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryWeaponPosition : MonoBehaviour
{
    ArmoryController ac;
    public WeaponData weaponData;
    public int pos;
    Text wepName;

    void Awake()
    {
        ac = FindObjectOfType<ArmoryController>();
        wepName = transform.GetChild(0).GetComponent<Text>();
        GetComponent<Button>().onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        if (!ac.selectedUnit)
            return;
        if(!weaponData)
        {
            ac.ShowArmoryWeapons();
            ac.selectedWepPos = this;
        }
        else
        {
            ac.selectedUnit.weapons.Remove(weaponData);
            Deselect();
        }
    }

    public void SetInfo(WeaponData weapon)
    {
        weaponData = weapon;

        wepName.text = weaponData.weaponName;
        weaponData.weaponPos = pos;
        weaponData.inUse = true;
        weaponData.unit = ac.selectedUnit;
    }

    void Deselect()
    {
        weaponData.weaponPos = -1;
        weaponData.unit = null;
        weaponData.inUse = false;
        ClearInfo();
    }

    public void ClearInfo()
    {
        weaponData = null;
        wepName.text = "N/A";
    }
}
