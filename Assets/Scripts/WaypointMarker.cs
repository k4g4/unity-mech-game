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
    SpriteRenderer sprite;
    Color32 blue, red;
    void Awake()
    {
        blue = new Color32(100, 100, 255, 100);
        red = new Color32(255, 100, 100, 100);
        sprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        atkLR = transform.GetChild(0).GetComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        infoBox = transform.GetChild(0).GetComponent<Text>();
    }

    void Update() //BUG | Attack line renderer needs to point at last move pos instead of the other way around
    {
        //transform.position = Camera.main.WorldToScreenPoint(pos);
        transform.position = pos;
        if(next) //Line renderer positions
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, next.pos);
        }
        else
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, pos);
        }

        if(atk)
        {
            atkLR.SetPosition(0, transform.position);
            atkLR.SetPosition(1, atk.pos);
        }
        else
        {
            atkLR.SetPosition(0, transform.position);
            atkLR.SetPosition(1, pos);
        }
    }
    
    public void SetInfo(string info)
    {
        infoBox.text = info;
    }
    
    public void SetColor(bool isBlue)
    {
        if (isBlue)
            sprite.color = blue;
        else
            sprite.color = red;
    }
}
