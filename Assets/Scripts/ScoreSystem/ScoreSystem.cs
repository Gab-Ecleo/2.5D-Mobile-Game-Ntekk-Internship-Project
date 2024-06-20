using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private ScoresSO scoresSO;

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

    public void PointSystem(int addedPoints, int multiplier, float duration, bool isPoweredUp)
    {
        if (isPoweredUp)
        {
            StartCoroutine(HandleMultiplierDuration(multiplier, duration));
            isPoweredUp = false;
        }
        else
        {
            scoresSO.Points += addedPoints;
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
            GameEvents.ON_SCORE_CHANGES?.Invoke(scoresSO.PointsToAdd, scoresSO.Multiplier, scoresSO.PowerUpDuration, scoresSO.HasPowerUpMultiplier);
        }
    }

    private IEnumerator HandleMultiplierDuration( int multiplier, float duration)
    {
        scoresSO.Points *= multiplier;
        GameEvents.ON_UI_CHANGES?.Invoke();

        yield return new WaitForSeconds(duration);
    }
}
