using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
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
            list.Add(CardsManager.allCards[6].GetCopy());

            //UIController.instance.UpdatePlayerDeckCard(list.Count);

            for (int i = 0; i < CardsManager.allCards.Count; i++)
            {
                var card = CardsManager.allCards[Random.Range(0, CardsManager.allCards.Count)];


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
