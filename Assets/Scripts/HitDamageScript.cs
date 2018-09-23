using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDamageScript : MonoBehaviour
{
    public float speed = 1f;
    public float destroyTime = 1f;
    RectTransform rt;
    Color color;
    Transform targetPos;
    float alpha = 0;
    void Awake()
    {
        color = GetComponent<Text>().color;
        rt = GetComponent<RectTransform>();
        StartCoroutine(FadeTextToZeroAlpha(destroyTime, GetComponent<Text>()));
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        //color = Color.Lerp(color, Color.clear,0);
        //alpha += Time.deltaTime*10;
        rt.Translate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
