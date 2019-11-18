using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryUnitButton : MonoBehaviour
{
    public ArmoryController ac;
    public UnitData unit;
    Text unitName;

    void Awake()
    {
        unitName = transform.GetChild(0).GetComponent<Text>();
        GetComponent<Button>().onClick.AddListener(ButtonClick);
    }

    public void SetData()
    {
        unitName.text = unit.unitName;
    }

    void ButtonClick()
    {
        ac.SetUnit(unit);
    }
}
