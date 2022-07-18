using Assets.Scripts.Models;
using Assets.Scripts.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CardsManager : MonoBehaviour
    {
        public IReadOnlyList <CardDataSO> AllCards => _allCards;

        [SerializeField] private List<CardDataSO> _allCards;


        [SerializeField] private List<BattleCard> _battleCard;


        private void Awake()
        {
            for(int i = 0; i <_battleCard.Count; i++)
            {
                _battleCard[i].SetupBattleCard(_allCards[i]);
            }
        }


        public CardDataSO GetRandomCard()
        {
            return AllCards[Random.Range(0, _allCards.Count)];
        }

    }
}