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

    [SerializeField] private HeroType _type;
    [SerializeField] private Image _cardHeroImage;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Color _NormalColorHero;
    [SerializeField] private Color _TargetColorHero;

    public void OnDrop(PointerEventData eventData)
    {
        if (!_gameManager.IsPlayerTurn)
            return;

        BattleCard battleCard = eventData.pointerDrag.GetComponent<BattleCard>();

        if(battleCard && battleCard.IsCanAttack && _type == HeroType.ENEMY &&
            !_gameManager.EnemyFieldCards.Exists(x => x.IsProvocation))
        {
            battleCard.ChangeAttackState(false);
            _gameManager.DamageHero(battleCard,true);
        }
    }

    public void HighLightAsTarget(bool hightlight)
    {
        _cardHeroImage.color = hightlight ? _TargetColorHero : _NormalColorHero;
    }
}
