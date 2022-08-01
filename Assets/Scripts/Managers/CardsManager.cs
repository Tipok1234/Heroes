using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SO;
using Assets.Scripts.Models;

namespace Assets.Scripts.Managers
{
    public class CardsManager : MonoBehaviour
    {
        public  List<Card> allCards = new List<Card>();
        [SerializeField] private List<UnitCardDataSO> _cardDataSo = new List<UnitCardDataSO>();
        [SerializeField] private List<SpellCardDataSO> _spellCardDataSO = new List<SpellCardDataSO>();
        public void Awake()
        {
            for (int i = 0; i < _cardDataSo.Count; i++)
            {
                allCards.Add(new Card(_cardDataSo[i]));
            }

            for (int i = 0; i < _spellCardDataSO.Count; i++)
            {
                allCards.Add(new SpellCard(_spellCardDataSO[i]));
            }
        }
    }
}