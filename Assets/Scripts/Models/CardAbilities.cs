using Assets.Scripts.Models;
using UnityEngine;
using Assets.Scripts.Enums;
using Assets.Scripts.SO;

namespace Assets.Scripts.Models
{
    public class CardAbilities : MonoBehaviour
    {
        [SerializeField] private BattleCard _battleCard;  
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _provocation;

        public void OnCast()
        {
            foreach (var ability in _battleCard.ListAbilities)
            {
                switch(ability)
                {
                    case CardAbility.INSTANT_ACTIVE:
                        _battleCard.ChangeAttackState(true);

                        if (_battleCard.IsPlayer)
                            _battleCard.EnableCardLight(true);
                        break;

                    case CardAbility.SHIELD:
                        _shield.SetActive(true);
                        break;

                    case CardAbility.PROVOCATION:
                        _provocation.SetActive(true);
                        break;
                }
            }
        }

        public void OnDamageDeal()
        {
            foreach (var ability in _battleCard.ListAbilities)
            {
                switch (ability)
                {
                    case CardAbility.DOUBLE_ATTACK:

                        if (_battleCard.TimesDealDamage == 1)
                        {
                            _battleCard.ChangeAttackState(true);
                            if (_battleCard.IsPlayer)
                                _battleCard.EnableCardLight(true);
                        }

                        break;
                }
            }
        }

        public void OnTookDamage(BattleCard card = null)
        {
            _shield.SetActive(false);

            foreach (var ability in _battleCard.ListAbilities)
            {
                switch (ability)
                {
                    case CardAbility.SHIELD:
                        _shield.SetActive(true);
                        break;

                    case CardAbility.COUNTER_ATTACK:
                        if (card != null)
                            card.GetDamage(_battleCard.AttackPoints);
                            break;

                }
            }
        }

        public void OnNewTurn()
        {
            _battleCard.TimesDealDamage = 0;

            foreach (var ability in _battleCard.ListAbilities)
            {
                switch (ability)
                {
                    case CardAbility.REGENIRATION_EACH_TURN:
                        _battleCard.AddDefencePoints(2);
                        _battleCard.RefreshData();
                        break;
                        
                }
            }
        }
    }
}
