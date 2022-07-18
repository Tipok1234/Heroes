using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.SO;
using static Assets.Scripts.Enums.FieldTypeEnum;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public bool IsPlayerTurn
        {
            get { return turn % 2 == 0; }
        }

        public int PlayerFieldCardsCount => _playerFieldCards.Count;

        [SerializeField] private CardsManager _cardsManager;
        [SerializeField] private Transform _enemyHand;
        [SerializeField] private Transform _playerHand;
        [SerializeField] private Transform _enemyField;
        [SerializeField] private Transform _playerField;
        [SerializeField] private BattleCard _cardPref;

        [SerializeField] private TMP_Text _turnTimeText;
        [SerializeField] private Button _endTurnButton;

        private List<BattleCard> _playerHandCards = new List<BattleCard>(),
                                 _playerFieldCards = new List<BattleCard>(),
                                 _enemyHandCards = new List<BattleCard>(),
                                 _enemyFieldCards = new List<BattleCard>();

        private int turn;
        private int turnTime = 30;

        private List<CardDataSO> _enemyDeck, _playerDeck;

        private void Start()
        {
            turn = 0;


            _enemyDeck = GiveDeckCard();
            _playerDeck = GiveDeckCard();


            GiveHandCard(_enemyDeck, _enemyHand);
            GiveHandCard(_playerDeck, _playerHand);

            StartCoroutine(TurnFunc());
        }


        private List<CardDataSO> GiveDeckCard()
        {
            List<CardDataSO> list = new List<CardDataSO>();
            for (int i = 0; i < _cardsManager.AllCards.Count; i++)
                list.Add(_cardsManager.GetRandomCard());
            return list;
        }

        private void GiveHandCard(List<CardDataSO> deck, Transform hand)
        {
            int i = 0;
            while (i++ < 4)
                GiveCardToHand(deck, hand);
        }

        private void GiveCardToHand(List<CardDataSO> deck, Transform hand)
        {
            if (deck.Count == 0)
                return;

            CardDataSO card = deck[0];

            BattleCard cardGO = Instantiate(_cardPref,hand,false);

            cardGO.SetupBattleCard(card);

            if (hand == _enemyHand )
            {
                cardGO.EnableCardBack(true);
                _enemyHandCards.Add(cardGO);
            }
            else
            {
                cardGO.EnableCardBack(false);
                _playerHandCards.Add(cardGO);
                cardGO.GetComponent<AttackCard>().enabled = false;
            }

            deck.RemoveAt(0);
        }

        private IEnumerator TurnFunc()
        {
            turnTime = 30;
            _turnTimeText.text = turnTime.ToString();

            foreach (var card in _playerFieldCards)
            {
                card.DeHighLightCard();
            }

            if (IsPlayerTurn)
            {
                foreach (var card in _playerFieldCards)
                {
                    card.ChangeAttackState(true);
                    card.HighLightCard();
                }
                    

                while (turnTime-- > 0)
                {
                    _turnTimeText.text = turnTime.ToString();
                    yield return new WaitForSeconds(1);
                }
            }
            else
            {
                foreach (var card in _enemyFieldCards)
                    card.ChangeAttackState(true);

                while (turnTime-- > 27)
                {
                    _turnTimeText.text = turnTime.ToString();
                    yield return new WaitForSeconds(1);
                }

                if (_enemyHandCards.Count > 0)
                    EnemyTurn(_enemyHandCards);
            }

            ChangeTurn();
        }

        private void EnemyTurn(List<BattleCard> cards)
        {
            int count = cards.Count == 1 ? 1 :
                Random.Range(1, cards.Count);

            for (int i = 0; i < count; i++)
            {
                if (_enemyFieldCards.Count > 5)
                    return;

                cards[0].EnableCardBack(false);
                cards[0].transform.SetParent(_enemyField);

                _enemyFieldCards.Add(cards[0]);
                _enemyHandCards.Remove(cards[0]);
            }
        }
        private void GiveNewCards()
        {
            GiveCardToHand(_enemyDeck ,_enemyHand);
            GiveCardToHand(_playerDeck, _playerHand);
        }

        public void ChangeTurn()
        {
            StopAllCoroutines();
            turn++;
            _endTurnButton.interactable = IsPlayerTurn;

            if (IsPlayerTurn)
                GiveNewCards();

            StartCoroutine(TurnFunc());
        }

        public void AddCardInField(FieldType fieldType, BattleCard card)
        {
            if (fieldType == FieldType.SELF_FIELD)
                _playerFieldCards.Add(card);

            if (fieldType == FieldType.ENEMY_FIELD)
                _enemyFieldCards.Add(card);
        }

        public void RemoveHandCard(FieldType fieldType, BattleCard card)
        {
            if (fieldType == FieldType.SELF_HAND)
                _playerHandCards.Remove(card);

            if (fieldType == FieldType.ENEMY_HAND)
                _enemyHandCards.Remove(card);
        }
    }
}
