using UnityEngine.UI;
using TMPro;
using Assets.Scripts.SO;
using Assets.Scripts.Managers;
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

        private CardDataSO _cardDataSO;

        public void HideCardInfo(CardDataSO cardDataSO)
        {
            _cardDataSO = cardDataSO;
            _logoImage.sprite = null;
            _nameText.text = "";
            _attackPointsText.text = "";
            _defencePointsText.text = "";

        }
        public void SetupBattleCard(CardDataSO cardDataSO)
        {
            _cardDataSO = cardDataSO;

            _logoImage.sprite = _cardDataSO.CardMainSprite;
            _nameText.text = _cardDataSO.CardName;
            _attackPointsText.text = _cardDataSO.AttackPoints.ToString();
            _defencePointsText.text = _cardDataSO.DefencePoints.ToString();
        }
    }
}
