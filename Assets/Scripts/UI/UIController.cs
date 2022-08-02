using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Managers;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        public GameObject TempCardGO => _tempCardGo;
        public GameObject GameCanvas => _gameCanvas;

        public static UIController instance;

        [SerializeField] private TMP_Text _playerMana;
        [SerializeField] private TMP_Text _enemyMana;
        [SerializeField] private TMP_Text _playerHP;
        [SerializeField] private TMP_Text _enemyHP;
        [SerializeField] private TMP_Text _playerDeckCardsCount;
        [SerializeField] private TMP_Text _enemyDeckCardsCount;

        [SerializeField] private GameObject _resultGO;
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private TMP_Text _turnTime;
        [SerializeField] private Button _endTurnButton;

        [SerializeField] private GameObject _tempCardGo;
        [SerializeField] private GameObject _gameCanvas;

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
            _playerMana.text = "<color=#00F5FF>" + GameManager.instance.CurrentGame.Player.Mana.ToString();
            _enemyMana.text = "<color=#00F5FF>" + GameManager.instance.CurrentGame.Enemy.Mana.ToString();
            _playerHP.text = "<color=#00FF00>" + GameManager.instance.CurrentGame.Player.HP.ToString();
            _enemyHP.text = "<color=#00FF00>" + GameManager.instance.CurrentGame.Enemy.HP.ToString();
        }

        public void ShowResult()
        {
            _resultGO.SetActive(true);

            if (GameManager.instance.CurrentGame.Enemy.HP == 0)
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
}