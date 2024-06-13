﻿using System;
using EventScripts;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace AudioScripts.AudioSettings
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        private AudioSettingsSO _audioData;
        [SerializeField] private AudioSource[] bgmSources;
        [SerializeField] private AudioSource[] sfxSources;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            //initialize values from the Audio Setting Data
            _audioData = Resources.Load("AudioSettingData/Audio Settings Data") as AudioSettingsSO;
            if (_audioData == null) return;
            UpdateBGMVolume(_audioData.bgmVolume);
            UpdateSfxVolume(_audioData.sfxVolume);
        }

        //updates the values of all the audio sources in the list
        private void UpdateBGMVolume(float volume)
        {
            foreach (var bgmSource in bgmSources)
            {
                bgmSource.volume = volume;
            }
        }

        //updates the values of all the audio sources in the list
        private void UpdateSfxVolume(float volume)
        {
            foreach (var sfxSource in sfxSources)
            {
                sfxSource.volume = volume;
            }
        }

        //fetched by the event that is called by the Audio UI update
        private void UpdateVolume(object rawPayload)
        {
            //if the object from the parameter is not the AudioSettingPayload struct, return. Else, update the Audio sources and data.
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
                    UpdateSfxVolume(settingsPayload.Volume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //Adds/removes listener to the UpdateVolume function
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