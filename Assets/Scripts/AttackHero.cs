using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Enums;
public class AttackHero : MonoBehaviour, IDropHandler
{
    private HeroType _type;
    [SerializeField] private Color _normalHeroColor;
    [SerializeField] private Color _targetHeroColor;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.instance.IsPlayerTurn)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if(card && card._card.CanAttack && _type == HeroType.ENEMY &&
            !GameManager.instance._enemyFieldCards.Exists(x => x._card.IsProvocation))
        {
            GameManager.instance.DamageHero(card, true);
            AudioManager._instanceAudio.VoiceAttack();         
        }
    }

    public void HightLightTarget(bool hightLight)
    {
        GetComponent<Image>().color = hightLight ? _targetHeroColor : _normalHeroColor;
    }
}
