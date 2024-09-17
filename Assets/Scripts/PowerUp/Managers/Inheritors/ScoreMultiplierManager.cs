﻿using EventScripts;
using UnityEngine;

namespace PowerUp.Managers.Inheritors
{
    public class ScoreMultiplierManager : PowerUpManager
    {
        [Header("Score Multiplier References")]
        [SerializeField] private ScoresSO playerScore;
        [SerializeField] private int scoreMultiplier = 2;
        
        protected override void Start()
        {
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
            GameManager.Instance.FetchPowerUps().hasMultiplier = true;
            playerScore.Multiplier = scoreMultiplier;

            base.ActivatePowerUp();
        }

        protected override void DeactivatePowerUp()
        {
            GameManager.Instance.FetchPowerUps().hasMultiplier = false;
            playerScore.Multiplier = 1;

            base.DeactivatePowerUp();
        }

        private void OnEnable()
        {
            PowerUpsEvents.ACTIVATE_MULTIPLIER_PU += ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_MULTIPLIER_PU += DeactivatePowerUp;
        }

        private void OnDisable()
        {
            PowerUpsEvents.ACTIVATE_MULTIPLIER_PU -= ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_MULTIPLIER_PU -= DeactivatePowerUp;
        }
    }
}