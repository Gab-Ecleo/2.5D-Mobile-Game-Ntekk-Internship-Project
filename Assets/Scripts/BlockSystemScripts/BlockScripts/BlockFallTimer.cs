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
        [SerializeField] private BlockScript blockScript;
        
        [Header("Timer Data")] 
        [SerializeField] private float initialTimer = 0.5f;
        private bool _isTimerActivated;
        
        [Header("Active Timers")]
        [SerializeField] private float timeLeft;

        private void Start()
        {
            timeLeft = initialTimer;
        }

        private void Update()
        {
            UpdateTimer();
        }

        #region Timer
        private void UpdateTimer()
        {
            if (!_isTimerActivated) return;
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                _isTimerActivated = false;
                StopTimer();
                blockScript.TransferCell();
                // StartTimer();
            }
        }

        public void StartTimer()
        {
            timeLeft = initialTimer;
            _isTimerActivated = true;
        }

        public void StopTimer()
        {
            _isTimerActivated = false;
        }

        #endregion
        
    }
}