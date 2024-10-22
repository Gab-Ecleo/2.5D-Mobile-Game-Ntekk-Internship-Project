﻿using EventScripts;
using ScriptableData;
using UnityEngine;

namespace PowerUp.Managers.Inheritors
{
    
    public class TimeSlowManager : PowerUpManager
    {
        [Header("Time Slow Power-up")]
        [SerializeField] private BlockTimerSO blockTimerSo;
        
        protected override void Start()
        {
            blockTimerSo.ResetState();
            base.Start();
        }

        private void Update()
        {
            UpdatePowerUpDuration();
        }

        protected override void UpdatePowerUpDuration()
        {
            //checks if the power up has a duration
            if (!shouldHaveDuration) return;
            //checks if the timer is active
            if (!IsTimerActive) return;
            
            base.UpdatePowerUpDuration();
            
            if (currentDuration > 0) return;
            DeactivatePowerUp();
        }

        protected override void ActivatePowerUp()
        {
            GameManager.Instance.FetchPowerUps().timeSlow = true;
            
            blockTimerSo.blockTimerState = BlockTimerState.Slowed;
            BlockEvents.OnSyncBlockFallTimers?.Invoke();
            
            base.ActivatePowerUp();
        }

        protected override void DeactivatePowerUp()
        {
            GameManager.Instance.FetchPowerUps().timeSlow = false;
            
            blockTimerSo.blockTimerState = BlockTimerState.Normal;
            BlockEvents.OnSyncBlockFallTimers?.Invoke();

            base.DeactivatePowerUp();
        }

        private void OnEnable()
        {
            PowerUpsEvents.ACTIVATE_TIMESLOW_PU += ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_TIMESLOW_PU += ActivatePowerUp;
        }

        private void OnDisable()
        {
            PowerUpsEvents.ACTIVATE_TIMESLOW_PU -= ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_TIMESLOW_PU -= ActivatePowerUp;
        }
    }
}