using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Enums;
using System;

public class AttackCard : MonoBehaviour, IDropHandler
{
    [SerializeField] private BattleCard _battleCard;
    public static event Action<BattleCard, BattleCard> CardFigthAction;

    public void OnDrop(PointerEventData eventData)
    {
        BattleCard card = eventData.pointerDrag.GetComponent<BattleCard>(),
                   defender = GetComponent<BattleCard>();

        if (card && card.IsCanAttack && GetComponent<BattleCard>().FieldType == FieldType.ENEMY_FIELD)
        {
            if (_battleCard.GameManager.EnemyFieldCards.Exists(x => x.IsProvocation) &&
                !defender.IsProvocation)
                return;

            card.ChangeAttackState(false);
            CardFigthAction?.Invoke(card, GetComponent<BattleCard>());
        }
    }
}
