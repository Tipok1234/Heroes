using Assets.Scripts.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{

    public class CardsManager : MonoBehaviour
    {
        public IReadOnlyList<CardDataSO> AllCards => _allCards;

        [SerializeField] private List<CardDataSO> _allCards;

    }
}