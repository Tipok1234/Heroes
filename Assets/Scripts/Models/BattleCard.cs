using UnityEngine.UI;
using TMPro;
using Assets.Scripts.SO;
using UnityEngine;
using Assets.Scripts.Enums;

namespace Assets.Scripts.Models
{
    public class BattleCard : MonoBehaviour
    {
        public int ManaCostPoints => _currentManaCostPoints;
        public int AttackPoints => _currentAttackPoints;
        public bool IsAlive => _currentDefencePoints > 0;
        public bool IsCanAttack => _isCanAttack;
        public bool IsPlaced => _isPlaced;

        public Color NormalColor => _normalColor;
        public Color TargetColor => _targetColor;
        public FieldType FieldType => _fieldType;


        [Header("UI")]
        [SerializeField] private Image _cardImage;
        [SerializeField] private Image _logoImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _attackPointsText;
        [SerializeField] private TMP_Text _defencePointsText;
        [SerializeField] private TMP_Text _manaCostPointsText;
        [SerializeField] private GameObject _hideObj;
        [SerializeField] private GameObject _highlitedObj;

        //[SerializeField] private bool _isPlayer;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _targetColor;
        private CardDataSO _cardDataSO;
        private FieldType _fieldType;

        private bool _isCanAttack;
        private bool _isPlaced;

        private int _currentDefencePoints;
        private int _currentAttackPoints;
        private int _currentManaCostPoints;
        


        public void SetupBattleCard(CardDataSO cardDataSO)
        {
            _cardDataSO = cardDataSO;
            _logoImage.sprite = _cardDataSO.CardMainSprite;
            _nameText.text = _cardDataSO.CardName;


            _currentDefencePoints = _cardDataSO.DefencePoints;
            _currentAttackPoints = _cardDataSO.AttackPoints;
            _currentManaCostPoints = _cardDataSO.ManaCostPoints;
            RefreshData();
        }

        public void SetFieldType(FieldType type)
        {
            _fieldType = type;
        }

        public void GetPlaced(bool placed)
        {
            _isPlaced = placed;
        }
        public void ChangeAttackState(bool can)
        {   
            _isCanAttack = can;
            EnableCardLight(can);
        }

        public void GetDamage(int damage)
        {
            _currentDefencePoints -= damage;
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

        public void HighLightAsTarget(bool hightlight)
        {
            _cardImage.color = hightlight ? _targetColor : _normalColor;
        }
    }
}
