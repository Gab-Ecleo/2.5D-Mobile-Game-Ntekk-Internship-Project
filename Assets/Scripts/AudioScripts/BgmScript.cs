using System;
using AudioScripts.AudioSettings;
using EventScripts;
using UnityEditor;
using UnityEngine;

namespace AudioScripts
{
    /// <summary>
    /// Call the BgmScript instance, then use PlayBGM(AudioClip clip) to play a BGM.
    /// Use StopBGM() and PauseBGM() to manipulate the BGM's state.
    /// </summary>
    public class BgmScript : MonoBehaviour
    {
        [Header("AudioSources")]
        [SerializeField] private AudioSource _audioSource;

        private AudioClipsSO _audioClips;


        private void Awake()
        {
            AudioEvents.ON_PLAYER_DEATH += PlayDeathBGM;
        }

        private void OnDestroy()
        {
            AudioEvents.ON_PLAYER_DEATH -= PlayDeathBGM;
        }

        private void Start()
        {
            _audioClips = AudioManager.Instance.FetchAudioClips();

            PlayGameBGM();
        }

        #region Audio Controls

        private void PlayBGM(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        private void StopBGM()
        {
            _audioSource.Stop();
        }

        private void PauseBGM()
        {
            _audioSource.Pause();
        }

        #endregion

        private void PlayGameBGM()
        {
            StopBGM();
            PlayBGM(_audioClips.FirstLevelBGM);
        }

        private void PlayDeathBGM()
        {
            StopBGM();
            PlayBGM(_audioClips.DeathBGM);
        }
        
    }
}