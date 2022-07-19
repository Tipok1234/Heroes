using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackHero : MonoBehaviour, IDropHandler
{
    public enum HeroType
    {
        ENEMY,
        PLAYER
    }

    [SerializeField] private HeroType _type;
    [SerializeField] private GameManager _gameManager;

    public void OnDrop(PointerEventData eventData)
    {
        if (!_gameManager.IsPlayerTurn)
            return;

        BattleCard battleCard = eventData.pointerDrag.GetComponent<BattleCard>();

        if(battleCard && battleCard.IsCanAttack && _type == HeroType.ENEMY)
        {
            battleCard.ChangeAttackState(false);
            _gameManager.DamageHero(battleCard,true);
        }
    }
}
