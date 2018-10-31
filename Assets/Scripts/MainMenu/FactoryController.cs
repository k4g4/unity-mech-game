using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryController : MonoBehaviour
{
    public InputField factoryFuelField, factoryAmmoField, factorySteelField,factoryMechNameField;

    void Awake()
    {
        factoryAmmoField = GameObject.Find("FactoryAmmoField").GetComponent<InputField>();
        factoryFuelField = GameObject.Find("FactoryFuelField").GetComponent<InputField>();
        factorySteelField = GameObject.Find("FactorySteelField").GetComponent<InputField>();
        factoryMechNameField = GameObject.Find("FactoryMechNameField").GetComponent<InputField>();
    }

    public void BuildMech(int i)
    {
        UnitData mech = ScriptableObject.CreateInstance<UnitData>();
        if (GameController.instance.units.Count < 1)
            mech.unitID = 0;
        else
            mech.unitID = GameController.instance.units[GameController.instance.units.Count - 1].unitID + 1;
        mech.unitName = factoryMechNameField.GetComponent<InputField>().text;
        mech.unitType = i;
        mech.health = 100;
        mech.maxHealth = 100;
        mech.maxActionPoints = 30;

        GameController.instance.units.Add(mech);
        GameController.instance.SaveProfile();
    }

    public void BuildWeapon()
    {
        WeaponData weapon = ScriptableObject.CreateInstance<WeaponData>();
        if (GameController.instance.weapons.Count < 1)
            weapon.weaponID = 0;
        else
            weapon.weaponID = GameController.instance.weapons[GameController.instance.weapons.Count - 1].weaponID + 1;
        weapon.weaponName = "Autocannon 20";
        GameController.instance.weapons.Add(weapon);
        GameController.instance.SaveProfile();
    }
}
