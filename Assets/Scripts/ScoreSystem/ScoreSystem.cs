using System;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using ScriptableData;
using System.Diagnostics;

public class ScoreSystem : MonoBehaviour
{
    public TMP_Text uiText;

    public ScoresSO _playerScore;
    public PlayerStatsSO _playerCurrStats;

    private bool _hasMultiplier;
    #region Action
    private void OnEnable()
    {
        GameEvents.ON_SCORE_CHANGES += PointSystem;
        GameEvents.ON_UI_CHANGES += UpdateUI;
    }

    private void OnDisable()
    {
        GameEvents.ON_SCORE_CHANGES -= PointSystem;
        GameEvents.ON_UI_CHANGES -= UpdateUI;
    }
    #endregion

    private void Start()
    {
        ResetSore();
    }

    // funcation being called by the action to update ui and add score
    public void PointSystem()
    {
        if (_playerCurrStats.stats.hasMultiplier)
        {
            _playerScore.Points *= _playerScore.Multiplier;
            GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(_playerScore.PointsToAdd * _playerScore.Multiplier);
            GameEvents.ON_UI_CHANGES?.Invoke();
        }
        else
        {
            _playerScore.Points += _playerScore.PointsToAdd;
            GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(_playerScore.PointsToAdd);
            GameEvents.ON_UI_CHANGES?.Invoke();
        }
    }

    // will change later if hud is finalized
    public void UpdateUI()
    {
        uiText.text = _playerScore.Points.ToString("D5");
    }

    public void ResetSore()
    {
        _playerScore.Points = 0;
    }
}
