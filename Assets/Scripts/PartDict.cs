using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDict : MonoBehaviour
{

    Dictionary<int, GameObject> partDict = new Dictionary<int, GameObject>();

    public GameObject ac231, ac234, guidMiss;

    void Awake()
    {
        partDict.Add(0, ac231);
        partDict.Add(1, ac234);
        partDict.Add(2, guidMiss);
    }
}
