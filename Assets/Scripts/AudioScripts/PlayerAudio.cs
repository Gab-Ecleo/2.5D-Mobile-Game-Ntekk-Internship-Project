using AudioScripts.AudioSettings;
using EventScripts;
using PlayerScripts;
using UnityEngine;

namespace AudioScripts
{
    /// <summary>
    /// Player Related Audio queues
    /// </summary>
    public class PlayerAudio : MonoBehaviour
    {
        [Header("Audio Sources")]
        private AudioSource _footstepSource;
        
        [Header("Object References")]
        private PlayerMovement _playerMotor;
        private AudioClipsSO _audioClipSO;

        private SfxScript _sfxScript;


        #region UNITY METHODS

        private void Awake()
        {
            AudioEvents.ON_PLAYER_JUMP += PlayerJump;
            AudioEvents.ON_PLAYER_HIT += PlayerHit;
            AudioEvents.ON_PLAYER_PICKUP += PickUpBlock;
            AudioEvents.ON_PLAYER_DROP += DropBlock;
        }

        private void OnDestroy()
        {
            AudioEvents.ON_PLAYER_JUMP -= PlayerJump;
            AudioEvents.ON_PLAYER_HIT -= PlayerHit;
            AudioEvents.ON_PLAYER_PICKUP -= PickUpBlock;
            AudioEvents.ON_PLAYER_DROP -= DropBlock;
        }

        private void Start()
        {
            _sfxScript = SfxScript.Instance;
            _footstepSource = GetComponent<AudioSource>();
            _audioClipSO = AudioManager.Instance.FetchAudioClips();
            _playerMotor = GameManager.Instance.FetchPlayer()
                .GetComponent<PlayerMovement>();

            InitializeAudioClips();
        }
        private void Update()
        { 
            PlayerFootsteps();
        }

        #endregion
        
        //Dependency Initialization
        private void InitializeAudioClips()
        {
            _footstepSource.clip = _audioClipSO.FootstepSFX;
        }

        #region AUDIO QUEUES

        //Continuously play footstep unless player stops moving
        private void PlayerFootsteps()
        {
            if (_playerMotor.IsWalking())
            {
                if (_footstepSource.isPlaying) return;
                _footstepSource.loop = true;
                _footstepSource.Play();
            }
            else
            {
                _footstepSource.loop = false;
            }
        }

        private void PlayerJump()
        {
            _sfxScript.PlaySFXOneShot(_audioClipSO.JumpSFX);
        }

        private void PickUpBlock()
        {
            _sfxScript.PlaySFXOneShot(_audioClipSO.PickupSFX);
        }

        private void DropBlock()
        {
            _sfxScript.PlaySFXOneShot(_audioClipSO.DropSFX);
        }

        private void PlayerHit()
        {
            _sfxScript.PlaySFXOneShot(_audioClipSO.DamageSFX);
        }

        #endregion

    }
}