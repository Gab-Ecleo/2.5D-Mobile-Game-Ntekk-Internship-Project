using System;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerScripts
{
    //script used for calculating the cooldown of the grab feature
    public class PlayerGrabCooldown : MonoBehaviour
    {
        private float _initialCooldownTimer;
        [SerializeField] private float cooldownTimer = 3f;
        [SerializeField] private bool cooldownTimerEnabled;

        private void Awake()
        {
            InitializeScriptValues();
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void InitializeScriptValues()
        {
            cooldownTimerEnabled = false;
            _initialCooldownTimer = cooldownTimer;
        }
        
        //returns a public bool that can be used to check if the grab is on cooldown or not
        public bool GrabCoolDownEnabled()
        {
            return cooldownTimerEnabled;
        }

        #region TIMER_METHODS
        private void UpdateTimer()
        {
            //if cooldown is not enabled, ignore the rest. Else, do a countdown
            if (!cooldownTimerEnabled) return;
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                cooldownTimerEnabled = false;
            }
        }

        //enables the cooldown timer
        public void StartTimer()
        {
            cooldownTimerEnabled = true;
            cooldownTimer = _initialCooldownTimer;
        }
        #endregion
    }
}