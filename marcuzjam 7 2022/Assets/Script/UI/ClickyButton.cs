using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip;
    private void Start()
    {
        _img.sprite = _default;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        SoundManager.Instance.PlaySound(_compressClip);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = _default;



    }
}
