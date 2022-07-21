using UnityEngine.UI;
using TMPro;
using Assets.Scripts.SO;
using UnityEngine;
using Assets.Scripts.Enums;
using System.Collections.Generic;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Models
{
    public class BattleCard : MonoBehaviour
    {
        public GameManager GameManager => _gameManager;
        public int Defence => _currentDefencePoints;
        public int ManaCostPoints => _currentManaCostPoints;
        public int AttackPoints => _currentAttackPoints;
        public bool IsAlive => _currentDefencePoints > 0;
        public bool IsCanAttack => _isCanAttack;
        public bool IsPlaced => _isPlaced;
        public int TimesDealDamage { get; set; }
        public bool IsPlayer => _isPlayer;
        public List<CardAbility> ListAbilities => _allAbilities;

        public CardAbilities CardAbilities => _cardAbilities;

        public bool HasAbility
        {
            get { return _allAbilities.Count > 0; }
        }

        public bool IsProvocation
        {
            get { return _allAbilities.Exists(x => x == CardAbility.PROVOCATION); }
        }

        public Color NormalColor => _normalColor;
        public Color TargetColor => _targetColor;
        public FieldType FieldType => _fieldType;

        [Header("Components")]
        [SerializeField] private CardAbilities _cardAbilities;

        [Header("UI")]
        [SerializeField] private Image _cardImage;
        [SerializeField] private Image _logoImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _attackPointsText;
        [SerializeField] private TMP_Text _defencePointsText;
        [SerializeField] private TMP_Text _manaCostPointsText;
        [SerializeField] private GameObject _hideObj;
        [SerializeField] private GameObject _highlitedObj;
        private List<CardAbility> _allAbilities;

        
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _targetColor;
        private CardDataSO _cardDataSO;
        private FieldType _fieldType;

        private bool _isCanAttack;
        private bool _isPlaced;
        private bool _isPlayer;

        private int _currentDefencePoints;
        private int _currentAttackPoints;
        private int _currentManaCostPoints;

        private GameManager _gameManager;

        public void SetupBattleCard(CardDataSO cardDataSO, bool isPlayer)
        {
            _gameManager = FindObjectOfType<GameManager>();

            _isPlayer = isPlayer;
            _cardDataSO = cardDataSO;
            _logoImage.sprite = _cardDataSO.CardMainSprite;
            _nameText.text = _cardDataSO.CardName;


            _currentDefencePoints = _cardDataSO.DefencePoints;
            _currentAttackPoints = _cardDataSO.AttackPoints;
            _currentManaCostPoints = _cardDataSO.ManaCostPoints;
            RefreshData();

            _allAbilities = new List<CardAbility>();
            if (_cardDataSO.CardAbility != 0)
                _allAbilities.Add(_cardDataSO.CardAbility);

            TimesDealDamage = 0;
        }


        public void SetFieldType(FieldType type)
        {
            _fieldType = type;
        }

        public void OnCast()
        {
            if(_isPlayer)
            {

                _gameManager.RemoveHandCard(FieldType.SELF_HAND, this);
                _gameManager.AddCardInField(FieldType.SELF_FIELD, this);
                _gameManager.ReduceMana(true, ManaCostPoints);
                _gameManager.CheckCardForAvaliabtlity();
            }
            else
            {
                _gameManager.RemoveHandCard(FieldType.ENEMY_HAND, this);
                _gameManager.AddCardInField(FieldType.ENEMY_FIELD, this);
                _gameManager.ReduceMana(false, ManaCostPoints);
            }

            _isPlaced = true;

            if (HasAbility)
                _cardAbilities.OnCast();
        }

        //public void OnTakeDamage(BattleCard card = null)
        //{
        //    //CheckForAvaliability();
        //    _allAbilities.OnTake
        //}
        //public void OnDamageDeal()
        //{
        //    _timesDealDamage++;
        //    _isCanAttack = false;

        //    if (HasAbility)
        //        _allAbilities.OnDamageDeal();
            
        //}

        public void GetPlaced(bool placed)
        {
            _isPlaced = placed;
        }
        public void ChangeAttackState(bool can)
        {
            TimesDealDamage++;
            _isCanAttack = can;
            EnableCardLight(can);

            
        }

        public void GetDamage(int damage)
        {
            if(damage > 0)
            {
                if (_allAbilities.Exists(x => x == CardAbility.SHIELD))
                    _allAbilities.Remove(CardAbility.SHIELD);
                else
                    _currentDefencePoints -= damage;
            }
            
        }

        public void RefreshData()
        {
            _attackPointsText.text = _currentAttackPoints.ToString();
            _defencePointsText.text = _currentDefencePoints.ToString();
            _manaCostPointsText.text = _currentManaCostPoints.ToString();

        }

        public void EnableCardBack(bool isEnable)
        {
            _hideObj.SetActive(isEnable);
        }

        public void EnableCardLight(bool isEnable)
        {
            _highlitedObj.SetActive(isEnable);
        }

        public void CheckForAvaliability(int currentMana)
        {
            GetComponent<CanvasGroup>().alpha = currentMana >= ManaCostPoints ?
                1 :
                .5f;
        }
        
        public void AddDefencePoints(int value)
        {
            _currentDefencePoints += value;
        }

        public void HighLightAsTarget(bool hightlight)
        {
            _cardImage.color = hightlight ? _targetColor : _normalColor;
        }

        public void OnDamageDeal()
        {
            TimesDealDamage++;
        }
    }
}
