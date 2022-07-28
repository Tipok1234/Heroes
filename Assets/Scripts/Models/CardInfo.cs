using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Models
{
    public class CardInfo : MonoBehaviour
    {
       // public GameObject ManaObj => _manaObj;
        
        public CardController _cardController;
        public Image _cardImage;
        public Image logo;
        public TMP_Text name;
        public TMP_Text attack;
        public TMP_Text defence;
        public TMP_Text manaCost;
        public GameObject _hideObj;
        public GameObject _hightlightedObj;
        public GameObject _manaObj;
        public Color _normalColor;
        public Color _targetColor;
        public Color _spellTargetColor;

        public void HideCardInfo()
        {
            _hideObj.SetActive(true);
            manaCost.text = "";
        }
        public void ShowCardInfo()
        {
            _hideObj.SetActive(false);

            logo.sprite = _cardController._card.Logo;
            logo.preserveAspect = true;
            name.text = _cardController._card.Name;

            if(_cardController._card.isSpell)
            {
                attack.gameObject.SetActive(false);
                defence.gameObject.SetActive(false);
            }

            RefreshData();
        }

        public void HideMana(bool manaShow)
        {
            _manaObj.SetActive(manaShow);
        }

        public void RefreshData()
        {
            attack.text = "<color=#C10000>" + _cardController._card.Attack.ToString();
            defence.text = "<color=#00FF00>" + _cardController._card.Defence.ToString();
            manaCost.text = "<color=#FFFFFF>" + _cardController._card.Manacost.ToString();
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
