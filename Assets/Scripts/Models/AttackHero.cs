using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Enums;


namespace Assets.Scripts.Models
{
    public class AttackHero : MonoBehaviour, IDropHandler
    {
        public static AttackHero instanceHero;

        [SerializeField] private HeroType _type;
        [SerializeField] private Color _normalHeroColor;
        [SerializeField] private Color _targetHeroColor;
        [SerializeField] private Image _imageTarget;

        public void OnDrop(PointerEventData eventData)
        {
            if (!GameManager.instance.IsPlayerTurn)
                return;

            CardController card = eventData.pointerDrag.GetComponent<CardController>();

            if (GameManager.instance.EnemyFieldCards.Count <= 3 &&
                card && card.Card.CanAttack && _type == HeroType.ENEMY && 
                !GameManager.instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation))
            {
                GameManager.instance.DamageHero(card, true);
                AudioManager.Instance.VoiceAttack();
            }
        }

        public void HightLightTargetHero(bool hightLight)
        {
            _imageTarget.color = hightLight ? _targetHeroColor : _normalHeroColor;
        }
    }
}
