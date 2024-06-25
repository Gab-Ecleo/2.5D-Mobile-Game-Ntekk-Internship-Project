using System;
using System.Collections;
using EventScripts;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

namespace PlayerScripts
{
    public class PlayerBehavior : MonoBehaviour
    {
        //player's local stats
        [SerializeField] private PlayerStatsSO initialPlayerStats;
        [SerializeField] private PlayerStatsSO CurrentPlayerStats;
        [SerializeField] private bool _barrierActive = true;
        [SerializeField] private GameObject _deathScreen;

        private void Start()
        {
            //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
            CurrentPlayerStats._barrierDurability = initialPlayerStats._barrierDurability;
            CurrentPlayerStats._barrierDuration = initialPlayerStats._barrierDuration;
        }
        
        private void Update()
        {
            if (_barrierActive)
            {
                StartCoroutine("BarrierTimer");
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
                    Debug.Log($"Barrier Hits Left: {CurrentPlayerStats._barrierDurability}");
                    break;
            }
        }
    
        IEnumerator BarrierTimer()
        {
            yield return new WaitForSeconds(CurrentPlayerStats._barrierDuration);
            CurrentPlayerStats._barrierDurability = 0;
            _barrierActive = false;
            Debug.Log($"Barrier Depleted! Hit Count: {CurrentPlayerStats._barrierDurability}");
        }

        private void PlayerDeath()
        {
            //add death behavior
            Debug.Log("Player Dead");
            _deathScreen.SetActive(true);
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