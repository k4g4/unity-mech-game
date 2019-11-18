using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryController : MonoBehaviour
{
    public GameObject armoryUnitButton, armoryWeaponButton;
    ArmoryWeaponPosition mechLShould, mechRShould, mechLArm, mechRArm;
    GameObject mechImage, armoryUnitContent, armoryWeaponContent, armoryWeaponView,armoryUnitView;
    public UnitData selectedUnit;
    public ArmoryWeaponPosition selectedWepPos;

    int currentUnitID;

    void Awake()
    {
        mechImage = GameObject.Find("ArmoryMechImage");
        mechLShould = GameObject.Find("ArmoryLShoulderButton").GetComponent<ArmoryWeaponPosition>();
        mechRShould = GameObject.Find("ArmoryRShoulderButton").GetComponent<ArmoryWeaponPosition>();
        mechLArm = GameObject.Find("ArmoryLArmButton").GetComponent<ArmoryWeaponPosition>();
        mechRArm = GameObject.Find("ArmoryRArmButton").GetComponent<ArmoryWeaponPosition>();
        armoryWeaponView = GameObject.Find("ArmoryWeaponView");
        armoryUnitView = GameObject.Find("ArmoryUnitView");
        armoryUnitContent = GameObject.Find("ArmoryUnitContent");
        armoryWeaponContent = GameObject.Find("ArmoryWeaponContent");
        
    }

    void GenerateUnits()
    {
        for(int i=0;i<GameController.instance.units.Count;i++)
        {
            GameObject clone = Instantiate(armoryUnitButton, armoryUnitContent.transform);
            clone.GetComponent<ArmoryUnitButton>().ac = this;
            clone.GetComponent<ArmoryUnitButton>().unit = GameController.instance.units[i];
            clone.GetComponent<ArmoryUnitButton>().SetData();
        }
    }

    void GenerateWeapons()
    {
        for (int i = 0; i < GameController.instance.weapons.Count; i++)
        {
            if (GameController.instance.weapons[i].inUse)
                continue;
            GameObject clone = Instantiate(armoryWeaponButton, armoryWeaponContent.transform);
            clone.GetComponent<ArmoryWeaponButton>().ac = this;
            clone.GetComponent<ArmoryWeaponButton>().weapon = GameController.instance.weapons[i];
            clone.transform.GetChild(0).GetComponent<Text>().text = GameController.instance.weapons[i].weaponName;
        }
    }

    public void ShowArmoryWeapons()
    {
        armoryWeaponView.SetActive(true);
        GameController.instance.RemoveContent(armoryWeaponContent);
        GenerateWeapons();
    }

    public void SetWeapon(WeaponData weapon)
    {
        selectedWepPos.SetInfo(weapon);
        selectedUnit.weapons.Add(weapon);
        armoryWeaponView.SetActive(false);
        GameController.instance.SaveProfile();
    }

    public void ShowArmory()
    {
        armoryWeaponView.SetActive(true);
        GameController.instance.RemoveContent(armoryUnitContent);
        GameController.instance.RemoveContent(armoryWeaponContent);
        GenerateUnits();
        GenerateWeapons();
        ResetWeaponPositions();
        armoryWeaponView.SetActive(false);
    }

    public void SetUnit(UnitData unit)
    {
        selectedUnit = unit;
        switch(unit.unitType)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                Debug.Log("ARMORY: Invalid Mech Type : " + unit.unitType);
                break;
        }
        ResetWeaponPositions();
        for(int i=0;i<selectedUnit.weapons.Count;i++)
        {
            SetWeapon(selectedUnit.weapons[i], selectedUnit.weapons[i].weaponPos);
        }
    }

    void ResetWeaponPositions()
    {
        mechLArm.ClearInfo();
        mechRArm.ClearInfo();
        mechLShould.ClearInfo();
        mechRShould.ClearInfo();
    }

    
    public void SetWeapon(WeaponData weapon, int pos)
    {
        ArmoryWeaponPosition wep = mechLArm;
        switch(pos)
        {
            case 0: //larm
                wep = mechLArm;
                break;
            case 1: //rarm
                wep = mechRArm;
                break;
            case 2: //lshoulder
                wep = mechLShould;
                break;
            case 3:
                wep = mechRShould;
                break; //rshoulder
            default:
                Debug.Log("ARMORY: Invalid Part Position : " + pos);
                break;
        }
        wep.SetInfo(weapon);
        armoryWeaponView.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            if (armoryWeaponView.activeInHierarchy)
                armoryWeaponView.SetActive(false);
    }
}
