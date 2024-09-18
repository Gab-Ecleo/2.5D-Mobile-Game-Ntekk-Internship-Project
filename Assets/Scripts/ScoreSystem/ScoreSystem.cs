using System;
using UnityEngine;
using TMPro;
using ScriptableData;

public class ScoreSystem : MonoBehaviour
{
    public TMP_Text uiText;
    public ScoresSO _playerScore;

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
        GameManager.Instance.FetchPowerUps();
        ResetScore();
    }


    public void PointSystem(int pointsToAdd)
    {
        GameManager.Instance.FetchPowerUps();

        _playerScore.PointsToAdd = pointsToAdd;
        if (GameManager.Instance.FetchPowerUps().hasMultiplier)
        {
            var multiplier = _playerScore.PointsToAdd * _playerScore.Multiplier;
            _playerScore.Points += multiplier;
            GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(multiplier);
        }
        else
        {
            _playerScore.Points += _playerScore.PointsToAdd;
            GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(_playerScore.PointsToAdd);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        uiText.text = _playerScore.Points.ToString("D5");
    }

    public void ResetScore()
    {
        _playerScore.Points = 0;
        UpdateUI();
    }
}
