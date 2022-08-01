using Assets.Scripts.Enums;
using Assets.Scripts.SO;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{ 
    public class SpellCard : Card
    {
        public int SpellValue => _spellValue;
        public SpellType Spell => _spell;
        public TargetType SpellTarget => _spellTarget;

        private SpellType _spell;
        private TargetType _spellTarget;
        private int _spellValue;
        private SpellCardDataSO _spellCardDataSO;


        public SpellCard(SpellCardDataSO spellCardDataSO)
        {
            _spellCardDataSO = spellCardDataSO;
            isSpell = true;

            _spell = _spellCardDataSO.SpellType;
            _spellTarget = _spellCardDataSO.TargetType;
            _spellValue = _spellCardDataSO.SpellValue;

            Name = _spellCardDataSO.CardName;
            Logo = _spellCardDataSO.CardMainSprite;

            Manacost = _spellCardDataSO.ManaCostPoints;
            CanAttack = false;
            IsPlaced = false;
            _abilities = new List<AbilityType>();

            timeDealDamage = 0;
        }

        public SpellCard(SpellCard card)
        {
            isSpell = true;

            _spell = card._spell;
            _spellTarget = card._spellTarget;
            _spellValue = card._spellValue;

            Name = card.Name;
            Logo = card.Logo;
            Attack = card.Attack;
            Defence = card.Defence;
            Manacost = card.Manacost;
            CanAttack = false;
            IsPlaced = false;

            _abilities = new List<AbilityType>(card._abilities);

            timeDealDamage = 0;
        }

        public new SpellCard GetCopy()
        {
            return new SpellCard(this);
        }
    }
}
