using UnityEngine;
using Assets.Scripts.Enums;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "SpellCard_", menuName = "ScriptableObjects/SpellCardData", order = 2)]
    public class SpellCardDataSO : CardDataSO
    {
        public SpellType SpellType => _spell;
        public TargetType TargetType => _spellTarget;
        public int SpellValue => _spellValue;

        [SerializeField] private SpellType _spell;
        [SerializeField] private TargetType _spellTarget;
        [SerializeField] private int _spellValue;
    }
}