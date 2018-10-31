using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortieController : MonoBehaviour
{
    GameObject sortieReservesContent, sortieActiveContent, sortieReadyBG;
    public GameObject sortieUnitButton;
    Text sortieUnitInfoText;
    int level;

    void Awake()
    {
        sortieReservesContent = GameObject.Find("SortieReservesContent");
        sortieActiveContent = GameObject.Find("SortieActiveContent");
        sortieReadyBG = GameObject.Find("SortieReadyBG");
        sortieUnitInfoText = GameObject.Find("SortieUnitInfoText").GetComponent<Text>();
    }

    public void ShowSortie()
    {
        GameController.instance.RemoveContent(sortieReservesContent);
        GameController.instance.RemoveContent(sortieActiveContent);
        GenerateUnitsList();
    }

    public void ShowBG(int level)
    {
        sortieReadyBG.SetActive(true);
        this.level = level;
    }

    void GenerateUnitsList()
    {
        for(int i=0;i<GameController.instance.units.Count;i++)
        {
            GameObject clone = Instantiate(sortieUnitButton, sortieReservesContent.transform);
            clone.GetComponent<SortieUnitButton>().sc = this;
            clone.GetComponent<SortieUnitButton>().unit = GameController.instance.units[i];
            clone.transform.GetChild(0).GetComponent<Text>().text = GameController.instance.units[i].unitName;
        }
    }

    public void ToggleActive(SortieUnitButton unit)
    {
        if(unit.activeUnit)
        {
            unit.activeUnit = false;
            unit.transform.SetParent(sortieReservesContent.transform);
        }
        else
        {
            unit.activeUnit = true;
            unit.transform.SetParent(sortieActiveContent.transform);
        }
    }

    public void SetUnitInfo(UnitData unit)
    {
        sortieUnitInfoText.text = unit.unitName;
    }

    public void StartSortie()
    {
        for(int i=0;i<sortieActiveContent.transform.childCount;i++)
        {
            GameController.instance.activeUnits.Add(sortieActiveContent.transform.GetChild(i).GetComponent<SortieUnitButton>().unit);
        }

        GameController.instance.LoadScene(level);
    }
}
