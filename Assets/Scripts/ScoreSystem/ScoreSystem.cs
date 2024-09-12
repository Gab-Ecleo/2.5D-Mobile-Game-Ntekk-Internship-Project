using System;
using UnityEngine;
using TMPro;
using ScriptableData;

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
    }

    private void OnDisable()
    {
        GameEvents.ON_SCORE_CHANGES -= PointSystem;
    }
    #endregion

    private void Start()
    {
        GameManager.Instance.FetchCurrentPlayerStat();
    }

    public void PointSystem(int pointsToAdd)
    {
        GameManager.Instance.FetchCurrentPlayerStat();

        _playerScore.PointsToAdd = pointsToAdd;
        if (_playerCurrStats.stats.hasMultiplier)
        {
            int totalPoints = pointsToAdd * _playerScore.Multiplier;
            _playerScore.Points += totalPoints;
            GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(totalPoints);
        }
        else
        {
            _playerScore.Points += pointsToAdd;
            GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(pointsToAdd);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (uiText != null)
        {
            uiText.text = _playerScore.Points.ToString("D5");
        }
        else
        {
            Debug.LogWarning("UI Text component is not assigned.");
        }
    }

    // Resets the score to zero
    public void ResetScore()
    {
        _playerScore.Points = 0;
        UpdateUI(); // Optionally update UI after resetting
    }
}
