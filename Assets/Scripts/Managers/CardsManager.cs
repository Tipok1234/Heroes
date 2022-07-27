using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Managers
{
    public class CardsManager : MonoBehaviour
    {
        public static List<Card> allCards = new List<Card>();
        public void Awake()
        {
            allCards.Add(new Card("Eminem", "Sprites/Cards/FreshMan/Image_Eminem",5,5,5));
            allCards.Add(new Card("Feduk", "Sprites/Cards/FreshMan/Image_Feduk", 7,3,4));
            allCards.Add(new Card("Frame Tamer", "Sprites/Cards/FreshMan/Image_Frame_Tamer", 2, 8,4));
            allCards.Add(new Card("KISH", "Sprites/Cards/FreshMan/Image_KISH", 4, 4,3));
            allCards.Add(new Card("Kizaru", "Sprites/Cards/FreshMan/Image_Kizaru", 7, 7, 7));
            allCards.Add(new Card("OG Buda", "Sprites/Cards/FreshMan/Image_Og_Buda", 3, 5, 4));
            allCards.Add(new Card("Morgenshtern", "Sprites/Cards/FreshMan/Image_Morgenshtern", 6, 4, 5));

            allCards.Add(new Card("Provocation", "Sprites/Cards/FreshMan/Unglystephan", 1, 3, 2, AbilityType.PROVOCATION));
            allCards.Add(new Card("Regeniration", "Sprites/Cards/FreshMan/Soda_luv", 6, 3, 5, AbilityType.REGENIRATION_EACH_TURN));
            allCards.Add(new Card("Double Attack", "Sprites/Cards/FreshMan/Scally_Milano", 2, 8, 4, AbilityType.DOUBLE_ATTACK));
            allCards.Add(new Card("Instant Active", "Sprites/Cards/FreshMan/Offmi", 4, 4, 3, AbilityType.INSTANT_ACTIVE));
            allCards.Add(new Card("Shield", "Sprites/Cards/FreshMan/Image_Mayot", 7, 7, 7, AbilityType.SHIELD));
            allCards.Add(new Card("Counter Attack", "Sprites/Cards/FreshMan/Image_Kizaru", 3, 5, 4, AbilityType.COUNTER_ATTACK));


            allCards.Add(new SpellCard("HEAL ALLY FIELD CARDS", "Sprites/Cards/Spells/HEAL_ALLY_FIELD_CARDS",  2, SpellType.HEAL_ALLY_FIELD_CARDS,2, TargetType.NO_TARGET));
            allCards.Add(new SpellCard("DAMAGE ENEMY FIELD CARDS", "Sprites/Cards/Spells/DAMAGE_ENEMY_FIELD_CARDS",  2, SpellType.DAMAGE_ENEMY_FIELD_CARDS,2, TargetType.NO_TARGET));
            allCards.Add(new SpellCard("HEAL ALLY HERO ", "Sprites/Cards/Spells/HEAL_ALLY_HERO",  2, SpellType.HEAL_ALLY_HERO,2, TargetType.NO_TARGET));
            allCards.Add(new SpellCard("DAMAGE ENEMY HERO ", "Sprites/Cards/Spells/DAMAGE_ENEMY_HERO",  2, SpellType.DAMAGE_ENEMY_HERO,2, TargetType.NO_TARGET));
            allCards.Add(new SpellCard("HEAL ALLY CARD", "Sprites/Cards/Spells/HEAL_ALLY_CARD",  2,  SpellType.HEAL_ALLY_CARD, 2, TargetType.ALLY_CARD_TARGET));
            allCards.Add(new SpellCard("DAMAGE ENEMY CARD ", "Sprites/Cards/Spells/DAMAGE_ENEMY_CARD",  2,  SpellType.DAMAGE_ENEMY_HERO, 2, TargetType.ENEMY_CARD_TARGET));
            allCards.Add(new SpellCard("SHIELD ON ALLY CARD", "Sprites/Cards/Spells/SHIELD_ON_ALLY_CARD", 2, SpellType.SHIELD_ON_ALLY_CARD, 2, TargetType.ALLY_CARD_TARGET));
            allCards.Add(new SpellCard("PROVOCATION ON ALLY CARD ", "Sprites/Spells/FreshMan/PROVOCATION_ON_ALLY_CARD",  2, SpellType.PROVOCATION_ON_ALLY_CARD,0, TargetType.ALLY_CARD_TARGET));
            allCards.Add(new SpellCard("BUFF CARD DAMAGE","Sprites/Cards/Spells/BUFF_CARD_DAMAGE", 2, SpellType.BUFF_CARD_DAMAGE,2, TargetType.ALLY_CARD_TARGET));
            allCards.Add(new SpellCard("DEBUFF CARD DAMAGE ", "Sprites/Cards/Spells/DEBUFF_CARD_DAMAGE",  2, SpellType.DEBUFF_CARD_DAMAGE,2, TargetType.ENEMY_CARD_TARGET));
        }

    }
}