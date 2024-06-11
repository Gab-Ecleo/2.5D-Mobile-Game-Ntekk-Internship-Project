using UnityEngine;

namespace AudioScripts
{
    public class BgmScript : MonoBehaviour
    {
        public static BgmScript instance;
        private AudioSource _audioSource;
        
        // [SerializeField] private AudioClip introBgmClip;

        private void Awake()
        {
            instance = this;
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