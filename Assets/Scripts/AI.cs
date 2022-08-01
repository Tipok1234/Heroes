using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Controllers;
using Assets.Scripts.Enums;

public class AI : MonoBehaviour
{
    public void MakeTurn()
    {
        StartCoroutine(EnemyTurn(GameManager.instance._enemyHandCards));
    }

    private IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(1);

        int count = cards.Count == 1 ? 1 : Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (GameManager.instance._enemyFieldCards.Count > 5 ||
                GameManager.instance._currentGame._enemy._mana == 0 ||
                GameManager.instance._enemyHandCards.Count == 0)
                break;

            List<CardController> cardList = cards.FindAll(x => GameManager.instance._currentGame._enemy._mana >= x._card.Manacost);

            if (cardList.Count == 0)
                break;

            if (cardList[0]._card.isSpell)
            {
                CastSpell(cardList[0]);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                cardList[0].GetComponent<CardMovement>().MoveToField(GameManager.instance._enemyField);
                yield return new WaitForSeconds(0.5f);
                cardList[0].transform.SetParent(GameManager.instance._enemyField);
                cardList[0].OnCast();
            }

        }

        yield return new WaitForSeconds(1);

        while (GameManager.instance._enemyFieldCards.Exists(x => x._card.CanAttack))
        {
            var activeCard = GameManager.instance._enemyFieldCards.FindAll(x => x._card.CanAttack)[0];
            bool hasProvocation = GameManager.instance._playerFieldCards.Exists(x => x._card.IsProvocation);

            if (hasProvocation ||
                Random.Range(0, 2) == 0 &&
                GameManager.instance._playerFieldCards.Count > 0)
            {
                CardController enemy;

                if (hasProvocation)
                    enemy = GameManager.instance._playerFieldCards.Find(x => x._card.IsProvocation);
                else
                    enemy = GameManager.instance._playerFieldCards[Random.Range(0, GameManager.instance._playerFieldCards.Count)];

                activeCard.CardMovement.MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(0.75f);

                GameManager.instance.CardsFight(activeCard, enemy);
            }
            else
            {
                activeCard.GetComponent<CardMovement>().MoveToTarget(GameManager.instance._playerHero.transform);

                yield return new WaitForSeconds(0.75f);

                GameManager.instance.DamageHero(activeCard, false);
            }
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1);
        GameManager.instance.ChangeTurn();
    }

    private void CastSpell(CardController card)
    {
        switch (((SpellCard)card._card).SpellTarget)
        {
            case TargetType.NO_TARGET:

                switch (((SpellCard)card._card).Spell)
                {
                    case SpellType.HEAL_ALLY_FIELD_CARDS:

                        if (GameManager.instance._enemyFieldCards.Count > 0)
                            StartCoroutine(CastCard(card));

                        break;

                    case SpellType.DAMAGE_ENEMY_FIELD_CARDS:

                        if (GameManager.instance._playerFieldCards.Count > 0)
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

                if (GameManager.instance._enemyFieldCards.Count > 0)
                    StartCoroutine(CastCard(card,
                        GameManager.instance._enemyFieldCards[Random.Range(0,GameManager.instance._enemyFieldCards.Count)]));

                break;

            case TargetType.ENEMY_CARD_TARGET:

                if (GameManager.instance._playerFieldCards.Count > 0)
                    StartCoroutine(CastCard(card,
                        GameManager.instance._playerFieldCards[Random.Range(0, GameManager.instance._playerFieldCards.Count)]));

                break;
        }
    }

    private IEnumerator CastCard(CardController spell, CardController target = null)
    {
        if(((SpellCard)spell._card).SpellTarget == TargetType.NO_TARGET)
        {
            spell.GetComponent<CardMovement>().MoveToField(GameManager.instance._enemyField);
            yield return new WaitForSeconds(0.51f);

            spell.OnCast();
        }
        else
        {
            spell.CardInfo.ShowCardInfo();
            spell.GetComponent<CardMovement>().MoveToTarget(target.transform);
            yield return new WaitForSeconds(0.51f);

            GameManager.instance._enemyHandCards.Remove(spell);
            GameManager.instance._enemyFieldCards.Add(spell);
            GameManager.instance.ReduceMana(false, spell._card.Manacost);

            spell._card.IsPlaced = true;


            spell.UseSpell(target);
        }

        //string targetStr = target == null ? "no_turget" : target._card.Name;
        //Debug.LogError("AI spell cast" + (spell._card).Name + "target: " + targetStr);
    }
}
