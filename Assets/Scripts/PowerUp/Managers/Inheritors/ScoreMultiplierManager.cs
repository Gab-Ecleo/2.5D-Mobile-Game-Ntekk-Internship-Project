using EventScripts;
using UnityEngine;

namespace PowerUp.Managers.Inheritors
{
    public class ScoreMultiplierManager : PowerUpManager
    {
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
            Debug.Log("Activate Multiplier");
            base.ActivatePowerUp();
        }

        protected override void DeactivatePowerUp()
        {
            GameManager.Instance.FetchPowerUps().hasMultiplier = false;
            Debug.Log("Deactivate Multiplier");
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