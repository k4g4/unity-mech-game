using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEBUGSpawnWeapon : MonoBehaviour
{
    InputField id;

    void Awake()
    {
        id = GameObject.Find("DEBUGWeaponID").GetComponent<InputField>();
        GetComponent<Button>().onClick.AddListener(SpawnWeapon);
    }

    void SpawnWeapon()
    {
        FindObjectOfType<FactoryController>().BuildWeapon(int.Parse(id.text));
    }
}
