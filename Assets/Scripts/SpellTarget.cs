using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
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

        if (spell && spell._card.IsSpell && spell._isPlayerCard &&
            target._card.IsPlaced &&
            GameManager.instance._currentGame._player._mana >= spell._card.Manacost)
        {
            var spellCard = (SpellCard)spell._card;
            
            if ((spellCard._spellTarget == SpellCard.TargetType.ALLY_CARD_TARGET &&
                target._isPlayerCard) ||
                (spellCard._spellTarget == SpellCard.TargetType.ENEMY_CARD_TARGET &&
                !target._isPlayerCard))
            {
                GameManager.instance.ReduceMana(true, spell._card.Manacost);
                spell.UseSpell(target);
                GameManager.instance.CheckCardsForManaAvailability();
            }
        }
    }
}