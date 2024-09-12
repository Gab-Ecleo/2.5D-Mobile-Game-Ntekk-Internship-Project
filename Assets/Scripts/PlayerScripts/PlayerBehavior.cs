using System;
using System.Collections;
using AudioScripts;
using AudioScripts.AudioSettings;
using EventScripts;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Input = UnityEngine.Windows.Input;

namespace PlayerScripts
{
    public class PlayerBehavior : MonoBehaviour
    {
        //player's local stats
        [SerializeField] private PlayerStatsSO initialPlayerStats;
        [SerializeField] private PlayerStatsSO currentPlayerStats;
        //[SerializeField] private GameObject deathScreen;

        [SerializeField] private Vector3 _playerPos;
        
        private AudioClipsSO _audioClip;
        private AudioManager _audioManager;

        private void Start()
        {
            LocalStorageEvents.OnLoadPlayerStats?.Invoke();
            // get starting positon
            GetPlayerPosition();

            //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
            currentPlayerStats.stats = initialPlayerStats.stats;
            Time.timeScale = 1;
        }
        
        #region BARRIER_BEHAVIOR
            //triggered when player takes damage
            private void OnDamage()
            {
                CheckBarrier();
                // Plays SFX correlating to the action
                AudioEvents.ON_PLAYER_HIT?.Invoke();
            }

            private void CheckBarrier()
            {
                // Check if player has barrier
                // IF player has no barrier, triggers death when hit
                // if (currentPlayerStats.stats.barrierDurability < 1)
                // {
                //     PlayerDeath();
                //     return;
                // }
                
                switch (currentPlayerStats.stats.barrierDurability)
                {
                    //if player has no shield, trigger death
                    case <= 0:
                        PlayerDeath();
                        break;
                    
                    //if player has shield, reduce shield by 1
                    case > 0:
                        --currentPlayerStats.stats.barrierDurability;
                        PlayerEvents.ON_BARRIER_HIT?.Invoke();
                        //Debug.Log($"Barrier Hits Left: {currentPlayerStats.stats.barrierDurability}");
                        break;
                }
            }
        #endregion

        #region player reset pos
        private void GetPlayerPosition()
        {
            _playerPos = this.gameObject.transform.position;
            currentPlayerStats.stats.StartingPos = _playerPos;
        }

        private void ResetPlayerPosition()
        {
            this.gameObject.transform.position = currentPlayerStats.stats.StartingPos;
        }
        #endregion 

        private void PlayerDeath()
        {
            //add death behavior
            AudioEvents.ON_PLAYER_DEATH?.Invoke();
            Debug.Log("Player Dead");
            GameEvents.TRIGGER_GAMEEND_SCREEN?.Invoke(true);
            GameEvents.IS_GAME_OVER?.Invoke(true);
            LocalStorageEvents.OnSaveCurrencyData?.Invoke();
            Time.timeScale = 0;
        }

        private void OnEnable()
        {
            PlayerEvents.OnPlayerDamage += OnDamage;
            PlayerEvents.OnPlayerPositionReset += ResetPlayerPosition;
        }

        private void OnDisable()
        {
            PlayerEvents.OnPlayerDamage -= OnDamage;
            PlayerEvents.OnPlayerPositionReset -= ResetPlayerPosition;
        }
    }
}