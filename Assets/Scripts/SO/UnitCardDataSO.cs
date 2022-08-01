using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "UnitCardData_", menuName = "ScriptableObjects/UnitCardData", order = 1)]
    public class UnitCardDataSO : CardDataSO
    {
        public int AttackPoints => _attackPoints;
        public int DefencePoints => _defencePoints;
        public AbilityType CardAbility => _cardAbility;

        [Header("Stats")]
        [SerializeField] private int _attackPoints;
        [SerializeField] private int _defencePoints;
        [SerializeField] private AbilityType _cardAbility;

    }
}
