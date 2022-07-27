using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Controllers
{
    public class CardController : MonoBehaviour
    {
        public Card _card;
        public bool _isPlayerCard;

        public CardInfo _cardInfo;
        public CardMovement _cardMovement;
        public CardAbilities _cardAbilities;
       // [SerializeField] private GameObject _effectHeal;

        private GameManager _gameManager;

        public void Init(Card card, bool isPlayerCard)
        {
            _card = card;
            _gameManager = GameManager.instance;
            _isPlayerCard = isPlayerCard;

            if(isPlayerCard)
            {
                _cardInfo.ShowCardInfo();
                GetComponent<AttackCard>().enabled = false;
            }
            else
            {
                _cardInfo.HideCardInfo();
            }
        }

        public void OnCast()
        {
            
            if (_card.isSpell && ((SpellCard)_card)._spellTarget != TargetType.NO_TARGET)
                return;

            if(_isPlayerCard)
            {
                _gameManager._playerHandCards.Remove(this);
                _gameManager._playerFieldCards.Add(this);
                _gameManager.ReduceMana(true, _card.Manacost);
                _gameManager.CheckCardsForManaAvailability();
            }
            else
            {
                _gameManager._enemyHandCards.Remove(this);
                _gameManager._enemyFieldCards.Add(this);
                _gameManager.ReduceMana(false, _card.Manacost);
                _cardInfo.ShowCardInfo();
                _cardInfo.HideMana(false);
            }
            _card.IsPlaced = true;

            if (_card.HasAbility)
                _cardAbilities.OnCast();

            if (_card.isSpell)
                UseSpell(null);

            UIController.instance.UpdateHPAndMana();
        }

        public void OnTakeDamage(CardController attacker = null)
        {
            CheckForALive();
            _cardAbilities.OnDamageTake(attacker);
        }

        public void OnDamageDeal()
        {
            _card.timeDealDamage++;
            _card.CanAttack = false;
            _cardInfo.HightLightCard(false);

            if (_card.HasAbility)
                _cardAbilities.OnDamageDeal();
        }

        public void UseSpell(CardController target)
        {
            var spellCard = (SpellCard)_card;
            var effectHealPos = new Vector3(-6, 0, 0);

            switch(spellCard._spell)
            {
                case SpellType.HEAL_ALLY_FIELD_CARDS:

                    var allyCards = _isPlayerCard ?
                                    _gameManager._playerFieldCards :
                                    _gameManager._enemyFieldCards;

                    foreach (var card in allyCards)
                    {
                        card._card.Defence += spellCard._spellValue;
                        card._cardInfo.RefreshData();
                    }

                    Instantiate(_gameManager._effectHeal, effectHealPos, Quaternion.identity);

                    break;

                case SpellType.DAMAGE_ENEMY_FIELD_CARDS:

                    var enemyCard = _isPlayerCard ?
                                    new List<CardController>(_gameManager._enemyFieldCards) :
                                    new List<CardController>(_gameManager._playerFieldCards);

                    foreach (var card in enemyCard)
                        GiveDamageTo(card, spellCard._spellValue);

                    break;

                case SpellType.HEAL_ALLY_HERO:

                    if (_isPlayerCard)
                    {
                        _gameManager._currentGame._player._hp += spellCard._spellValue;
                    }  
                    else
                    {
                        _gameManager._currentGame._enemy._hp += spellCard._spellValue;
                    }

                    Instantiate(_gameManager._effectHeal, effectHealPos, Quaternion.identity);
                    UIController.instance.UpdateHPAndMana();

                    break;

                case SpellType.DAMAGE_ENEMY_HERO:

                    if (_isPlayerCard)
                        _gameManager._currentGame._enemy._hp -= spellCard._spellValue;
                    else
                        _gameManager._currentGame._player._hp -= spellCard._spellValue;

                    UIController.instance.UpdateHPAndMana();
                    _gameManager.CheckForResult();

                    break;

                case SpellType.HEAL_ALLY_CARD:
                   
                        target._card.Defence += spellCard._spellValue;
                    Instantiate(_gameManager._effectHeal, effectHealPos, Quaternion.identity);

                    break;

                case SpellType.DAMAGE_ENEMY_CARD:
                    GiveDamageTo(target, spellCard._spellValue);
                    break;

                case SpellType.SHIELD_ON_ALLY_CARD:

                    if (!target._card._abilities.Exists(x => x == AbilityType.SHIELD))
                        target._card._abilities.Add(AbilityType.SHIELD);

                    break;

                case SpellType.PROVOCATION_ON_ALLY_CARD:

                    if (!target._card._abilities.Exists(x => x == AbilityType.PROVOCATION))
                        target._card._abilities.Add(AbilityType.PROVOCATION);

                    break;

                case SpellType.BUFF_CARD_DAMAGE:
                    target._card.Attack += spellCard._spellValue;
                    break;

                case SpellType.DEBUFF_CARD_DAMAGE:
                    target._card.Attack = Mathf.Clamp(target._card.Attack - spellCard._spellValue,0,int.MaxValue);
                    break;
            }

            if(target != null)
            {
                target._cardAbilities.OnCast();
                target.CheckForALive();
            }

            DestroyCard();
        }

        private void GiveDamageTo(CardController card, int damage)
        {
            card._card.GetDamage(damage);
            card.CheckForALive();
            card.OnTakeDamage();
        }

        public void CheckForALive()
        {
            var effectDeathPos = new Vector3(-6, 0, 0);

            if (_card.isALive)
                _cardInfo.RefreshData();
            else               
            {
                DestroyCard();
                Instantiate(_gameManager._effectDeath, effectDeathPos, Quaternion.identity);
            }
        }

        public void DestroyCard()
        {
            
            _cardMovement.OnEndDrag(null);

            RemoveCardFromList(_gameManager._enemyFieldCards);
            RemoveCardFromList(_gameManager._enemyHandCards);
            RemoveCardFromList(_gameManager._playerFieldCards);
            RemoveCardFromList(_gameManager._playerHandCards);

            gameObject.SetActive(false);
        }

        private void RemoveCardFromList(List<CardController> list)
        {
            if (list.Exists(x => x == this))
                list.Remove(this);
        }
    }
}
