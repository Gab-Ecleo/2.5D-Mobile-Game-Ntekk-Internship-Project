using System;
using EventScripts;
using ScriptableData;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AudioScripts.AudioSettings
{
    [Serializable]
    public enum AudioType
    {
        BGM,Sfx
    }
    
    public struct AudioSettingPayload
    {
        public AudioType AudioSettingType { get; private set; }
        public float Volume { get; private set; }
        
        public AudioSettingPayload(AudioType audioSettingType, float volume)
        {
            AudioSettingType = audioSettingType;
            Volume = volume;
        }
    }
    public class AudioUIManager : MonoBehaviour
    {
        private AudioSettingsSO _audioData;

        [Header("UI Sliders")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("UI Texts")]
        [SerializeField] private TextMeshProUGUI bgmTxt;
        [SerializeField] private TextMeshProUGUI sfxTxt;
        
        private void Start()
        {
            _audioData = Resources.Load("AudioSettingData/Audio Settings Data") as AudioSettingsSO;
            bgmSlider.value = _audioData.bgmVolume;
            sfxSlider.value = _audioData.sfxVolume;
        }

        private void OnsliderValueChanged(AudioType type, float value)
        {
            var valToText = (value * 100).ToString("0");
            AudioSettingPayload payload = new AudioSettingPayload(type, value);

            switch (type)
            {
                case AudioType.BGM:
                    bgmTxt.text = valToText;
                    AudioEvents.OnValueChanged(payload);
                    break;
                case AudioType.Sfx:
                    sfxTxt.text = valToText;
                    AudioEvents.OnValueChanged(payload);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void OnEnable()
        {
            bgmSlider.onValueChanged.AddListener(value =>OnsliderValueChanged(AudioType.BGM,value));
            sfxSlider.onValueChanged.AddListener(value =>OnsliderValueChanged(AudioType.Sfx,value));
        }

        private void OnDisable()
        {
            bgmSlider.onValueChanged.RemoveListener(value =>OnsliderValueChanged(AudioType.BGM,value));
            sfxSlider.onValueChanged.RemoveListener(value =>OnsliderValueChanged(AudioType.Sfx,value));
        }
    }
}