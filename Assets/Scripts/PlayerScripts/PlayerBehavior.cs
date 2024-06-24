using System;
using EventScripts;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerBehavior : MonoBehaviour
    {
        //player's local stats
        [SerializeField] private PlayerStatsSO initialPlayerStats;
        [SerializeField] private PlayerStatsSO CurrentPlayerStats;

        private void Start()
        {
            //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
            CurrentPlayerStats._barrierDurability = initialPlayerStats._barrierDurability;
            CurrentPlayerStats._barrierDuration = initialPlayerStats._barrierDuration;
        }
        //Barrier Timer countdown from max to 0
        private void Update()
        {
            --CurrentPlayerStats._barrierDuration;
            if (CurrentPlayerStats._barrierDuration == 0)
            {
                CurrentPlayerStats._barrierDurability = 0;
                Debug.Log("Barrier goes bye bye! :<");
            }
        }

        //triggered when player takes damage
        private void OnDamage()
        {
            switch (CurrentPlayerStats._barrierDurability)
            {
                //if player has no shield, trigger death
                case <= 0:
                    PlayerDeath();
                    break;
                
                //if player has shield, reduce shield by 1
                case > 0:
                    --CurrentPlayerStats._barrierDurability;
                    Debug.Log($"Shield Count: {CurrentPlayerStats._barrierDurability}");
                    break;
            }
        }

        private void PlayerDeath()
        {
            //add death behavior
            Debug.Log("Player Dead");
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