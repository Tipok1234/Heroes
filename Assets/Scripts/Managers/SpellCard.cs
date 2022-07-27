using Assets.Scripts.Enums;

namespace Assets.Scripts.Managers
{ 
    public class SpellCard : Card
    {
        public SpellType _spell;
        public TargetType _spellTarget;
        public int _spellValue;

        public SpellCard(string name, string logoPath, int manaCost, SpellType spellType = 0,
                         int spellValue = 0, TargetType targetType = 0) : base(name, logoPath, 0, 0, manaCost)
        {
            isSpell = true;

            _spell = spellType;
            _spellTarget = targetType;
            _spellValue = spellValue;
        }

        public SpellCard(SpellCard card) : base(card)
        {
            isSpell = true;

            _spell = card._spell;
            _spellTarget = card._spellTarget;
            _spellValue = card._spellValue;
        }

        public new SpellCard GetCopy()
        {
            return new SpellCard(this);
        }
    }
}
