using UnityEngine;

namespace AudioScripts
{
    /// <summary>
    /// Call the BgmScript instance, then use PlayBGM(AudioClip clip) to play a BGM.
    /// Use StopBGM() and PauseBGM() to manipulate the BGM's state.
    /// </summary>
    public class BgmScript : MonoBehaviour
    {
        public static BgmScript Instance;
        private AudioSource _audioSource;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayBGM(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void StopBGM()
        {
            _audioSource.Stop();
        }

        public void PauseBGM()
        {
            _audioSource.Pause();
        }
    }
}