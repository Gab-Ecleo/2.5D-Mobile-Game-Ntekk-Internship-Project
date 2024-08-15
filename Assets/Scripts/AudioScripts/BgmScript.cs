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
            
            PlayBGM();
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

        private void PlayBGM()
        {
            StopBGM();
            
            //Skip if it's not in the main menu
            if(SceneManager.GetActiveScene().buildIndex == 0){  
                PlayBGM(_audioClips.MenuBGM);
                return;
            }
            
            PlayBGM(BGM_Randomizer());
        }

        private void PlayDeathBGM()
        {
            StopBGM();
            PlayBGM(_audioClips.DeathBGM);
        }

        /// <summary>
        /// Returns randomized Audio clip
        /// </summary>
        private AudioClip BGM_Randomizer()
        {
            AudioClip clip = null;
            var clipIndex = Random.Range(1, 4);
            
            switch (clipIndex)
            {
                case 1:
                    clip = _audioClips.FirstLevelBGM;
                    break;
                case 2:
                    clip = _audioClips.SecondLevelBGM;
                    break;
                case 3:
                    clip = _audioClips.ThirdLevelBGM;
                    break;
            }
            
            return clip;
        }
        
    }
}