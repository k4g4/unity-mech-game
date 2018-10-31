using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryWeaponButton : MonoBehaviour
{
    public ArmoryController ac;
    public WeaponData weapon;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        ac.SetWeapon(weapon);
    }
}
