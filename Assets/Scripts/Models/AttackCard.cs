using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Managers;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Models
{
    public class AttackCard : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (!GameManager.instance.IsPlayerTurn)
                return;

            CardController attacker = eventData.pointerDrag.GetComponent<CardController>(),
                           defender = GetComponent<CardController>();

            if (attacker && attacker.Card.CanAttack &&
                defender.Card.IsPlaced)
            {
                if (GameManager.instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation) &&
                    !defender.Card.IsProvocation)
                    return;

                GameManager.instance.CardsFight(attacker, defender);
            }
        }
    }
}
