using UnityEngine.UI;
using TMPro;
using Assets.Scripts.SO;
using UnityEngine;


namespace Assets.Scripts.Models
{
    public class BattleCard : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Image _logoImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _attackPointsText;
        [SerializeField] private TMP_Text _defencePointsText;
        [SerializeField] private GameObject _hideObj;
        [SerializeField] private GameObject _highlitedObj;
        [SerializeField] private bool isCanAttack;

        private CardDataSO _cardDataSO;

        public void SetupBattleCard(CardDataSO cardDataSO)
        {
            _cardDataSO = cardDataSO;

            _logoImage.sprite = _cardDataSO.CardMainSprite;
            _nameText.text = _cardDataSO.CardName;
            _attackPointsText.text = _cardDataSO.AttackPoints.ToString();
            _defencePointsText.text = _cardDataSO.DefencePoints.ToString();
        }

        public void ChangeAttackState(bool can)
        {
            isCanAttack = can;
        }

        public void EnableCardBack(bool isEnable)
        {
            _hideObj.SetActive(isEnable);
        }

        public void HighLightCard()
        {
            _highlitedObj.SetActive(true);
        }

        public void DeHighLightCard()
        {
            _highlitedObj.SetActive(false);
        }
    }
}
