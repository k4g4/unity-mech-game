using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointMarker : MonoBehaviour
{
    public Vector3 pos;
    Text infoBox;
    WaypointMarker next;

    void Awake()
    {
        infoBox = transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(pos);
    }
    
    public void SetInfo(string info)
    {
        infoBox.text = info;
    }
}
