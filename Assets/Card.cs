using Assets.Scripts.Enums;
using Assets.Scripts.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{ 
    public class Card 
    {
        public bool isALive { get { return Defence > 0; } }
        public bool HasAbility { get { return _abilities.Count > 0; } }
        public bool IsProvocation { get { return _abilities.Exists(x => x == AbilityType.PROVOCATION); } }

        public bool IsSpell => _isSpell;
        public List<AbilityType> Abilities => _abilities;

        public string Name;
        public Sprite Logo;
        public int Attack;
        public int Defence;
        public int Manacost;
        
        protected bool _isSpell;

        public bool CanAttack;
        public bool IsPlaced;

        protected List<AbilityType> _abilities;

        public int timeDealDamage;


        private UnitCardDataSO _unitCardDataSO;
        public Card()
        {
                
        }

        public Card(UnitCardDataSO unitCardDataSO)
        {         
            _unitCardDataSO = unitCardDataSO;

            Name = _unitCardDataSO.CardName;
            Logo = _unitCardDataSO.CardMainSprite;
            Attack = _unitCardDataSO.AttackPoints;
            Defence = _unitCardDataSO.DefencePoints;
            Manacost = _unitCardDataSO.ManaCostPoints;
            CanAttack = false;
            IsPlaced = false;
            _abilities = new List<AbilityType>();

            if (_unitCardDataSO.CardAbility != 0)
                _abilities.Add(_unitCardDataSO.CardAbility);

            timeDealDamage = 0;
        }

        public Card(Card card)
        {
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

        public void GetDamage(int damage)
        {
            if (damage > 0)
            {
                if (_abilities.Exists(x => x == AbilityType.SHIELD))
                    _abilities.Remove(AbilityType.SHIELD);
                else
                    Defence -= damage;
            }
        }

        public Card GetCopy()
        {
            return new Card(this);
        }
    }
}

