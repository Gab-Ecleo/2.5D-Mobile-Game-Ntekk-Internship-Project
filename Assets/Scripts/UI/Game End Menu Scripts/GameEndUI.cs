using System;
using System.Globalization;
using ScriptableData;
using TMPro;
using UnityEngine;

namespace UI.Game_End_Menu_Scripts
{
    public class GameEndUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject GameEndUIObj;
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private TextMeshProUGUI CurrencyText;

        [Header("Scriptable Data")]
        [SerializeField] private ScoresSO scoreData;
        [SerializeField] private CurrencySO currencyData;

        private void TriggerEndUI()
        {
            ScoreText.text = scoreData.Points.ToString();
            CurrencyText.text = currencyData.matchCoins.ToString(CultureInfo.InvariantCulture);
            GameEndUIObj.SetActive(true);
        }

        private void OnEnable()
        {
            GameEvents.TRIGGER_GAMEEND_SCREEN += TriggerEndUI;
        }

        private void OnDisable()
        {
            GameEvents.TRIGGER_GAMEEND_SCREEN -= TriggerEndUI;
        }
    }
}