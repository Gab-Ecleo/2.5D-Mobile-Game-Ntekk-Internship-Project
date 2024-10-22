﻿using EventScripts;
using UnityEngine;

namespace PowerUp.Managers.Inheritors
{
    public class SpringManager : PowerUpManager
    {
        [Header("Spring Power-Up")]
        [SerializeField] private float jumpBoost = 3;
        private float _initialJumpHeight;

        protected override void Start()
        {
            base.Start();

            _initialJumpHeight = currentPlayerStats.stats.jumpHeight;
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
            GameManager.Instance.FetchPowerUps().springJump = true;
            currentPlayerStats.stats.jumpHeight += jumpBoost;

            base.ActivatePowerUp();
        }

        protected override void DeactivatePowerUp()
        {
            GameManager.Instance.FetchPowerUps().springJump = false;
            currentPlayerStats.stats.jumpHeight = _initialJumpHeight;
            
            base.DeactivatePowerUp();
        }

        private void OnEnable()
        {
            PowerUpsEvents.ACTIVATE_SPRING_PU += ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_SPRING_PU += DeactivatePowerUp;
        }

        private void OnDisable()
        {
            PowerUpsEvents.ACTIVATE_SPRING_PU -= ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_SPRING_PU -= DeactivatePowerUp;
        }
    }
}