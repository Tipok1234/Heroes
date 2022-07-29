using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Managers;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public TMP_Text _playerMana;
    public TMP_Text _enemyMana;
    public TMP_Text _playerHP;
    public TMP_Text _enemyHP;
    public TMP_Text _playerDeckCardsCount;
    public TMP_Text _enemyDeckCardsCount;

    public GameObject _resultGO;
    public TMP_Text _resultText;
    public TMP_Text _turnTime;
    public Button _endTurnButton;
    //public Button _restartButton;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        _endTurnButton.interactable = true;
        _resultGO.SetActive(false);
        UpdateHPAndMana();
    }

    public void UpdateHPAndMana()
    {
        _playerMana.text = "<color=#00F5FF>" + GameManager.instance._currentGame._player._mana.ToString();
        _enemyMana.text = "<color=#00F5FF>" + GameManager.instance._currentGame._enemy._mana.ToString();
        _playerHP.text = "<color=#00FF00>" +  GameManager.instance._currentGame._player._hp.ToString();
        _enemyHP.text = "<color=#00FF00>" + GameManager.instance._currentGame._enemy._hp.ToString();
    }

    public void ShowResult()
    {
        _resultGO.SetActive(true);

        if (GameManager.instance._currentGame._enemy._hp == 0)
            _resultText.text = "WIN SNOOP DOG";
        else
            _resultText.text = "WIN GUCCI MAIN";
    }

    public void UpdateTurnTime(int time)
    {
        _turnTime.text = "<color=#000000>" + time.ToString();
    }

    public void UpdatePlayerDeckCard(int countDeckCard)
    {
        _playerDeckCardsCount.text = "<color=#000000>" + countDeckCard.ToString();
    }

    public void UpdateEnemyDeckCard(int countDeckCard)
    {
        _enemyDeckCardsCount.text = "<color=#000000>" + countDeckCard.ToString();
    }

    public void DisableTurnButton()
    {
        _endTurnButton.interactable = GameManager.instance.IsPlayerTurn;
    }


}
