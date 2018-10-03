using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    public float moveSpeed, rotSpeed;

    Vector3 targetPos = new Vector3(1.3f,7f,-10f);
    Quaternion targetRot;
    List<GameObject> menuButtons = new List<GameObject>();
    List<GameObject> menuBG = new List<GameObject>();
    GameObject mainMenuButton,titleText;

    void Awake()
    {
        titleText = GameObject.Find("TitleText");
        mainMenuButton = GameObject.Find("MainMenuButton");

        menuButtons.Add(GameObject.Find("ExitButton"));
        menuButtons.Add(GameObject.Find("ArmoryButton"));
        menuButtons.Add(GameObject.Find("FactoryButton"));
        menuButtons.Add(GameObject.Find("SortieButton"));

        menuBG.Add(GameObject.Find("ExitBG"));
        menuBG.Add(GameObject.Find("FactoryBG"));
        menuBG.Add(GameObject.Find("ArmoryBG"));
        menuBG.Add(GameObject.Find("SortieBG"));

        HideAllMenus();
        ShowAllMenuButtons();
    }

    void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
	}

    public void MoveCamera(Vector3 pos, Quaternion rot)
    {
        targetPos = pos;
        targetRot = rot;
    }

    public void ReturnToMainMenu()
    {
        ShowAllMenuButtons();
        HideAllMenus();
    }

    public void ShowMenu(int i)
    {
        HideAllMenuButtons();
        HideAllMenus();
        menuBG[i].SetActive(true);
    }

    void HideAllMenuButtons()
    {
        for(int i=0;i<menuButtons.Count;i++)
        {
            menuButtons[i].SetActive(false);
        }
        mainMenuButton.SetActive(true);
        titleText.SetActive(false);
        HideAllMenus();
    }

    void ShowAllMenuButtons()
    {
        for(int i=0;i<menuButtons.Count;i++)
        {
            menuButtons[i].SetActive(true);
        }
        mainMenuButton.SetActive(false);
        titleText.SetActive(true);
    }

    void HideAllMenus()
    {
        for(int i=0;i<menuBG.Count;i++)
        {
            menuBG[i].SetActive(false);
        }
    }

    void ShowAllMenus()
    {
        for (int i = 0; i < menuBG.Count; i++)
        {
            menuBG[i].SetActive(true);
        }
    }

    public void AcceptExit()
    {
        Application.Quit();
    }

    public void CancelExit()
    {
        HideAllMenus();
        ShowAllMenuButtons();
    }
}
