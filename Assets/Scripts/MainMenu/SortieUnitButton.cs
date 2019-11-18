using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SortieUnitButton : MonoBehaviour, IPointerEnterHandler
{
    public UnitData unit;
    public SortieController sc;
    public bool activeUnit;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        sc.ToggleActive(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sc.SetUnitInfo(unit);
    }
}
