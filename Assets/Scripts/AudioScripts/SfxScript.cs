using System;
using UnityEngine;

namespace AudioScripts
{
    /// <summary>
    /// Call the SfxScript instance, then use PlaySFXOneShot(AudioClip clip) to play a one shot sound effect anywhere.
    /// </summary>
    public class SfxScript : MonoBehaviour
    {
        public static SfxScript Instance;
        private AudioSource _audioSource;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySFXOneShot(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        public void StopSFX()
        {
            _audioSource.Stop();
        }
    }
}