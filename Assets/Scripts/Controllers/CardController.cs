//using UnityEngine;
//using Assets.Scripts.SO;
//using Assets.Scripts.Models;
//using Assets.Scripts.Managers;
//using System.Collections.Generic;
//using Assets.Scripts.Enums;

//namespace Assets.Scripts.Controllers
//{

//    public class CardController : MonoBehaviour
//    {
//        public CardDataSO CardatSo => _cardDataSO;
//        public BattleCard BattleC => _battleCard;
//        public CardScripts CardScr => _cardScript;
//        public bool IsPlayerCard => _isPlayerCard;
//        public int ManaCostPoints => _currentManaCostPoints;
//        public int AttackPoints => _currentAttackPoints;
//        public bool IsAlive => _currentDefencePoints > 0;
//        public bool IsCanAttack => _isCanAttack;
//        public bool IsPlaced => _isPlaced;

//        public FieldType FieldType => _fieldType;

//        private CardDataSO _cardDataSO;
//        private bool _isPlayerCard;
//        private BattleCard _battleCard;
//        private CardScripts _cardScript;
//        private GameManager _gameManager;

//        private int _currentDefencePoints;
//        private int _currentAttackPoints;
//        private int _currentManaCostPoints;

//        private bool _isCanAttack;
//        private bool _isPlaced;

//        private Transform _defaultParent, _defaultTempCardParent;
//        private FieldType _fieldType;

//        public void Init(CardDataSO cardDataSO, bool isPlayerCard)
//        {
//            _cardDataSO = cardDataSO;
//            _isPlayerCard = isPlayerCard;
//            _gameManager = GameManager.instance;

//            if(isPlayerCard)
//            {
//                _battleCard.SetupBattleCard(_cardDataSO);
//                GetComponent<AttackCard>().enabled = false;
//            }
//            else
//            {
//                _battleCard.EnableCardBack(false);
//            }
            
//        }

//        public void SetDefaultParent(Transform parent)
//        {
//            _defaultParent = parent;
//        }

//        public void OnCast()
//        {
//            if(IsPlayerCard)
//            {
//                _gameManager.ReduceMana(true, BattleC.ManaCostPoints);
//                _gameManager.CheckCardForAvaliabtlity();
//            }
//            else
//            {
//                _gameManager.ReduceMana(false, BattleC.ManaCostPoints);
//            }
//            is
//        }
//        public void GetPlaced(bool placed)
//        {
//            _isPlaced = placed;
//        }
//        public void OnTakeDamage(CardController attacker = null)
//        {
//            CheckForALive();
//        }

//        public void OnDamageDeal()
//        {
//            BattleC.ChangeAttackState(false);
//            BattleC.EnableCardLight(false);
//        }

//        public void CheckForALive()
//        {
//            if (_battleCard.IsAlive)
//                BattleC.RefreshData();
//            else
//                DestroyCard();
//        }

//        public void DestroyCard()
//        {
//            _cardScript.OnEndDrag(null);

//            RemoveCardFromList(_gameManager.ListPlayerHand);
//            RemoveCardFromList(_gameManager.ListPlayerField);
//            RemoveCardFromList(_gameManager.ListEnemyHand);
//            RemoveCardFromList(_gameManager.ListEnemyField);


//            Destroy(gameObject);
//        }

//        private void RemoveCardFromList(List<CardController> list)
//        {
//            if (list.Exists(x => x == this))
//                list.Remove(this);
//        }
//    }
//}
