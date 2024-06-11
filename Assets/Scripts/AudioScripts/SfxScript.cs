using System;
using UnityEngine;

namespace AudioScripts
{
    public class SfxScript : MonoBehaviour
    {
        public static SfxScript instance;
        private AudioSource _audioSource;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySFXOneShot(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}