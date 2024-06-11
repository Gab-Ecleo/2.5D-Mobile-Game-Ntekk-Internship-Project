using System;
using EventScripts;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace AudioScripts.AudioSettings
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSettingsSO _audioData;
        [SerializeField] private AudioSource[] bgmSources;
        [SerializeField] private AudioSource[] sfxSources;

        private void Start()
        {
            _audioData = Resources.Load("AudioSettingData/Audio Settings Data") as AudioSettingsSO;
            if (_audioData == null) return;
            UpdateBGMVolume(_audioData.sfxVolume);
            UpdateSfxVolume(_audioData.sfxVolume);
        }

        private void UpdateBGMVolume(float volume)
        {
            foreach (var bgmSource in bgmSources)
            {
                bgmSource.volume = volume;
            }
        }

        private void UpdateSfxVolume(float volume)
        {
            foreach (var sfxSource in sfxSources)
            {
                sfxSource.volume = volume;
            }
        }

        private void UpdateVolume(object rawPayload)
        {
            if (rawPayload is not AudioSettingPayload settingsPayload)
            {
                return;
            }
            
            switch (settingsPayload.AudioSettingType)
            {
                case AudioType.BGM:
                    _audioData.bgmVolume = settingsPayload.Volume;
                    UpdateBGMVolume(settingsPayload.Volume);
                    break;
                case AudioType.Sfx:
                    _audioData.sfxVolume = settingsPayload.Volume;
                    UpdateBGMVolume(settingsPayload.Volume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable()
        {
            AudioEvents.OnValueChanged += UpdateVolume;
        }

        private void OnDisable()
        {
            AudioEvents.OnValueChanged -= UpdateVolume;
        }
    }
}