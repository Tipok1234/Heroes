using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Enums;
using Assets.Scripts.Models;

namespace Assets.Scripts.Managers
{
    public class AIManager : MonoBehaviour
    {
        public void MakeTurn()
        {
            StartCoroutine(EnemyTurn(GameManager.instance.EnemyHandCards));
        }

        private IEnumerator EnemyTurn(List<CardController> cards)
        {
            yield return new WaitForSeconds(1);

            int count = cards.Count == 1 ? 1 : Random.Range(0, cards.Count);

            for (int i = 0; i < count; i++)
            {
                if (GameManager.instance.EnemyFieldCards.Count > 5 ||
                    GameManager.instance.CurrentGame.Enemy.Mana == 0 ||
                    GameManager.instance.EnemyHandCards.Count == 0)
                    break;

                List<CardController> cardList = cards.FindAll(x => GameManager.instance.CurrentGame.Enemy.Mana >= x.Card.Manacost);

                if (cardList.Count == 0)
                    break;

                if (cardList[0].Card.IsSpell)
                {
                    CastSpell(cardList[0]);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    cardList[0].GetComponent<CardMovement>().MoveToField(GameManager.instance.EnemyField);
                    yield return new WaitForSeconds(0.5f);
                    cardList[0].transform.SetParent(GameManager.instance.EnemyField);
                    cardList[0].OnCast();
                }
            }

            yield return new WaitForSeconds(1);

            while (GameManager.instance.EnemyFieldCards.Exists(x => x.Card.CanAttack))
            {
                var activeCard = GameManager.instance.EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
                bool hasProvocation = GameManager.instance.PlayerFieldCards.Exists(x => x.Card.IsProvocation);

                if (hasProvocation ||
                    Random.Range(0, 2) == 0 &&
                    GameManager.instance.PlayerFieldCards.Count > 0)
                {
                    CardController enemy;

                    if (hasProvocation)
                        enemy = GameManager.instance.PlayerFieldCards.Find(x => x.Card.IsProvocation);
                    else
                        enemy = GameManager.instance.PlayerFieldCards[Random.Range(0, GameManager.instance.PlayerFieldCards.Count)];

                    activeCard.CardMovement.MoveToTarget(enemy.transform);
                    yield return new WaitForSeconds(0.75f);

                    GameManager.instance.CardsFight(activeCard, enemy);
                }
                else
                {
                    if (GameManager.instance.PlayerFieldCards.Count <= 3)
                    {
                        activeCard.GetComponent<CardMovement>().MoveToTarget(GameManager.instance.PlayerHero.transform);
                        AudioManager.Instance.VoiceAttack();
                        yield return new WaitForSeconds(0.75f);

                        GameManager.instance.DamageHero(activeCard, false);
                    }
                }
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(1);
            GameManager.instance.ChangeTurn();
        }

        private void CastSpell(CardController card)
        {
            switch (((SpellCard)card.Card).SpellTarget)
            {
                case TargetType.NO_TARGET:

                    switch (((SpellCard)card.Card).Spell)
                    {
                        case SpellType.HEAL_ALLY_FIELD_CARDS:

                            if (GameManager.instance.EnemyFieldCards.Count > 0)
                                StartCoroutine(CastCard(card));

                            break;

                        case SpellType.DAMAGE_ENEMY_FIELD_CARDS:

                            if (GameManager.instance.PlayerFieldCards.Count > 0)
                                StartCoroutine(CastCard(card));

                            break;

                        case SpellType.HEAL_ALLY_HERO:
                            StartCoroutine(CastCard(card));
                            break;

                        case SpellType.DAMAGE_ENEMY_HERO:
                            StartCoroutine(CastCard(card));
                            break;
                    }
                    break;

                case TargetType.ALLY_CARD_TARGET:

                    if (GameManager.instance.EnemyFieldCards.Count > 0)
                        StartCoroutine(CastCard(card,
                            GameManager.instance.EnemyFieldCards[Random.Range(0, GameManager.instance.EnemyFieldCards.Count)]));
                    break;

                case TargetType.ENEMY_CARD_TARGET:

                    if (GameManager.instance.PlayerFieldCards.Count > 0)
                        StartCoroutine(CastCard(card,
                            GameManager.instance.PlayerFieldCards[Random.Range(0, GameManager.instance.PlayerFieldCards.Count)]));
                    break;
            }
        }

        private IEnumerator CastCard(CardController spell, CardController target = null)
        {
            if (((SpellCard)spell.Card).SpellTarget == TargetType.NO_TARGET)
            {
                spell.GetComponent<CardMovement>().MoveToField(GameManager.instance.EnemyField);
                yield return new WaitForSeconds(0.51f);

                spell.OnCast();
            }
            else
            {
                spell.CardInfo.ShowCardInfo();
                spell.GetComponent<CardMovement>().MoveToTarget(target.transform);
                yield return new WaitForSeconds(0.51f);

                GameManager.instance.EnemyHandCards.Remove(spell);
                GameManager.instance.EnemyFieldCards.Add(spell);
                GameManager.instance.ReduceMana(false, spell.Card.Manacost);

                spell.Card.IsPlaced = true;

                spell.UseSpell(target);
            }
        }
    }
}
