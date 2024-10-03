using System;
using AudioScripts.AudioSettings;
using EventScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
        [SerializeField] private AudioClipsSO _audioClips;

        private void Awake()
        {
            AudioEvents.ON_PLAYER_DEATH += PlayDeathBGM;
            AudioEvents.RANDOMIZE_AUDIO += LoadClip;
        }

        private void OnDestroy()
        {
            AudioEvents.ON_PLAYER_DEATH -= PlayDeathBGM;
            AudioEvents.RANDOMIZE_AUDIO -= LoadClip;
        }

        private void Start()
        {
            _audioClips = AudioManager.Instance.FetchAudioClips();
            
            if(SceneManager.GetActiveScene().buildIndex == 0)
                PlayBGM(_audioClips.MenuBGM);
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

        private void LoadClip(int clipIndex)
        {
            StopBGM();
            PlayBGM(BGM_Randomizer(clipIndex));
        }

        private void PlayDeathBGM()
        {
            StopBGM();
            PlayBGM(_audioClips.DeathBGM);
        }

        /// <summary>
        /// Returns randomized Audio clip
        /// </summary>
        private AudioClip BGM_Randomizer(int clipIndex)
        {
            AudioClip clip = null;

            switch (clipIndex)
            {
                case 0:
                    clip = _audioClips.FirstLevelBGM;
                    break;
                case 1:
                    clip = _audioClips.SecondLevelBGM;
                    break;
            }
            
            return clip;
        }
        
    }
}