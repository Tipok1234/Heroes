using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Player
    {
        public int Mana => _mana;
        public int HP => _hp;

        private int _hp;
        private int _mana;
        private int _manaPool;

        private const int _max_manaPool = 10;

        public Player()
        {
            _hp = 30;
            _mana = _manaPool = 2;
        }

        public void RestoreRoundMana()
        {
            _mana = _manaPool;
        }

        public void InCreaseManapool()
        {
            _manaPool = Mathf.Clamp(_manaPool + 1, 0, _max_manaPool);
        }

        public void GetDamage(int damage)
        {
            _hp = Mathf.Clamp(_hp - damage, 0, int.MaxValue);
        }

        public void SetMana(int value)
        {
            _mana = value;
        }

        public void AddHPValue(int value)
        {
            _hp += value;
        }
    }
}
