using System;
using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class ScoreSystem : MonoBehaviour
{
    public TMP_Text uiText;

    public ScoresSO _playerScore;

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
        ResetScore();
    }

    // funcation being called by the action to update ui and add score
    public void PointSystem(int addedPoints, int multiplier, bool isPoweredUp)
    {
        if (isPoweredUp)
        {
            _playerScore.Points *= multiplier;
            GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(addedPoints * multiplier);
        }
        else
        {
            _playerScore.Points += addedPoints;
            GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(addedPoints);
        }
    }

    // will change later if hud is finalized
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
