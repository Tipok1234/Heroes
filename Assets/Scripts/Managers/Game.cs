using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
        [SerializeField] private CardsManager _cardsManager;
        public Player _player;
        public Player _enemy;
        public List<Card> _enemyDeck, _playerDeck;

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

            //UIController.instance.UpdatePlayerDeckCard(list.Count);

            for (int i = 0; i < _cardsManager.allCards.Count; i++)
            {
                var card = _cardsManager.allCards[Random.Range(0, _cardsManager.allCards.Count)];


                if (card.isSpell)
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
