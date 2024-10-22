﻿using EventScripts;

namespace PowerUp.Managers.Inheritors
{
    public class SingleClearManager : PowerUpManager
    {
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
            GameManager.Instance.FetchPowerUps().singleBlockRemover = true;
            base.ActivatePowerUp();
        }

        protected override void DeactivatePowerUp()
        {
            GameManager.Instance.FetchPowerUps().singleBlockRemover = false;
            base.DeactivatePowerUp();
        }

        private void OnEnable()
        {
            PowerUpsEvents.ACTIVATE_SINGLECLEAR_PU += ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_SINGLECLEAR_PU += DeactivatePowerUp;
        }

        private void OnDisable()
        {
            PowerUpsEvents.ACTIVATE_SINGLECLEAR_PU -= ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_SINGLECLEAR_PU -= DeactivatePowerUp;
        }
    }
}