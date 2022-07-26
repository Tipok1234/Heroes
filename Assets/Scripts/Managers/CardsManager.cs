using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class Card
    {
        public enum AbilityType
        {
            NO_ABILITY,
            INSTANT_ACTIVE,
            DOUBLE_ATTACK,
            SHIELD,
            PROVOCATION,
            REGENIRATION_EACH_TURN,
            COUNTER_ATTACK
        }
        public bool isALive { get { return Defence > 0; } }
        public bool HasAbility { get { return _abilities.Count > 0; } }
        public bool IsProvocation { get { return _abilities.Exists(x => x == AbilityType.PROVOCATION); } }
        public bool IsSpell;


        public string Name;
        public Sprite Logo;
        public int Attack;
        public int Defence;
        public int Manacost;

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
            if(damage > 0)
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

    public class SpellCard : Card
    {
        public enum SpellType
        {
            NO_SPELL,
            HEAL_ALLY_FIELD_CARDS,
            DAMAGE_ENEMY_FIELD_CARDS,
            HEAL_ALLY_HERO,
            DAMAGE_ENEMY_HERO,
            HEAL_ALLY_CARD,
            DAMAGE_ENEMY_CARD,
            SHIELD_ON_ALLY_CARD,
            PROVOCATION_ON_ALLY_CARD,
            BUFF_CARD_DAMAGE,
            DEBUFF_CARD_DAMAGE
        }

        public enum TargetType
        {
            NO_TARGET,
            ALLY_CARD_TARGET,
            ENEMY_CARD_TARGET
        }


        public SpellType _spell;
        public TargetType _spellTarget;
        public int _spellValue;

        public SpellCard(string name, string logoPath, int manaCost, SpellType spellType = 0,
                         int spellValue = 0, TargetType targetType = 0) : base(name, logoPath, 0, 0, manaCost)
        {
            IsSpell = true;

            _spell = spellType;
            _spellTarget = targetType;
            _spellValue = spellValue;
        }

        public SpellCard(SpellCard card) : base(card)
        {
            IsSpell = true;

            _spell = card._spell;
            _spellTarget = card._spellTarget;
            _spellValue = card._spellValue;
        }

        public new SpellCard GetCopy()
        {
            return new SpellCard(this);
        }
    }
    public static class CardManager
    {
        public static List<Card> allCards = new List<Card>();
    }
    public class CardsManager : MonoBehaviour
    {
        public void Awake()
        {
            CardManager.allCards.Add(new Card("Eminem", "Sprites/Cards/FreshMan/Image_Eminem",5,5,5));
            CardManager.allCards.Add(new Card("Feduk", "Sprites/Cards/FreshMan/Image_Feduk", 7,3,4));
            CardManager.allCards.Add(new Card("Frame Tamer", "Sprites/Cards/FreshMan/Image_Frame_Tamer", 2, 8,4));
            CardManager.allCards.Add(new Card("KISH", "Sprites/Cards/FreshMan/Image_KISH", 4, 4,3));
            CardManager.allCards.Add(new Card("Kizaru", "Sprites/Cards/FreshMan/Image_Kizaru", 7, 7, 7));
            CardManager.allCards.Add(new Card("OG Buda", "Sprites/Cards/FreshMan/Image_Og_Buda", 3, 5, 4));
            CardManager.allCards.Add(new Card("Morgenshtern", "Sprites/Cards/FreshMan/Image_Morgenshtern", 6, 4, 5));

            CardManager.allCards.Add(new Card("Provocation", "Sprites/Cards/FreshMan/Unglystephan", 1, 3, 2, Card.AbilityType.PROVOCATION));
            CardManager.allCards.Add(new Card("Regeniration", "Sprites/Cards/FreshMan/Soda_luv", 6, 3, 5, Card.AbilityType.REGENIRATION_EACH_TURN));
            CardManager.allCards.Add(new Card("Double Attack", "Sprites/Cards/FreshMan/Scally_Milano", 2, 8, 4, Card.AbilityType.DOUBLE_ATTACK));
            CardManager.allCards.Add(new Card("Instant Active", "Sprites/Cards/FreshMan/Offmi", 4, 4, 3, Card.AbilityType.INSTANT_ACTIVE));
            CardManager.allCards.Add(new Card("Shield", "Sprites/Cards/FreshMan/Image_Mayot", 7, 7, 7, Card.AbilityType.SHIELD));
            CardManager.allCards.Add(new Card("Counter Attack", "Sprites/Cards/FreshMan/Image_Kizaru", 3, 5, 4, Card.AbilityType.COUNTER_ATTACK));


            CardManager.allCards.Add(new SpellCard("HEAL ALLY FIELD CARDS", "Sprites/Cards/Spells/HEAL_ALLY_FIELD_CARDS",  2, SpellCard.SpellType.HEAL_ALLY_FIELD_CARDS,2, SpellCard.TargetType.NO_TARGET));
            CardManager.allCards.Add(new SpellCard("DAMAGE ENEMY FIELD CARDS", "Sprites/Cards/Spells/DAMAGE_ENEMY_FIELD_CARDS",  2, SpellCard.SpellType.DAMAGE_ENEMY_FIELD_CARDS,2, SpellCard.TargetType.NO_TARGET));
            CardManager.allCards.Add(new SpellCard("HEAL ALLY HERO ", "Sprites/Cards/Spells/HEAL_ALLY_HERO",  2, SpellCard.SpellType.HEAL_ALLY_HERO,2, SpellCard.TargetType.NO_TARGET));
            CardManager.allCards.Add(new SpellCard("DAMAGE ENEMY HERO ", "Sprites/Cards/Spells/DAMAGE_ENEMY_HERO",  2, SpellCard.SpellType.DAMAGE_ENEMY_HERO,2, SpellCard.TargetType.NO_TARGET));
            CardManager.allCards.Add(new SpellCard("HEAL ALLY CARD", "Sprites/Cards/Spells/HEAL_ALLY_CARD",  2,  SpellCard.SpellType.HEAL_ALLY_CARD, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
            CardManager.allCards.Add(new SpellCard("DAMAGE ENEMY CARD ", "Sprites/Cards/Spells/DAMAGE_ENEMY_CARD",  2,  SpellCard.SpellType.DAMAGE_ENEMY_HERO, 2, SpellCard.TargetType.ENEMY_CARD_TARGET));
            CardManager.allCards.Add(new SpellCard("SHIELD ON ALLY CARD", "Sprites/Cards/Spells/SHIELD_ON_ALLY_CARD", 2, SpellCard.SpellType.SHIELD_ON_ALLY_CARD, 2, SpellCard.TargetType.ALLY_CARD_TARGET));
            CardManager.allCards.Add(new SpellCard("PROVOCATION ON ALLY CARD ", "Sprites/Spells/FreshMan/PROVOCATION_ON_ALLY_CARD",  2, SpellCard.SpellType.PROVOCATION_ON_ALLY_CARD,0, SpellCard.TargetType.ALLY_CARD_TARGET));
            CardManager.allCards.Add(new SpellCard("BUFF CARD DAMAGE","Sprites/Cards/Spells/BUFF_CARD_DAMAGE", 2, SpellCard.SpellType.BUFF_CARD_DAMAGE,2, SpellCard.TargetType.ALLY_CARD_TARGET));
            CardManager.allCards.Add(new SpellCard("DEBUFF CARD DAMAGE ", "Sprites/Cards/Spells/DEBUFF_CARD_DAMAGE",  2, SpellCard.SpellType.DEBUFF_CARD_DAMAGE,2, SpellCard.TargetType.ENEMY_CARD_TARGET));
        }

    }
}