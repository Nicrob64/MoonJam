using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextColourChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text theText;
    public Color hoverColour;
    public Color normalColour;

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = hoverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = normalColour;
    }
}