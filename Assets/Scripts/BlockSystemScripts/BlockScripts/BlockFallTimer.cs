using System;
using Unity.VisualScripting;
using UnityEngine;

namespace BlockSystemScripts.BlockScripts
{
    /// <summary>
    /// Timer for block falling
    /// </summary>
    public class BlockFallTimer : MonoBehaviour
    {
        [Header("Script References")]
        [SerializeField] private BlockScript blockScript;
        
        [Header("Timer Data")] 
        [SerializeField] private float initialTimer = 0.5f;
        private bool _isTimerActivated;
        
        [Header("Active Timers. To be private")]
        [SerializeField] private float timeLeft;

        private void Awake()
        {
            timeLeft = initialTimer;
        }

        private void Update()
        {
            UpdateTimer();
        }

        #region TIMER_METHODS
        private void UpdateTimer()
        {
            //If timer is not activated, ignore everything els
            if (!_isTimerActivated) return;
            //If timer is more than zero, reduce timer. 

            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            //If timer is equal to or less than zero, deactivate the timer, and move down the block
            else
            {
                StopTimer();
                blockScript.TransferCell();
            }
        }
        
        //Method to activate the timer
        public void StartTimer()
        {
            timeLeft = initialTimer;
            _isTimerActivated = true;
        }
        
        //method to deactivate the timer
        public void StopTimer()
        {
            _isTimerActivated = false;
        }

        #endregion
    }
}