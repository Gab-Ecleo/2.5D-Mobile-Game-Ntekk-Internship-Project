using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    public TMP_Text uiText;

    public ScoresSO _playerScore;

    // can be deleted after testing or deemed not part of gameplay
    private void Start()
    {
        _playerScore.Points = 0;
    }
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

    // funcation being called by the action to update ui and add score
    public void PointSystem(int addedPoints, int multiplier, bool isPoweredUp)
    {
        if (isPoweredUp)
        {
            _playerScore.Points *= multiplier;
        }
        else
        {
            _playerScore.Points += addedPoints;
        }
    }

    // will change later if hud is finalized
    public void UpdateUI()
    {
        uiText.text = _playerScore.Points.ToString();
    }
}
