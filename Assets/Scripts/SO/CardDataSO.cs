using UnityEngine;

namespace Assets.Scripts.SO
{
    [CreateAssetMenu(fileName = "CardData_", menuName = "ScriptableObjects/CardData", order = 1)]
    public class CardDataSO : ScriptableObject
    {
        public int AttackPoints => _attackPoints;
        public int DefencePoints => _defencePoints;

        public string CardName => _cardName;
        public Sprite CardMainSprite => _cardMainSprite;

        [Header("Stats")]
        [SerializeField] private int _attackPoints;
        [SerializeField] private int _defencePoints;
        [Header("View")]
        [SerializeField] private string _cardName;
        [SerializeField] private Sprite _cardMainSprite;
    }
}