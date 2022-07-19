using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models;
using Assets.Scripts.Managers;
using Assets.Scripts.Enums;

public class DropPlaceScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameManager _gameManager;


    public FieldType Type;
    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_FIELD)
            return;

        CardScripts card = eventData.pointerDrag.GetComponent<CardScripts>();
        BattleCard battleCard = card.GetComponent<BattleCard>();

        if (card && _gameManager.PlayerFieldCardsCount < 6 && _gameManager.IsPlayerTurn 
            && _gameManager.PlayerMana >= battleCard.ManaCostPoints)
        {
            _gameManager.RemoveHandCard(FieldType.SELF_HAND, battleCard);
            _gameManager.AddCardInField(FieldType.SELF_FIELD, battleCard);
            
            card._defaultParent = transform;

            _gameManager.ReduceMana(true, battleCard.ManaCostPoints);

        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD ||
            Type == FieldType.ENEMY_HAND || Type == FieldType.SELF_HAND)
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
