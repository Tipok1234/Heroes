using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _gameCanvas;
        [SerializeField] private Canvas _optionCanvas;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(PlayGame);
            _optionButton.onClick.AddListener(Setting);
            _exitButton.onClick.AddListener(ExitGame);
        }
        public void PlayGame()
        {
            _gameCanvas.enabled = !_gameCanvas.enabled;
            GameManager.instance.StartGame(); 
        }

        public void Setting()
        {
            _optionCanvas.enabled = !_optionCanvas.enabled;
        }

        public void ExitGame()
        {
            Application.Quit();
            Debug.LogError("GAME OVER!");
        }
    }
}
