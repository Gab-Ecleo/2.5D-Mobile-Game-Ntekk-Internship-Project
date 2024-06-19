using System;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    /*
    [SerializeField] private int Points;
    [SerializeField] private int PointsToAdd = 2;
    [SerializeField] private int Multiplier = 1;
    [SerializeField] private bool HasPowerUpMultiplier;
    */

    [SerializeField] private TMP_Text uiText;
    [SerializeField] private ScoresSO scoresSO;
    private void OnEnable()
    {
        GameEvents.ON_SCORE_CHANGES += AddedPoints;
        GameEvents.ON_UI_CHANGES += UpdateUI;
        GameEvents.IS_SCORE_MULTIPLIED += Multiplied;
    }

    private void OnDisable()
    {
        GameEvents.ON_SCORE_CHANGES -= AddedPoints;
        GameEvents.ON_UI_CHANGES -= UpdateUI;
        GameEvents.IS_SCORE_MULTIPLIED -= Multiplied;
    }

    public void AddedPoints(int addedPoints)
    {
        scoresSO.Points += addedPoints;
        GameEvents.ON_UI_CHANGES?.Invoke();
    }

    public void Multiplied(int _addedPoints, int _multiplier, bool isPoweredUp)
    {
        if (isPoweredUp)
        {
            scoresSO.Points += _addedPoints * _multiplier;
            GameEvents.ON_UI_CHANGES?.Invoke();
        }
    }

    public void UpdateUI()
    {
        uiText.text = scoresSO.Points.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameEvents.ON_SCORE_CHANGES?.Invoke(scoresSO.PointsToAdd);
            GameEvents.IS_SCORE_MULTIPLIED?.Invoke(scoresSO.PointsToAdd, scoresSO.Multiplier, scoresSO.HasPowerUpMultiplier);
        }
    }
}
