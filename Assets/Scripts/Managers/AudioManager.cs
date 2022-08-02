using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance => _instanceAudio;

        [SerializeField] private AudioSource _backGroundAudioSource;
        [SerializeField] private AudioSource _attackAudioSource;
        [SerializeField] private AudioSource _distributionAudioSource;
        [SerializeField] private AudioSource _healAudioSource;
        [SerializeField] private AudioSource _buffAudioSource;

        private static AudioManager _instanceAudio;

        private void Awake()
        {
            if (_instanceAudio == null)
                _instanceAudio = this;

            _backGroundAudioSource.Play();
        }

        public void VoiceAttack()
        {
            _attackAudioSource.Play();
        }

        public void DistributionCard()
        {
            _distributionAudioSource.Play();
        }

        public void HealAudio()
        {
            _healAudioSource.Play();
        }

        public void BuffAudio()
        {
            _buffAudioSource.Play();
        }
    }
}
