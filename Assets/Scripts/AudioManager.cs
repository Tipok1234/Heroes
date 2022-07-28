using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource _backGroundAudioSource;
    public AudioSource _attackAudioSource;
    public AudioSource _distributionAudioSource;
    public AudioSource _healAudioSource;
    public AudioSource _buffAudioSource;

    public static AudioManager _instanceAudio;

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
