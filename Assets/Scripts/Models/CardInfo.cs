using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Models
{
    public class CardInfo : MonoBehaviour
    {
        // public GameObject ManaObj => _manaObj;

        [SerializeField] private CardController _cardController;
        [SerializeField] private Image _cardImage;
        [SerializeField] private Image _logo;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _attack;
        [SerializeField] private TMP_Text _defence;
        [SerializeField] private TMP_Text _manaCost;
        [SerializeField] private GameObject _hideObj;
        [SerializeField] private GameObject _hightlightedObj;
        [SerializeField] private GameObject _manaObj;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _targetColor;
        [SerializeField] private Color _spellTargetColor;

        public void HideCardInfo()
        {
            _hideObj.SetActive(true);
            _manaCost.text = "";
        }
        public void ShowCardInfo()
        {
            _hideObj.SetActive(false);

            _logo.sprite = _cardController._card.Logo;
            _logo.preserveAspect = true;
            _name.text = _cardController._card.Name;

            if(_cardController._card.isSpell)
            {
                _attack.gameObject.SetActive(false);
                _defence.gameObject.SetActive(false);
            }

            RefreshData();
        }

        public void HideMana(bool manaShow)
        {
            _manaObj.SetActive(manaShow);
        }

        public void RefreshData()
        {
            _attack.text = "<color=#C10000>" + _cardController._card.Attack.ToString();
            _defence.text = "<color=#00FF00>" + _cardController._card.Defence.ToString();
            _manaCost.text = "<color=#FFFFFF>" + _cardController._card.Manacost.ToString();
        }

        public void HightLightCard(bool hightlight)
        {
            _hightlightedObj.SetActive(hightlight);
        }

        public void HightLightManaAvaliability(int currentMana)
        {
            GetComponent<CanvasGroup>().alpha = currentMana >= _cardController._card.Manacost ? 1 : 0.5f;
        }

        public void HightLightTarget(bool hightLight)
        {
            _cardImage.color = hightLight ? _targetColor : _normalColor;
        }
        public void HightLightAsSpellTarget(bool hightLight)
        {
            _cardImage.color = hightLight ? _spellTargetColor : _normalColor;
        }

    }
}
