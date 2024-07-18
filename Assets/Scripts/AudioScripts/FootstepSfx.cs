using PlayerScripts;
using UnityEngine;

namespace AudioScripts
{
    /// <summary>
    /// Notice: Change the footstep audio file
    /// </summary>
    public class FootstepSfx : MonoBehaviour
    {
        private AudioSource _audioSource;
        private PlayerMovement _playerMotor;

        private void Start()
        {
            _playerMotor = GameManager.Instance.
                FetchPlayer().GetComponent<PlayerMovement>();
            
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// TO BE MODIFIED
        /// </summary>
        private void Update()
        {
            if (_playerMotor.IsWalking())
                _audioSource.enabled = _playerMotor.IsWalking();
        }
    }
}