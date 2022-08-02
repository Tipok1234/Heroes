using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.Scripts.Models
{ 
public class SpellTarget : MonoBehaviour, IDropHandler
{
        public void OnDrop(PointerEventData eventData)
        {
            if (!GameManager.instance.IsPlayerTurn)
                return;

            CardController spell = eventData.pointerDrag.GetComponent<CardController>(),
                           target = GetComponent<CardController>();

            if (spell && spell.Card.IsSpell && spell.IsPlayerCard &&
                target.Card.IsPlaced &&
                GameManager.instance.CurrentGame.Player.Mana >= spell.Card.Manacost)
            {
                var spellCard = (SpellCard)spell.Card;

                if ((spellCard.SpellTarget == TargetType.ALLY_CARD_TARGET &&
                    target.IsPlayerCard) ||
                    (spellCard.SpellTarget == TargetType.ENEMY_CARD_TARGET &&
                    !target.IsPlayerCard))
                {
                    GameManager.instance.ReduceMana(true, spell.Card.Manacost);
                    spell.UseSpell(target);
                    GameManager.instance.CheckCardsForManaAvailability();
                }
            }
        }
    }
}
