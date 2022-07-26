using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.Managers;
using Assets.Scripts.Controllers;

public class AttackCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.instance.IsPlayerTurn)
            return;

        CardController attacker = eventData.pointerDrag.GetComponent<CardController>(),
                       defender = GetComponent<CardController>();

        if(attacker && attacker._card.CanAttack &&
            defender._card.IsPlaced)
        {
            if (GameManager.instance._enemyFieldCards.Exists(x => x._card.IsProvocation) &&
                !defender._card.IsProvocation)
                return;


            GameManager.instance.CardsFight(attacker, defender);
        }
    }
}
