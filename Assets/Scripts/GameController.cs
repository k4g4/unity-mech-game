using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UnitData : ScriptableObject
{
    public string unitName;
    public int health;
    public int maxHealth;
    public int maxActionPoints;
    public List<int> weapons = new List<int>();

}

public class WeaponData : ScriptableObject
{
    public string unitName;
    public int health;
    public int maxHealth;
    public int maxActionPoints;
    public List<int> weapons = new List<int>();

}


public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public List<UnitData> units = new List<UnitData>();
    public List<WeaponData> weapons = new List<WeaponData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SaveProfile();
    }

    public void LoadProfile()
    {
        string line;
        StreamReader s = File.OpenText(Application.dataPath + "/save.dat");
        line = s.ReadLine();
        while (line != null)
        {
            line = s.ReadLine();
        }
    }

    public void SaveProfile()
    {
        StreamWriter w = new StreamWriter(Application.dataPath + "/save.dat");
        //w = File.CreateText(Application.dataPath + "save.dat");
        w.WriteLine("hello world");
        w.Close();
    }

    void StartMission()
    {

    }
}
