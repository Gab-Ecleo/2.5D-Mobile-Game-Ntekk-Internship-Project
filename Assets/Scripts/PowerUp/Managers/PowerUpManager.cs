using ScriptableData;
using UnityEngine;

namespace PowerUp.Managers
{
    /// <summary>
    /// This is the Base Power Up Manager. Inherit this for new Power Up Types
    /// </summary>
    public class PowerUpManager : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] protected PlayerStatsSO currentPlayerStats;
        
        [Header("Power-Up Duration")] 
        [SerializeField] protected bool shouldHaveDuration;
        [SerializeField] protected float initialDuration = 10f;
        protected bool IsTimerActive;
        
        [Header("Pause Data")] 
        [SerializeField] protected PauseSO pauseSo;
        
        [Header("In-Game Stats. DO NOT MODIFY")]
        [SerializeField] protected PowerUpState currentState = PowerUpState.Inactive;
        [SerializeField] protected float currentDuration;

        protected virtual void Start()
        {
            currentDuration = 0;
            IsTimerActive = false;
            currentState = PowerUpState.Inactive;
        }
    
        //Base function on power up duration. Updated by an overload
        protected virtual void UpdatePowerUpDuration()
        {
            if (pauseSo.isPaused) return;
            //if the timer is active and current duration is not yet zero, proceed to countdown
            if (currentDuration > 0)
            {
                currentDuration -= Time.deltaTime;
            }
        }
        
        //Base function on power up activation. Triggered by an overload
        protected virtual void ActivatePowerUp()
        {
            currentState = PowerUpState.Active;
            
            //checks if the power up has a duration
            if (!shouldHaveDuration) return;
            IsTimerActive = true;
            currentDuration = initialDuration;
        }
        
        //Base function on power up deactivation. Triggered by an overload
        protected virtual void DeactivatePowerUp()
        {
            currentState = PowerUpState.Inactive;
            
            //checks if the power up has a duration
            if (!shouldHaveDuration) return;
            IsTimerActive = false;
            currentDuration = 0;
        }
    }
}