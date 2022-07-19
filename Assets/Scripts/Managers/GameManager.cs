using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.SO;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public int HealthPlayer => _healthPlayer;
        public int HealthEnemy => _healtEnemy;
        public int PlayerMana => _playerMana;
        public int EnemyMana => _enemyMana;
        public bool IsPlayerTurn
        {
            get { return turn % 2 == 0; }
        }

        public int PlayerFieldCardsCount => _playerFieldCards.Count;

        [SerializeField] private CardsManager _cardsManager;
        [SerializeField] private Transform _enemyHand;
        [SerializeField] private Transform _playerHand;
        [SerializeField] private Transform _enemyField;
        [SerializeField] private Transform _playerField;
        [SerializeField] private BattleCard _cardPref;
        [SerializeField] private GameObject _resultGO;

        [SerializeField] private TMP_Text _turnTimeText;
        [SerializeField] private TMP_Text _playerManaText;
        [SerializeField] private TMP_Text _enemyManaText;
        [SerializeField] private TMP_Text _healthPlayerHeroText;
        [SerializeField] private TMP_Text _healthEnemyHeroText;
        [SerializeField] private TMP_Text _resultWinnerText;

        [SerializeField] private Button _endTurnButton;

        private List<BattleCard> _playerHandCards = new List<BattleCard>(),
                                 _playerFieldCards = new List<BattleCard>(),
                                 _enemyHandCards = new List<BattleCard>(),
                                 _enemyFieldCards = new List<BattleCard>();

        private int turn;
        private int turnTime = 10;
        private int _enemyMana = 10;
        private int _playerMana = 10;
        private int _healthPlayer;
        private int _healtEnemy;

        private List<CardDataSO> _enemyDeck, _playerDeck;

        private void Awake()
        {
            AttackCard.CardFigthAction += OnCardFigthAction;

            turn = 0;

            _enemyDeck = GiveDeckCard();
            _playerDeck = GiveDeckCard();

            _healtEnemy = 30;
            _healthPlayer = 30;

            GiveHandCard(_enemyDeck, _enemyHand);
            GiveHandCard(_playerDeck, _playerHand);

            ShowMana();
            ShowHealthHero();

            StartCoroutine(TurnFunc());
        }

        private void OnCardFigthAction(BattleCard playerCard, BattleCard enemyCard)
        {
            CardsFight(playerCard, enemyCard);
        }

        private List<CardDataSO> GiveDeckCard()
        {
            List<CardDataSO> list = new List<CardDataSO>();
            for (int i = 0; i < _cardsManager.AllCards.Count; i++)
                list.Add(_cardsManager.GetRandomCard());
            return list;
        }

        private void GiveHandCard(List<CardDataSO> deck, Transform hand)
        {
            int i = 0;
            while (i++ < 4)
                GiveCardToHand(deck, hand);
        }

        private void GiveCardToHand(List<CardDataSO> deck, Transform hand)
        {
            if (deck.Count == 0)
                return;

            CardDataSO card = deck[0];

            BattleCard cardGO = Instantiate(_cardPref, hand, false);

            cardGO.SetupBattleCard(card);

            if (hand == _enemyHand)
            {
                cardGO.EnableCardBack(true);
                _enemyHandCards.Add(cardGO);
            }
            else
            {
                cardGO.EnableCardBack(false);
                _playerHandCards.Add(cardGO);
                cardGO.GetComponent<AttackCard>().enabled = false;
            }

            deck.RemoveAt(0);
        }

        private IEnumerator TurnFunc()
        {
            turnTime = 30;
            _turnTimeText.text = turnTime.ToString();

            foreach (var card in _playerFieldCards)
            {
                card.EnableCardLight(false);
            }

            if (IsPlayerTurn)
            {
                foreach (var card in _playerFieldCards)
                {
                    card.ChangeAttackState(true);
                    card.EnableCardLight(true);
                }


                while (turnTime-- > 0)
                {
                    _turnTimeText.text = turnTime.ToString();
                    yield return new WaitForSeconds(1);
                }
            }
            else
            {
                foreach (var card in _enemyFieldCards)
                    card.ChangeAttackState(true);

                while (turnTime-- > 27)
                {
                    _turnTimeText.text = turnTime.ToString();
                    yield return new WaitForSeconds(1);
                }

                if (_enemyHandCards.Count > 0)
                    EnemyTurn(_enemyHandCards);
            }

            ChangeTurn();
        }

        private void EnemyTurn(List<BattleCard> cards)
        {
            int count = cards.Count == 1 ? 1 :
                Random.Range(1, cards.Count);

            for (int i = 0; i < count; i++)
            {
                if (_enemyFieldCards.Count > 5 || _enemyMana == 0)
                    return;

                List<BattleCard> cardList = cards.FindAll(x => _enemyMana >= x.ManaCostPoints);

                if (cardList.Count == 0)
                    break;

                ReduceMana(false, cardList[0].ManaCostPoints);

                cardList[0].EnableCardBack(false);
                cardList[0].transform.SetParent(_enemyField);
                cardList[0].SetFieldType(FieldType.ENEMY_FIELD);

                _enemyFieldCards.Add(cardList[0]);
                _enemyHandCards.Remove(cardList[0]);
            }

            foreach (var activeCard in _enemyFieldCards.FindAll(x => x.IsCanAttack))
            {
                if (Random.Range(0, 2) == 0 && _playerFieldCards.Count > 0)
                {
                    var enemy = _playerFieldCards[Random.Range(0, _playerFieldCards.Count)];

                    // Debug.LogError(activeCard.name + "("+ activeCard.AttackPoints + ";" + ")" + enemy.name + "(" + enemy.AttackPoints + " )" );

                    activeCard.ChangeAttackState(false);
                    CardsFight(enemy, activeCard);
                }
                else
                {
                    //Debug.LogError("ATTACK HERO!");
                    activeCard.ChangeAttackState(false);
                    DamageHero(activeCard, false);
                }
            }
        }
        private void GiveNewCards()
        {

            GiveCardToHand(_enemyDeck, _enemyHand);
            GiveCardToHand(_playerDeck, _playerHand);
        }

        public void ChangeTurn()
        {
            StopAllCoroutines();
            turn++;
            _endTurnButton.interactable = IsPlayerTurn;

            if (IsPlayerTurn)
            {
                GiveNewCards();
                _playerMana = 10;
                _enemyMana = 10;
                ShowMana();
            }


            StartCoroutine(TurnFunc());
        }

        public void AddCardInField(FieldType fieldType, BattleCard card)
        {
            if (fieldType == FieldType.SELF_FIELD)
                _playerFieldCards.Add(card);

            if (fieldType == FieldType.ENEMY_FIELD)
                _enemyFieldCards.Add(card);
        }

        public void RemoveHandCard(FieldType fieldType, BattleCard card)
        {
            if (fieldType == FieldType.SELF_HAND)
                _playerHandCards.Remove(card);

            if (fieldType == FieldType.ENEMY_HAND)
                _enemyHandCards.Remove(card);
        }

        private void CardsFight(BattleCard playerCard, BattleCard enemyCard)
        {
            enemyCard.GetDamage(playerCard.AttackPoints);
            playerCard.GetDamage(enemyCard.AttackPoints);

            if (playerCard.IsAlive)
                playerCard.RefreshData();
            else
                DestroyCard(playerCard);


            if (enemyCard.IsAlive)
                enemyCard.RefreshData();
            else
                DestroyCard(enemyCard);
        }

        private void DestroyCard(BattleCard card)
        {
            card.GetComponent<CardScripts>().OnEndDrag(null);

            if (_enemyFieldCards.Exists(x => x == card))
                _enemyFieldCards.Remove(card);

            if (_playerFieldCards.Exists(x => x == card))
                _playerFieldCards.Remove(card);

            Destroy(card.gameObject);
        }

        private void ShowMana()
        {
            _playerManaText.text = _playerMana.ToString();
            _enemyManaText.text = _enemyMana.ToString();
        }

        public void ShowHealthHero()
        {
            _healthPlayerHeroText.text = _healthPlayer.ToString();
            _healthEnemyHeroText.text = _healtEnemy.ToString();
        }

        public void ReduceMana(bool playerMana, int manaCost)
        {
            if (playerMana)
                _playerMana = Mathf.Clamp(_playerMana - manaCost, 0, int.MaxValue);
            else
                _enemyMana = Mathf.Clamp(_enemyMana - manaCost, 0, int.MaxValue);

            ShowMana();
        }

        public void DamageHero(BattleCard battleCard, bool isEnemyAttack)
        {
            if (isEnemyAttack)
                _healtEnemy = Mathf.Clamp(_healtEnemy - battleCard.AttackPoints, 0, int.MaxValue);
            else
                _healthPlayer = Mathf.Clamp(_healthPlayer - battleCard.AttackPoints, 0, int.MaxValue);

            ShowHealthHero();
            battleCard.EnableCardLight(false);
            ChechForResult();
        }

        private void ChechForResult()
        {
            if(_healtEnemy == 0 || _healthPlayer == 0)
            {
                _resultGO.SetActive(true);
                StopAllCoroutines();

                if (_healtEnemy == 0)
                    _resultWinnerText.text = "SnoopDog - WIN!";
                else
                    _resultWinnerText.text = "Gucci Mane - WIN!";
            }
        }
    }
}
