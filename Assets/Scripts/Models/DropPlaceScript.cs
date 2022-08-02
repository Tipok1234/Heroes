using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Managers;
using Assets.Scripts.Controllers;
using Assets.Scripts.Enums;


namespace Assets.Scripts.Models
{
    public class DropPlaceScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public FieldType Type => type;

        [SerializeField] private FieldType type;
        public void OnDrop(PointerEventData eventData)
        {
            if (type != FieldType.SELF_FIELD)
                return;

            CardController card = eventData.pointerDrag.GetComponent<CardController>();

            if (card && GameManager.instance.IsPlayerTurn  &&
                GameManager.instance.CurrentGame.Player.Mana >= card.Card.Manacost 
               && GameManager.instance.PlayerFieldCards.Count <= 4 && !card.Card.IsPlaced)
            {
                if (!card.Card.IsSpell)
                    card.CardMovement._defaultParent = transform;

                card.CardInfo.HideMana(false);
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
}
