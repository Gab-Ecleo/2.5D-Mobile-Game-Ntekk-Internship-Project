using System;
using System.Collections;
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
        [SerializeField] private GameObject deathScreen;

        private void Start()
        {
            //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
            currentPlayerStats.barrierDurability = initialPlayerStats.barrierDurability;
        }
        
        #region BARRIER_BEHAVIOR
            //triggered when player takes damage
            private void OnDamage()
            {
                CheckBarrier();
            }

            private void CheckBarrier()
            {
                // Check if player has barrier
                // IF player has no barrier, triggers death when hit
                if (currentPlayerStats.barrierUpgrade < 1)
                {
                    PlayerDeath();
                    return;
                }
                
                switch (currentPlayerStats.barrierDurability)
                {
                    //if player has no shield, trigger death
                    case <= 0:
                        PlayerDeath();
                        break;
                    
                    //if player has shield, reduce shield by 1
                    case > 0:
                        --currentPlayerStats.barrierDurability;
                        Debug.Log($"Barrier Hits Left: {currentPlayerStats.barrierDurability}");
                        break;
                }
            }
        #endregion
        
        private void PlayerDeath()
        {
            //add death behavior
            Debug.Log("Player Dead");
            deathScreen.SetActive(true);
            Time.timeScale = 0;
        }

        private void OnEnable()
        {
            PlayerEvents.OnPlayerDamage += OnDamage;
        }

        private void OnDisable()
        {
            PlayerEvents.OnPlayerDamage -= OnDamage;
        }
    }
}