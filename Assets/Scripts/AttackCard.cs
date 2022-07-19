using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Enums;
using System;

public class AttackCard : MonoBehaviour, IDropHandler
{
    public static event Action<BattleCard, BattleCard> CardFigthAction;

    public void OnDrop(PointerEventData eventData)
    {
        BattleCard card = eventData.pointerDrag.GetComponent<BattleCard>();

        if (card && card.IsCanAttack && GetComponent<BattleCard>().FieldType == FieldType.ENEMY_FIELD)
        {         
            card.ChangeAttackState(false);
            CardFigthAction?.Invoke(card, GetComponent<BattleCard>());
        }
    }
}
