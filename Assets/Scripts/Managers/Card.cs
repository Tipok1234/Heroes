using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{ 
public class Card : MonoBehaviour
{
        public bool isALive { get { return Defence > 0; } }
        public bool HasAbility { get { return _abilities.Count > 0; } }
        public bool IsProvocation { get { return _abilities.Exists(x => x == AbilityType.PROVOCATION); } }

        public string Name;
        public Sprite Logo;
        public int Attack;
        public int Defence;
        public int Manacost;
        public bool isSpell;

        public bool CanAttack;
        public bool IsPlaced;

        public List<AbilityType> _abilities;

        public int timeDealDamage;


        public Card(string name, string logoPath, int attack, int defence, int manaCost, AbilityType abilityType = 0)
        {
            Name = name;
            Logo = Resources.Load<Sprite>(logoPath);
            Attack = attack;
            Defence = defence;
            Manacost = manaCost;
            CanAttack = false;
            IsPlaced = false;
            _abilities = new List<AbilityType>();

            if (abilityType != 0)
                _abilities.Add(abilityType);

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

