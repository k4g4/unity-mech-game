using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class UnitData : ScriptableObject
{
    public int unitID=0;
    public string unitName="";
    public int unitType=0;
    public int health=0;
    public int maxHealth=0;
    public int maxActionPoints=0;
    public int exp=0;
    public int level=0;
    public List<WeaponData> weapons = new List<WeaponData>();
}

public class WeaponData : ScriptableObject
{
    public int weaponID=0;
    public string weaponName="";
    public int weaponPos=0;
    public int weaponType=0;
    public bool inUse = false;
    public UnitData unit;
}


public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public List<UnitData> units = new List<UnitData>();
    public List<WeaponData> weapons = new List<WeaponData>();
    public List<UnitData> activeUnits = new List<UnitData>();
    string playerName="";
    int fuel=0, ammo=0, steel=0;

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
    }

    void Start()
    {
        LoadProfile();
    }

    public void LoadProfile()
    {
        string line;
        StreamReader s = File.OpenText(Application.dataPath + "/save.dat");
        line = s.ReadLine();
        playerName = line;
        line = s.ReadLine();
        fuel = int.Parse(line);
        line = s.ReadLine();
        ammo = int.Parse(line);
        line = s.ReadLine();
        steel = int.Parse(line);
        line = s.ReadLine();
        if (line==null)
            return;
        bool resume = false;
        while (line != "###UNITS###") //read in weapons
        {
            WeaponData wepData = ScriptableObject.CreateInstance<WeaponData>();
            line = s.ReadLine();
            if (line == "###UNITS###")
            {
                resume = true;
                break;
            }
            wepData.weaponID = int.Parse(line);
            line = s.ReadLine();
            wepData.weaponName = line;
            line = s.ReadLine();
            wepData.weaponPos = int.Parse(line);
            line = s.ReadLine();
            wepData.weaponType = int.Parse(line);
            line = s.ReadLine();
            wepData.inUse = bool.Parse(line);
            weapons.Add(wepData);
        }
        while (line != null)
        {
            line = s.ReadLine();
            if (line == "###UNIT###") //create new unit
            {
                resume = false;
                UnitData data = ScriptableObject.CreateInstance<UnitData>();
                line = s.ReadLine();
                data.unitID = int.Parse(line);
                line = s.ReadLine();
                data.unitName = line;
                line = s.ReadLine();
                data.unitType = int.Parse(line);
                line = s.ReadLine();
                data.health = int.Parse(line);
                line = s.ReadLine();
                data.maxHealth = int.Parse(line);
                line = s.ReadLine();
                data.maxActionPoints = int.Parse(line);
                line = s.ReadLine();
                data.exp = int.Parse(line);
                line = s.ReadLine();
                data.level = int.Parse(line);
                line = s.ReadLine();
                while(line != "###UNITEND###")
                {
                    int wepID = int.Parse(line);
                    for (int i = 0; i < weapons.Count; i++) //assign unit weapons
                    {
                        if (wepID == weapons[i].weaponID)
                        {
                            weapons[i].unit = data;
                            data.weapons.Add(weapons[i]);
                            break;
                        }
                    }
                    line = s.ReadLine();
                }
                units.Add(data);
            }
        }
        s.Close();
    }

    public void SaveProfile()
    {
        StreamWriter w = new StreamWriter(Application.dataPath + "/save.dat");
        //w = File.CreateText(Application.dataPath + "save.dat");
        w.WriteLine(playerName);
        w.WriteLine(fuel);
        w.WriteLine(ammo);
        w.WriteLine(steel);
        w.WriteLine("###WEAPONS###");
        for (int i = 0; i < weapons.Count; i++)
        {
            w.WriteLine(weapons[i].weaponID);
            w.WriteLine(weapons[i].weaponName);
            w.WriteLine(weapons[i].weaponPos);
            w.WriteLine(weapons[i].weaponType);
            w.WriteLine(weapons[i].inUse);
        }
        w.WriteLine("###UNITS###");
        for (int i = 0; i < units.Count; i++)
        {
            w.WriteLine("###UNIT###");
            w.WriteLine(units[i].unitID);
            w.WriteLine(units[i].unitName);
            w.WriteLine(units[i].unitType);
            w.WriteLine(units[i].health);
            w.WriteLine(units[i].maxHealth);
            w.WriteLine(units[i].maxActionPoints);
            w.WriteLine(units[i].exp);
            w.WriteLine(units[i].level);
            for (int j = 0; j < units[i].weapons.Count; j++)
            {
                w.WriteLine(units[i].weapons[j].weaponID);
            }
            w.WriteLine("###UNITEND###");
        }
        w.Close();
    }

    void StartMission()
    {

    }

    public void RemoveContent(GameObject content)
    {
        int count = content.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }


    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(units[0].weapons.Count);
        }
    }
}
