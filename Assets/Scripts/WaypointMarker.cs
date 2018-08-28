using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointMarker : MonoBehaviour
{
    public Vector3 pos;
    Text infoBox;
    public WaypointMarker next;
    public WaypointMarker atk;
    LineRenderer lr,atkLR;

    void Awake()
    {
        atkLR = transform.GetChild(0).GetComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        infoBox = transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(pos);
        if(next) //Line renderer positions
        {
            lr.SetPosition(0, pos);
            lr.SetPosition(1, next.pos);
        }
        else
        {
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos);
        }

        if(atk)
        {
            atkLR.SetPosition(0, pos);
            atkLR.SetPosition(1, atk.pos);
        }
        else
        {
            atkLR.SetPosition(0, pos);
            atkLR.SetPosition(1, pos);
        }
    }
    
    public void SetInfo(string info)
    {
        infoBox.text = info;
    }
}
