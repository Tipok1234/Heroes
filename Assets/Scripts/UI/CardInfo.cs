using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Models;

namespace Assets.Scripts.UI
{
    public class CardInfo : MonoBehaviour
    {
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
            HideMana(false);
            _hideObj.SetActive(true);
            _manaCost.text = "";
        }
        public void ShowCardInfo()
        {
            _hideObj.SetActive(false);

            _logo.sprite = _cardController.Card.Logo;
            _logo.preserveAspect = true;
            _name.text = _cardController.Card.Name;

            if(_cardController.Card.IsSpell)
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
            _attack.text = "<color=#C10000>" + _cardController.Card.Attack.ToString();
            _defence.text = "<color=#00FF00>" + _cardController.Card.Defence.ToString();
            _manaCost.text = "<color=#FFFFFF>" + _cardController.Card.Manacost.ToString();
        }

        public void HightLightCard(bool hightlight)
        {
            _hightlightedObj.SetActive(hightlight);
        }

        public void HightLightManaAvaliability(int currentMana)
        {
            GetComponent<CanvasGroup>().alpha = currentMana >= _cardController.Card.Manacost ? 1 : 0.5f;
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
