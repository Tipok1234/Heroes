using Assets.Scripts.Models;
using Assets.Scripts.SO;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CardsManager : MonoBehaviour
    {
        public IReadOnlyList <CardDataSO> AllCards => _allCards;

        [SerializeField] private List<CardDataSO> _allCards;


        [SerializeField] private List<BattleCard> _battleCard;

        public CardDataSO GetRandomCard()
        {
            return AllCards[Random.Range(0, _allCards.Count)];
        }

    }
}