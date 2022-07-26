using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.Controllers;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Managers
{
    public class Game
    {
        public Player _player;
        public Player _enemy;
        public List<Card> _enemyDeck, _playerDeck;

        public Game()
        {
            _enemyDeck = GiveDeckCard();
            _playerDeck = GiveDeckCard();
            _player = new Player();
            _enemy = new Player();
            UIController.instance.UpdateEnemyDeckCard(_enemyDeck.Count);
            UIController.instance.UpdatePlayerDeckCard(_playerDeck.Count);         
        }

        List<Card> GiveDeckCard()
        {
            List<Card> list = new List<Card>();
            list.Add(CardManager.allCards[6].GetCopy());

           // UIController.instance.UpdatePlayerDeckCard(list.Count);

            for (int i = 0; i < CardManager.allCards.Count; i++)
            {
                var card = CardManager.allCards[Random.Range(0, CardManager.allCards.Count)];
                
                
                if (card.IsSpell)
                {
                    list.Add(((SpellCard)card).GetCopy());
                }
                else
                {
                    list.Add(card.GetCopy());
                }
                
            }
            return list;
        }
    }
    public class GameManager : MonoBehaviour
    {
        public bool IsPlayerTurn { get { return _turn % 2 == 0; } }
        public static GameManager instance;
        public Game _currentGame;
        public Transform _enemyHand;
        public Transform _enemyField;
        public Transform _playerHand;
        public Transform _playerField;
        public GameObject _cardPref;

        private int _turn;
        private int _turnTime = 30;

        public AttackHero _enemyHero;
        public AttackHero _playerHero;
        public AI _enemyAI;

        public List<CardController> _playerHandCards = new List<CardController>(),
                                    _playerFieldCards = new List<CardController>(),
                                    _enemyHandCards = new List<CardController>(),
                                    _enemyFieldCards = new List<CardController>();




        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        private void Start()
        {
            StartGame();
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

        private void StartGame()
        {
            _turn = 0;

            _currentGame = new Game();
            GiveHandCards(_currentGame._enemyDeck, _enemyHand);
            GiveHandCards(_currentGame._playerDeck, _playerHand);

            UIController.instance.StartGame();

            StartCoroutine(TurnFunc());
        }

        private void GiveHandCards(List<Card> deck, Transform hand)
        {
            int i = 0;
            while (i++ < 4)
                GiveCardToHand(deck,hand);
        }

        private void GiveCardToHand(List<Card> deck, Transform hand)
        {
            if (deck.Count == 0)
                return;

            CreateCardPref(deck[0], hand);

            deck.RemoveAt(0);

            UIController.instance.UpdateEnemyDeckCard(_currentGame._enemyDeck.Count);
            UIController.instance.UpdatePlayerDeckCard(_currentGame._playerDeck.Count);
        }

        private void CreateCardPref(Card card, Transform hand)
        {
            GameObject cardGO = Instantiate(_cardPref, hand, false);
            CardController cardC = cardGO.GetComponent<CardController>();

            cardC.Init(card, hand == _playerHand);

            if (cardC._isPlayerCard)
                _playerHandCards.Add(cardC);
            else
                _enemyHandCards.Add(cardC);
        }

        private IEnumerator TurnFunc()
        {
            _turnTime = 30;
            UIController.instance.UpdateTurnTime(_turnTime);

            foreach (var card in _playerFieldCards)
            {
                card._cardInfo.HightLightCard(false);
            }          
                
            CheckCardsForManaAvailability();

            if (IsPlayerTurn)
            {
                foreach (var card in _playerFieldCards)
                {
                    card._card.CanAttack = true;
                    card._cardInfo.HightLightCard(true);
                    card._cardAbilities.OnNewTurn();
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
                    card._card.CanAttack = true;
                    card._cardAbilities.OnNewTurn();                                      
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
                _currentGame._player.InCreaseManapool();
                _currentGame._player.RestoreRoundMana();

                UIController.instance.UpdateHPAndMana();
            }
            else
            {
                _currentGame._enemy.InCreaseManapool();
                _currentGame._enemy.RestoreRoundMana();
            }
                

            StartCoroutine(TurnFunc());

        }

        private void GiveNewCards()
        {
            GiveCardToHand(_currentGame._enemyDeck, _enemyHand);
            GiveCardToHand(_currentGame._playerDeck, _playerHand);
        }

        public void CardsFight(CardController attacker, CardController defender)
        {
            defender._card.GetDamage(attacker._card.Attack);
            attacker.OnDamageDeal();
            defender.OnTakeDamage(attacker);

            attacker._card.GetDamage(defender._card.Attack);
            attacker.OnTakeDamage();

            attacker.CheckForALive();
            defender.CheckForALive();
        }

        public void ReduceMana(bool playerMana, int manaCost)
        {
            if (playerMana)
                _currentGame._player._mana = manaCost;
            else
                _currentGame._enemy._mana = manaCost;

            UIController.instance.UpdateHPAndMana();
        }

        public void DamageHero(CardController card, bool isEnemyAttack)
        {
            if (isEnemyAttack)
                _currentGame._enemy.GetDamage(card._card.Attack);
            else
                _currentGame._player.GetDamage(card._card.Attack);

            UIController.instance.UpdateHPAndMana();
            card.OnDamageDeal();
            CheckForResult();
        }

        public void CheckForResult()
        {
            if(_currentGame._enemy._hp == 0 || _currentGame._player._hp == 0)
            {
                StopAllCoroutines();
                UIController.instance.ShowResult();
                
            }
        }

        public void CheckCardsForManaAvailability()
        {
            foreach (var card in _playerHandCards)
                card._cardInfo.HightLightManaAvaliability(_currentGame._player._mana);
        }

        public void HightLightTargets(CardController attacker,bool hightLight)
        {
            List<CardController> targets = new List<CardController>();

            if(attacker._card.IsSpell)
            {
                var spellCard = (SpellCard)attacker._card;

                if (spellCard._spellTarget == SpellCard.TargetType.NO_TARGET)
                    targets = new List<CardController>();
                else if (spellCard._spellTarget == SpellCard.TargetType.ALLY_CARD_TARGET)
                    targets = _playerFieldCards;
                else
                    targets = _enemyFieldCards;
            }
            else
            {
                if (_enemyFieldCards.Exists(x => x._card.IsProvocation))
                    targets = _enemyFieldCards.FindAll(x => x._card.IsProvocation);
                else
                {
                    targets = _enemyFieldCards;

                   // if(!attacker._card.IsSpell)
                    _enemyHero.HightLightTarget(hightLight);
                }
            }              
            foreach (var card in targets)
            {
                if (attacker._card.IsSpell)
                    card._cardInfo.HightLightAsSpellTarget(hightLight);
                else
                    card._cardInfo.HightLightTarget(hightLight);
            }    
        }
    }
}
