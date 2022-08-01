using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class OptionMenu : MonoBehaviour
    {       
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Canvas _canvasOptionMenu;
        [SerializeField] private Button _openOptionButton;
        [SerializeField] private Button _backOptionButton;

        private void Awake()
        {
            _openOptionButton.onClick.AddListener(OpenOptionMenu);
            _backOptionButton.onClick.AddListener(OpenOptionMenu);
        }
        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", Mathf.Log10(volume) * 50);
        }

        public void ClickSound()
        {
            AudioListener.pause = !AudioListener.pause;
        }
        private void OpenOptionMenu()
        {
            _canvasOptionMenu.enabled = !_canvasOptionMenu.enabled;  
        }
    }
}
