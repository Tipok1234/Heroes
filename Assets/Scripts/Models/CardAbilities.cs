using Assets.Scripts.Enums;
using UnityEngine;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Models
{
    public class CardAbilities : MonoBehaviour
    {
        [SerializeField] private CardController _cardController;
        [SerializeField] private  GameObject _shield;
        [SerializeField] private GameObject _provocation;

        public void OnCast()
        {
            foreach (var ability in _cardController.Card.Abilities)
            {
                switch (ability)
                {
                    case AbilityType.INSTANT_ACTIVE:
                        _cardController.Card.CanAttack = true;

                        if (_cardController.IsPlayerCard)
                            _cardController.CardInfo.HightLightCard(true);
                        break;

                    case AbilityType.SHIELD:
                        _shield.SetActive(true);
                        break;

                    case AbilityType.PROVOCATION:
                        _provocation.SetActive(true);
                        break;
                }
            }
        }

        public void OnDamageDeal()
        {
            foreach (var ability in _cardController.Card.Abilities)
            {
                switch (ability)
                {
                    case AbilityType.DOUBLE_ATTACK:

                        if (_cardController.Card.timeDealDamage == 1)
                        {
                            _cardController.Card.CanAttack = true;
                            if (_cardController.IsPlayerCard)
                                _cardController.CardInfo.HightLightCard(true);
                        }

                        break;
                }
            }
        }

        public void OnDamageTake(CardController attacker = null)
        {
            _shield.SetActive(false);

            foreach (var ability in _cardController.Card.Abilities)
            {
                switch (ability)
                {
                    case AbilityType.SHIELD:
                        _shield.SetActive(true);
                        break;

                    case AbilityType.COUNTER_ATTACK:
                        if (attacker != null)
                            attacker.Card.GetDamage(_cardController.Card.Attack);
                        break;
                }
            }
        }

        public void OnNewTurn()
        {
            _cardController.Card.timeDealDamage = 0;

            foreach (var ability in _cardController.Card.Abilities)
            {
                switch (ability)
                {
                    case AbilityType.REGENIRATION_EACH_TURN:
                        _cardController.Card.Defence += 2;
                        _cardController.CardInfo.RefreshData();
                        break;

                }
            }
        }
    }
}
