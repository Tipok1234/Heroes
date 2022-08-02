using Assets.Scripts.Managers;
using System.Collections.Generic;
using Assets.Scripts.UI;
using Assets.Scripts.Models;
using UnityEngine;

public class Game : MonoBehaviour
{
        public Player Player => _player;
        public Player Enemy => _enemy;

        public List<Card> EnemyDeck => _enemyDeck;
        public List<Card> PlayerDeck => _playerDeck;
        

        [SerializeField] private CardsManager _cardsManager;
        private Player _player;
        private Player _enemy;

        private List<Card> _enemyDeck, _playerDeck;

        public void InitGame()
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
            list.Add(_cardsManager.allCards[6].GetCopy());

            for (int i = 0; i < _cardsManager.allCards.Count; i++)
            {
                var card = _cardsManager.allCards[Random.Range(0, _cardsManager.allCards.Count)];


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
