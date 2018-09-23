using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonHover : MonoBehaviour
     , IPointerClickHandler
     , IPointerEnterHandler
{     

    public int menuID = 0;
    public Vector3 targetPos;
    public Quaternion targetRot;
    public bool isMainMenuButton = false;
    MainMenuCameraController mmcc;

    void Awake()
    {
        mmcc = FindObjectOfType<MainMenuCameraController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(menuID >= 0)
            mmcc.MoveCamera(targetPos, targetRot);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMainMenuButton)
            mmcc.ReturnToMainMenu();
        else
            mmcc.ShowMenu(menuID);
    }
}
