using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackHero : MonoBehaviour, IDropHandler
{
    public enum HeroType
    {
        ENEMY,
        PLAYER
    }

    public HeroType _type;
    public Color _normalHeroColor;
    public Color _targetHeroColor;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.instance.IsPlayerTurn)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if(card && card._card.CanAttack && _type == HeroType.ENEMY &&
            !GameManager.instance._enemyFieldCards.Exists(x => x._card.IsProvocation))
        {
            GameManager.instance.DamageHero(card, true);
        }
    }

    public void HightLightTarget(bool hightLight)
    {
        GetComponent<Image>().color = hightLight ? _targetHeroColor : _normalHeroColor;
    }
}
