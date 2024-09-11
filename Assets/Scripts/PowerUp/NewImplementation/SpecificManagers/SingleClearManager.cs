using System;
using EventScripts;
using UnityEngine;

namespace PowerUp.NewImplementation.SpecificManagers
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
            currentPlayerStats.stats.singleBlockRemover = true;
            base.ActivatePowerUp();
        }

        protected override void DeactivatePowerUp()
        {
            currentPlayerStats.stats.singleBlockRemover = false;
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