using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;

public class CINE_UnitInfoText : MonoBehaviour
{
    public float startDelay = 0f;
    public float textDelay = 0.05f;
    Text textBody;
	// Use this for initialization
	void Awake ()
    {
        textBody = GetComponent<Text>();
	}

    void Start()
    {
        Timing.RunCoroutine(TextDelay(textBody.text));
    }

    IEnumerator<float> TextDelay(string text)
    {
        int textCount = text.Length;
        textBody.text = "";
        yield return Timing.WaitForSeconds(startDelay);

        for (int i = 0; i < textCount; i++)
        {
            yield return Timing.WaitForSeconds(textDelay);
            textBody.text += text.Substring(0, 1);
            text = text.Substring(1, text.Length - 1);
        }
    }
}
