using UnityEngine;

namespace Assets.Scripts.SO
{
    public class CardDataSO : ScriptableObject
    {
        public int ManaCostPoints => _manaCostPoints;
        public string CardName => _cardName;
        public Sprite CardMainSprite => _cardMainSprite;

        [Header("Stats")]
        [SerializeField] private int _manaCostPoints;
        [Header("View")]
        [SerializeField] private string _cardName;
        [SerializeField] private Sprite _cardMainSprite;
    }
}