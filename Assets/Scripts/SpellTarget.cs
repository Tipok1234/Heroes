using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.EventSystems;


public class SpellTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.instance.IsPlayerTurn)
            return;

        CardController spell = eventData.pointerDrag.GetComponent<CardController>(),
                       target = GetComponent<CardController>();

        if (spell && spell._card.isSpell && spell.IsPlayerCard &&
            target._card.IsPlaced &&
            GameManager.instance._currentGame._player._mana >= spell._card.Manacost)
        {
            var spellCard = (SpellCard)spell._card;
            
            if ((spellCard.SpellTarget == TargetType.ALLY_CARD_TARGET &&
                target.IsPlayerCard) ||
                (spellCard.SpellTarget == TargetType.ENEMY_CARD_TARGET &&
                !target.IsPlayerCard))
            {
                GameManager.instance.ReduceMana(true, spell._card.Manacost);
                spell.UseSpell(target);
                GameManager.instance.CheckCardsForManaAvailability();
            }
        }
    }
}
