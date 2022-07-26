using Assets.Scripts.Models;
using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Models
{
    public class CardAbilities : MonoBehaviour
    {
        public CardController _cardController;
        public GameObject _shield;
        public GameObject _provocation;

        public void OnCast()
        {
            foreach (var ability in _cardController._card._abilities)
            {
                switch (ability)
                {
                    case Card.AbilityType.INSTANT_ACTIVE:
                        _cardController._card.CanAttack = true;

                        if (_cardController._isPlayerCard)
                            _cardController._cardInfo.HightLightCard(true);
                        break;

                    case Card.AbilityType.SHIELD:
                        _shield.SetActive(true);
                        break;

                    case Card.AbilityType.PROVOCATION:
                        _provocation.SetActive(true);
                        break;
                }
            }
        }

        public void OnDamageDeal()
        {
            foreach (var ability in _cardController._card._abilities)
            {
                switch (ability)
                {
                    case Card.AbilityType.DOUBLE_ATTACK:

                        if (_cardController._card.timeDealDamage == 1)
                        {
                            _cardController._card.CanAttack = true;
                            if (_cardController._isPlayerCard)
                                _cardController._cardInfo.HightLightCard(true);
                        }

                        break;
                }
            }
        }

        public void OnDamageTake(CardController attacker = null)
        {
            _shield.SetActive(false);

            foreach (var ability in _cardController._card._abilities)
            {
                switch (ability)
                {
                    case Card.AbilityType.SHIELD:
                        _shield.SetActive(true);
                        break;

                    case Card.AbilityType.COUNTER_ATTACK:
                        if (attacker != null)
                            attacker._card.GetDamage(_cardController._card.Attack);
                        break;

                }
            }
        }

        public void OnNewTurn()
        {
            _cardController._card.timeDealDamage = 0;

            foreach (var ability in _cardController._card._abilities)
            {
                switch (ability)
                {
                    case Card.AbilityType.REGENIRATION_EACH_TURN:
                        _cardController._card.Defence += 2;
                        _cardController._cardInfo.RefreshData();
                        break;

                }
            }
        }
    }
}
