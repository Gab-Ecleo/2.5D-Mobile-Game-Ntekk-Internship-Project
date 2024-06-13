using System;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerGrabCooldown : MonoBehaviour
    {
        private float _initialCooldownTimer;
        [SerializeField] private float cooldownTimer = 3f;
        [SerializeField] private bool cooldownTimerEnabled;

        private void Start()
        {
            cooldownTimerEnabled = false;
            _initialCooldownTimer = cooldownTimer;
        }

        private void Update()
        {
            UpdateTimer();
        }

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

        public bool GrabCoolDownEnabled()
        {
            return cooldownTimerEnabled;
        }

        //enables the cooldown timer
        public void StartTimer()
        {
            cooldownTimerEnabled = true;
            cooldownTimer = _initialCooldownTimer;
        }
    }
}