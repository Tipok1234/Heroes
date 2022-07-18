using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SO;

namespace Assets.Scripts.Managers
{

    public class GameScript : MonoBehaviour
    {
            public List<CardDataSO> EnemyDeck, PlayerDeck,
                                    EnemyHand, PlayerHand,
                                    EnemyField, PlayerField;

            private CardsManager _cardsManager;

            public GameScript()
            {
                _cardsManager = GameObject.FindObjectOfType<CardsManager>();

                EnemyDeck = GiveDeckCard();
                PlayerDeck = GiveDeckCard();

                EnemyHand = new List<CardDataSO>();
                PlayerHand = new List<CardDataSO>();

                EnemyField = new List<CardDataSO>();
                PlayerField = new List<CardDataSO>();
            }

            private List<CardDataSO> GiveDeckCard()
            {
                List<CardDataSO> list = new List<CardDataSO>();
                for (int i = 0; i < _cardsManager.AllCards.Count; i++)
                    list.Add(_cardsManager.GetRandomCard());
                return list;
            }
        }
    
}
