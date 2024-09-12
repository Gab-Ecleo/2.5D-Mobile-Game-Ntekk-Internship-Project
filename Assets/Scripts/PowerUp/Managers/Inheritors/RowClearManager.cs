using EventScripts;

namespace PowerUp.Managers.Inheritors
{
    public class RowClearManager : PowerUpManager
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
            currentPlayerStats.stats.expressDelivery = true;
            base.ActivatePowerUp();
        }

        protected override void DeactivatePowerUp()
        {
            currentPlayerStats.stats.expressDelivery = false;
            base.DeactivatePowerUp();
        }

        private void OnEnable()
        {
            PowerUpsEvents.ACTIVATE_ROWCLEAR_PU += ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_ROWCLEAR_PU += DeactivatePowerUp;
        }

        private void OnDisable()
        {
            PowerUpsEvents.ACTIVATE_ROWCLEAR_PU -= ActivatePowerUp;
            PowerUpsEvents.DEACTIVATE_ROWCLEAR_PU -= DeactivatePowerUp;
        }
    }
}