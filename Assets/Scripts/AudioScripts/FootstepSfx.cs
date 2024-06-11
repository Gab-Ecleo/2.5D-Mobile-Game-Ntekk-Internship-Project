using PlayerScripts;
using UnityEngine;

namespace AudioScripts
{
    public class FootstepSfx : MonoBehaviour
    {
        private AudioSource _audioSource;
        private PlayerMovement _playerMotor;

        private void Start()
        {
            _playerMotor = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// TO BE MODIFIED
        /// </summary>
        private void Update()
        {
            if (_audioSource.enabled != _playerMotor.IsWalking)
            {
                _audioSource.enabled = _playerMotor.IsWalking;
            }
        }
    }
}