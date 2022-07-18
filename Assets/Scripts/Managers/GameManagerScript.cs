using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.Managers;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.SO
{
    public class GameManagerScript : MonoBehaviour
    {
        private GameScript currentGame;
        [SerializeField] private Transform _enemyHand;
        [SerializeField] private Transform _playerHand;
        [SerializeField] private BattleCard _cardPref;
        private int turn;
        private int turnTime = 30;
        public TMP_Text _turnTimeText;
        public Button _endTurnButton;

        public bool IsPlayerTurn
        {
            get { return turn % 2 == 0; }
        }


        private void Start()
        {
            turn = 0;

            currentGame = new GameScript();

            GiveHandCard(currentGame.EnemyDeck, _enemyHand);
            GiveHandCard(currentGame.PlayerDeck, _playerHand);

            StartCoroutine(TurnFunc());
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

            if (hand == _playerHand )
                cardGO.SetupBattleCard(card);
            else
                cardGO.HideCardInfo(card);

            deck.RemoveAt(0);
        }

        private IEnumerator TurnFunc()
        {
            turnTime = 30;
            _turnTimeText.text = turnTime.ToString();

            if (IsPlayerTurn)
            {
                while(turnTime-- > 0)
                {
                    _turnTimeText.text = turnTime.ToString();
                    yield return new WaitForSeconds(1);
                }
            }
            else
            {
                while(turnTime-- > 27)
                {
                    _turnTimeText.text = turnTime.ToString();
                    yield return new WaitForSeconds(1);
                }
            }

            ChangeTurn();
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

        private void GiveNewCards()
        {
            GiveCardToHand(currentGame.EnemyDeck, _enemyHand);
            GiveCardToHand(currentGame.PlayerDeck, _playerHand);
        }
    }
}
