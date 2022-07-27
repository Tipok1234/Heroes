using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.Controllers;
using Assets.Scripts.Enums;

public class DropPlaceScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType type;
    public void OnDrop(PointerEventData eventData)
    {
        if (type != FieldType.SELF_FIELD)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card && GameManager.instance.IsPlayerTurn &&
            GameManager.instance._currentGame._player._mana >= card._card.Manacost &&
            !card._card.IsPlaced)
        {
            if(!card._card.isSpell)
            card._cardMovement._defaultParent = transform;


            card._cardInfo.HideMana(false);
            card.OnCast();
        }       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || type == FieldType.ENEMY_FIELD || 
            type == FieldType.ENEMY_HAND || type == FieldType.SELF_HAND)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card)
            card._defaultTempCardParant = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        if (card && card._defaultTempCardParant == transform)
            card._defaultTempCardParant = card._defaultParent;
    }
}
