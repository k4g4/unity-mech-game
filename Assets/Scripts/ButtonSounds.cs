using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour
    , IPointerClickHandler
    , IPointerEnterHandler
{
    AudioSource audSoc;
    public AudioClip hover, clickSFX;

    void Awake()
    {
        audSoc = GameObject.Find("SoundController").GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audSoc.PlayOneShot(hover,0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audSoc.PlayOneShot(clickSFX,0.5f);
    }
}
