using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.UI;

namespace Assets.Scripts.Controllers
{
    public class CardController : MonoBehaviour
    {
        public CardInfo CardInfo => _cardInfo;
        public CardMovement CardMovement => _cardMovement;
        public CardAbilities CardAbilities => _cardAbilities;
        public Card Card => _card;
        public bool IsPlayerCard => _isPlayerCard;

        private bool _isPlayerCard;

        [SerializeField] private AttackCard attackCard;
        [SerializeField] private Card _card;
        [SerializeField] private CardInfo _cardInfo;
        [SerializeField] private CardMovement _cardMovement;
        [SerializeField] private CardAbilities _cardAbilities;

        private GameManager _gameManager;

        public void Init(Card card, bool isPlayerCard)
        {
            _card = card;
            _gameManager = GameManager.instance;
            _isPlayerCard = isPlayerCard;

            if(isPlayerCard)
            {
                _cardInfo.ShowCardInfo();
                attackCard.enabled = false;
            }
            else
            {
                _cardInfo.HideCardInfo();
            }
        }

        public void OnCast()
        {           
            if (_card.IsSpell && ((SpellCard)_card).SpellTarget != TargetType.NO_TARGET)
                return;

            if(_isPlayerCard)
            {
                _gameManager.PlayerHandCards.Remove(this);
                _gameManager.PlayerFieldCards.Add(this);
                _gameManager.ReduceMana(true, _card.Manacost);
                _gameManager.CheckCardsForManaAvailability();
            }
            else
            {
                _gameManager.EnemyHandCards.Remove(this);
                _gameManager.EnemyFieldCards.Add(this);
                _gameManager.ReduceMana(false, _card.Manacost);
                _cardInfo.ShowCardInfo();
                _cardInfo.HideMana(false);
            }
            _card.IsPlaced = true;

            if (_card.HasAbility)
                _cardAbilities.OnCast();

            if (_card.IsSpell)
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

            switch (spellCard.Spell)
            {
                case SpellType.HEAL_ALLY_FIELD_CARDS:

                    var allyCards = _isPlayerCard ?
                                    _gameManager.PlayerFieldCards :
                                    _gameManager.EnemyFieldCards;

                    foreach (var card in allyCards)
                    {
                        AudioManager.Instance.HealAudio();
                        card._card.Defence += spellCard.SpellValue;
                        card._cardInfo.RefreshData();
                    }

                    _gameManager.InstantiateEffectHeal();

                    break;

                case SpellType.DAMAGE_ENEMY_FIELD_CARDS:

                    var enemyCard = _isPlayerCard ?
                                    new List<CardController>(_gameManager.EnemyFieldCards) :
                                    new List<CardController>(_gameManager.PlayerFieldCards);

                    foreach (var card in enemyCard)
                    {
                        AudioManager.Instance.VoiceAttack();
                        _gameManager.InstantiateEffectDamage();
                        GiveDamageTo(card, spellCard.SpellValue);
                    }

                    break;

                case SpellType.HEAL_ALLY_HERO:

                    if (_isPlayerCard)
                    {
                        _gameManager.CurrentGame.Player.AddHPValue(spellCard.SpellValue);
                    }  
                    else
                    {
                        _gameManager.CurrentGame.Enemy.AddHPValue(spellCard.SpellValue);
                    }

                    AudioManager.Instance.HealAudio();
                    _gameManager.InstantiateEffectHeal();
                    UIController.instance.UpdateHPAndMana();

                    break;

                case SpellType.DAMAGE_ENEMY_HERO:

                    if (_isPlayerCard)
                    {
                        _gameManager.CurrentGame.Enemy.AddHPValue(-spellCard.SpellValue);
                    }                      
                    else
                    {
                        _gameManager.CurrentGame.Player.AddHPValue(-spellCard.SpellValue);
                    }

                    AudioManager.Instance.VoiceAttack();
                    UIController.instance.UpdateHPAndMana();
                    _gameManager.InstantiateEffectDamage();
                    _gameManager.CheckForResult();

                    break;

                case SpellType.HEAL_ALLY_CARD:

                    {
                        AudioManager.Instance.HealAudio();
                        target._card.Defence += spellCard.SpellValue;
                        _gameManager.InstantiateEffectHeal();
                    }

                    break;

                case SpellType.DAMAGE_ENEMY_CARD:

                    {
                        AudioManager.Instance.VoiceAttack();
                        GiveDamageTo(target, spellCard.SpellValue);
                        _gameManager.InstantiateEffectDamage();
                    }

                    break;

                case SpellType.SHIELD_ON_ALLY_CARD:

                    if (!target._card.Abilities.Exists(x => x == AbilityType.SHIELD))
                    {
                        AudioManager.Instance.BuffAudio();
                        target._card.Abilities.Add(AbilityType.SHIELD);
                    }
                        
                    break;

                case SpellType.PROVOCATION_ON_ALLY_CARD:

                    if (!target._card.Abilities.Exists(x => x == AbilityType.PROVOCATION))
                    {
                        AudioManager.Instance.BuffAudio();
                        target._card.Abilities.Add(AbilityType.PROVOCATION);
                    }
                       
                    break;

                case SpellType.BUFF_CARD_DAMAGE:

                    {
                        AudioManager.Instance.BuffAudio();
                        target._card.Attack += spellCard.SpellValue;
                    }
                    
                    break;

                case SpellType.DEBUFF_CARD_DAMAGE:

                    {
                        AudioManager.Instance.BuffAudio();
                        target._card.Attack = Mathf.Clamp(target._card.Attack - spellCard.SpellValue, 0, int.MaxValue);
                    }
                 
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
            if (_card.isALive)
                _cardInfo.RefreshData();
            else               
            {
                DestroyCard();
                _gameManager.InstantiateEffectDeath();
            }
        }

        public void DestroyCard()
        {           
            _cardMovement.OnEndDrag(null);

            RemoveCardFromList(_gameManager.EnemyFieldCards);
            RemoveCardFromList(_gameManager.EnemyHandCards);
            RemoveCardFromList(_gameManager.PlayerFieldCards);
            RemoveCardFromList(_gameManager.PlayerHandCards);

            gameObject.SetActive(false);
        }

        private void RemoveCardFromList(List<CardController> list)
        {
            if (list.Exists(x => x == this))
                list.Remove(this);
        }
    }
}
