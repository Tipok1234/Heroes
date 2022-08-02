using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;
using Assets.Scripts.Controllers;
using Assets.Scripts.UI;
using Assets.Scripts.Models;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public Transform EnemyField => _enemyField;
        public AttackHero PlayerHero => _playerHero;
        public Game CurrentGame => _currentGame;
        public GameObject EffectHeal => _effectHeal;
        public GameObject EffectDeath => _effectDeath;

        public List<CardController> PlayerHandCards => _playerHandCards;
        public List<CardController> PlayerFieldCards => _playerFieldCards;
        public List<CardController> EnemyHandCards => _enemyHandCards;
        public List<CardController> EnemyFieldCards => _enemyFieldCards;

        public bool IsPlayerTurn { get { return _turn % 2 == 0; } }
        public static GameManager instance;

        [SerializeField] private Game _currentGame;
        [SerializeField] private Transform _enemyHand;
        [SerializeField] private Transform _enemyField;
        [SerializeField] private Transform _playerHand;
        [SerializeField] private Transform _playerField;

        [SerializeField] private GameObject _cardPref;
        [SerializeField] private GameObject _effectDamage;
        [SerializeField] private GameObject _effectHeal;
        [SerializeField] private GameObject _effectDeath;

        [SerializeField] private AttackHero _enemyHero;
        [SerializeField] private AttackHero _playerHero;
        [SerializeField] private AIManager _enemyAI;

        private int _turn;
        private int _turnTime = 30;
        private int _countCard = 4;
        private Vector3 effectPos = new Vector3(-6, 0, 0);        

        private List<CardController> _playerHandCards = new List<CardController>(),
                                    _playerFieldCards = new List<CardController>(),
                                    _enemyHandCards = new List<CardController>(),
                                    _enemyFieldCards = new List<CardController>();

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void RestartGame()
        {
            StopAllCoroutines();

            foreach (var card in _playerHandCards)
                Destroy(card.gameObject);
            foreach (var card in _playerFieldCards)
                Destroy(card.gameObject);
            foreach (var card in _enemyHandCards)
                Destroy(card.gameObject);
            foreach (var card in _enemyFieldCards)
                Destroy(card.gameObject);

            _playerHandCards.Clear();
            _playerFieldCards.Clear();
            _enemyHandCards.Clear();
            _enemyFieldCards.Clear();

            StartGame();
        }

        public void StartGame()
        {
            _turn = 0;

            _currentGame.InitGame();
            StartCoroutine(GiveHandCards(_currentGame.EnemyDeck, _enemyHand));
            StartCoroutine(GiveHandCards(_currentGame.PlayerDeck, _playerHand));           

            UIController.instance.StartGame();

            StartCoroutine(TurnFunc());
        }

        private IEnumerator GiveHandCards(List<Card> deck, Transform hand)
        { 
            int i = 0;
            while (i++ < _countCard)
            {
                GiveCardToHand(deck, hand);
                yield return new WaitForSeconds(0.25f);
                CheckCardsForManaAvailability();
            }
        }

        private void GiveCardToHand(List<Card> deck, Transform hand)
        {
            if (deck.Count == 0)
                return;

            CreateCardPref(deck[0], hand);
            AudioManager.Instance.DistributionCard();

            deck.RemoveAt(0);

            UIController.instance.UpdateEnemyDeckCard(_currentGame.EnemyDeck.Count);
            UIController.instance.UpdatePlayerDeckCard(_currentGame.PlayerDeck.Count);
        }

        private void CreateCardPref(Card card, Transform hand)
        {
            Vector3 posDragCard = new Vector3(5, 0, 0);
            GameObject cardGO = Instantiate(_cardPref,posDragCard,Quaternion.identity, hand);
            CardController cardC = cardGO.GetComponent<CardController>();

            cardC.Init(card, hand == _playerHand);           

            if (cardC.IsPlayerCard)
            {
                _playerHandCards.Add(cardC);
                 cardC.CardMovement.MoveToHand(_playerHand);
            }
            else
            {
                _enemyHandCards.Add(cardC);
                cardC.CardMovement.MoveToHand(_enemyHand);
            }              
        }

        private IEnumerator TurnFunc()
        {
            _turnTime = 30;
            UIController.instance.UpdateTurnTime(_turnTime);

            foreach (var card in _playerFieldCards)
            {
                card.CardInfo.HightLightCard(false);
            }          
                
            CheckCardsForManaAvailability();

            if (IsPlayerTurn)
            {
                foreach (var card in _playerFieldCards)
                {
                    card.Card.CanAttack = true;
                    card.CardInfo.HightLightCard(true);
                    card.CardAbilities.OnNewTurn();
                }        

                while(_turnTime-- > 0)
                {
                    UIController.instance.UpdateTurnTime(_turnTime);
                    yield return new WaitForSeconds(1);
                }
                ChangeTurn();
            }
            else
            {   
                foreach (var card in _enemyFieldCards)
                {
                    card.Card.CanAttack = true;
                    card.CardAbilities.OnNewTurn();                                      
                }

                _enemyAI.MakeTurn();               

                while(_turnTime-- > 0)
                {
                    UIController.instance.UpdateTurnTime(_turnTime);
                    yield return new WaitForSeconds(1);
                }
                ChangeTurn();
            }
        }

        public void ChangeTurn()
        {
            StopAllCoroutines();
            _turn++;

            UIController.instance.DisableTurnButton();

            if (IsPlayerTurn)
            {
                GiveNewCards();
                _currentGame.Player.InCreaseManapool();
                _currentGame.Player.RestoreRoundMana();

                UIController.instance.UpdateHPAndMana();
            }
            else
            {
                _currentGame.Enemy.InCreaseManapool();
                _currentGame.Enemy.RestoreRoundMana();
            }
            StartCoroutine(TurnFunc());            
        }

        private void GiveNewCards()
        {
            GiveCardToHand(_currentGame.EnemyDeck, _enemyHand);
            GiveCardToHand(_currentGame.PlayerDeck, _playerHand);
        }

        public void CardsFight(CardController attacker, CardController defender)
        {
            InstantiateEffectDamage();         

            defender.Card.GetDamage(attacker.Card.Attack);
            attacker.OnDamageDeal();
            AudioManager.Instance.VoiceAttack();
            defender.OnTakeDamage(attacker);

            attacker.Card.GetDamage(defender.Card.Attack);
            attacker.OnTakeDamage();

            attacker.CheckForALive();
            defender.CheckForALive();
        }

        public void ReduceMana(bool playerMana, int manaCost)
        {
            if (playerMana)
                _currentGame.Player.SetMana(manaCost);
            else
                _currentGame.Enemy.SetMana(manaCost);

            UIController.instance.UpdateHPAndMana();
        }

        public void DamageHero(CardController card, bool isEnemyAttack)
        {
            if (isEnemyAttack)
                _currentGame.Enemy.GetDamage(card.Card.Attack);
            else
                _currentGame.Player.GetDamage(card.Card.Attack);

            UIController.instance.UpdateHPAndMana();
            card.OnDamageDeal();
            CheckForResult();
        }

        public void CheckForResult()
        {
            if(_currentGame.Enemy.HP == 0 || _currentGame.Player.HP == 0)
            {
                StopAllCoroutines();
                UIController.instance.ShowResult();
            }
        }

        public void CheckCardsForManaAvailability()
        {
            foreach (var card in _playerHandCards)
                card.CardInfo.HightLightManaAvaliability(_currentGame.Player.Mana);
        }

        public void HightLightTargets(CardController attacker,bool hightLight)
        {
            List<CardController> targets = new List<CardController>();

            if(attacker.Card.IsSpell)
            {
                var spellCard = (SpellCard)attacker.Card;

                if (spellCard.SpellTarget == TargetType.NO_TARGET)
                    targets = new List<CardController>();
                else if (spellCard.SpellTarget == TargetType.ALLY_CARD_TARGET)
                    targets = _playerFieldCards;
                else
                    targets = _enemyFieldCards;
            }
            else
            {
                if (_enemyFieldCards.Exists(x => x.Card.IsProvocation))
                    targets = _enemyFieldCards.FindAll(x => x.Card.IsProvocation);
                else
                {
                    targets = _enemyFieldCards;
                }
            }              
            foreach (var card in targets)
            {
                if (attacker.Card.IsSpell)
                    card.CardInfo.HightLightAsSpellTarget(hightLight);
                else
                    card.CardInfo.HightLightTarget(hightLight);
            }    
        }

        public void InstantiateEffectHeal()
        {
            Instantiate(instance.EffectHeal, effectPos, Quaternion.identity);
        }

        public void InstantiateEffectDeath()
        {
            Instantiate(instance.EffectDeath, effectPos, Quaternion.identity);
        }

        public void InstantiateEffectDamage()
        {
            Instantiate(instance._effectDamage, effectPos, Quaternion.identity);
        }
    }
}
