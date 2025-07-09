using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public RectTransform text;
    public float nilai_offset;



    //public void OnMouseDown()
    //{
    //    text.position = new Vector2(text.position.x, text.position.y + nilai_offset);
    //    OnMouseUp();
    //}

    //public void OnMouseUp()
    //{
    //    text.position = new Vector2(text.position.x, text.position.y - nilai_offset);
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        text.position = new Vector2(text.position.x, text.position.y + nilai_offset);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.position = new Vector2(text.position.x, text.position.y - nilai_offset);
    }
}
