using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}
public class DropPlaceScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType Type;
    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_FIELD)
            return;

        CardScripts card = eventData.pointerDrag.GetComponent<CardScripts>();

        if (card)
            card._defaultParent = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD ||
            Type == FieldType.ENEMY_HAND)
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
