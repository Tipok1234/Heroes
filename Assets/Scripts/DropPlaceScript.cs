using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlaceScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardScripts card = eventData.pointerDrag.GetComponent<CardScripts>();

        if (card)
            card._defaultParent = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardScripts card = eventData.pointerDrag.GetComponent<CardScripts>();

        if (card)
            card._defaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardScripts card = eventData.pointerDrag.GetComponent<CardScripts>();

        if (card && card._defaultTempCardParent == transform)
            card._defaultTempCardParent = card._defaultParent;
    }
}
