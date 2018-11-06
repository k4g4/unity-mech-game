using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDict : MonoBehaviour
{
    public static PartDict instance = null;

    public Dictionary<int, GameObject> partDict = new Dictionary<int, GameObject>();

    public GameObject ac231, ac234, guidMiss, shot30,plasma;

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

        partDict.Add(0, ac231);
        partDict.Add(1, ac234);
        partDict.Add(2, guidMiss);
        partDict.Add(3, shot30);
        partDict.Add(4, plasma);
        
    }
}
