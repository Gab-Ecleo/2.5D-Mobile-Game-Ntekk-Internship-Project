using System;
using ScriptableData;
using TMPro;
using UnityEngine;

namespace Match_Timers
{
    public class MatchTimer : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private TextMeshProUGUI timerTxt;
        
        [SerializeField] private float seconds = 0;
        [SerializeField] private float minutes = 0;
        [SerializeField] private float hours = 0;

        private GameStateSO _gameState;
        private PowerUpsSO _powerUps;

        private void Start()
        {
            _powerUps = GameManager.Instance.FetchPowerUps();
            _gameState = GameManager.Instance.FetchGameStateData();

            seconds = 0;
            minutes = 0;
            hours = 0;
        }

        private void Update()
        {
            if (_gameState.isPaused) return;
            UpdateTimer();
            UpdateUI();
        }

        #region MATCH_TIMER
        private void UpdateTimer()
        {
            if (!_powerUps.timeSlow)
            { 
                seconds += Time.deltaTime;
            }
            else
            {
                seconds += Time.deltaTime * 0.5f;
            }
            
            ValidateMinute();
            ValidateHours();
        }
        
        private void ValidateMinute()
        {
            if (!(seconds >= 60f)) return;
            minutes += 1;
            seconds = 0;
        }

        private void ValidateHours()
        {
            if (!(minutes >= 60f)) return;
            hours += 1;
            minutes = 0;
            seconds = 0;
        }

        public void ResetTimer()
        {
            hours = 0;
            minutes = 0;
            seconds = 0;
        }
        #endregion

        #region UI_Update
        private void UpdateUI()
        {
            timerTxt.text = hours <= 0 ? $"{minutes:00} : {seconds:00}" : $"{hours:00} : {minutes:00} : {seconds:00}";
        }
        #endregion
    }
}