using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text uiText;

    [SerializeField] private ScoresSO scoresSO;

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
    public void PointSystem(int addedPoints, int multiplier, float duration, bool isPoweredUp)
    {
        if (isPoweredUp)
        {
            StartCoroutine(HandleMultiplierDuration(addedPoints, multiplier, duration));
        }
        else
        {
            scoresSO.Points += addedPoints;
        }

        // update the ui
        GameEvents.ON_UI_CHANGES?.Invoke();
    }

    // will change later if hud is finalized
    public void UpdateUI()
    {
        uiText.text = scoresSO.Points.ToString();
    }

    // to change for multiplier power up handling 
    IEnumerator HandleMultiplierDuration(int addedPoints, int multiplier, float duration)
    {
        // depends on balancing of the game which multiplier to use
        scoresSO.Points *= multiplier;
        //scoresSO.Points += addedPoints * multiplier;

        // timer
        float timer = 0;
        while (timer < duration)
        {
            Debug.Log(timer);
            timer += 0.5f;
            yield return new WaitForSeconds(0.1f);
        }

        // revert to normal scoring
        scoresSO.HasPowerUpMultiplier = false;
        scoresSO.Points += addedPoints;
    }


    // for testing only
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEvents.ON_SCORE_CHANGES?.Invoke(scoresSO.PointsToAdd, scoresSO.Multiplier, scoresSO.PowerUpDuration, scoresSO.HasPowerUpMultiplier);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            scoresSO.PowerUpDuration = 5;
            scoresSO.HasPowerUpMultiplier = true;
        }
    }*/
}
