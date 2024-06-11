using System;
using Events;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerBehavior : MonoBehaviour
    {
        //player's local stats
        private int _shieldCount;

        private void Start()
        {
            //initialize local stats from player data scriptable
            var initialPlayerStats = Resources.Load("PlayerData/PlayerStats") as PlayerStatsSO;
            if (initialPlayerStats != null) _shieldCount = initialPlayerStats.shield;
        }

        //triggered when player takes damage
        private void OnDamage()
        {
            switch (_shieldCount)
            {
                //if player has no shield, trigger death
                case <= 0:
                    PlayerDeath();
                    break;
                
                //if player has shield, reduce shield by 1
                case > 0:
                    --_shieldCount;
                    Debug.Log($"Shield Count: {_shieldCount}");
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