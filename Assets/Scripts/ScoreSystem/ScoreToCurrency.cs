using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;
using TMPro;   
public class ScoreToCurrency : MonoBehaviour
{
    private PlayerStatsSO _initialPlayerStats;
    
    private ScoresSO _scores;
    
    private GameManager _gameManager;
    
    [SerializeField] private int _scoreModifier = 2;
    
    [SerializeField] private int _finalScore;
    [SerializeField] private int _currency;

    [SerializeField] TMP_Text currencyText;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _initialPlayerStats = _gameManager.FetchInitialPlayerStat();
        _scores = _gameManager.FetchScores();

        UpdateUI();
    }

    private void ConvertScore()
    {
        // get the final score
        _finalScore = _scores.Points ;
        // convert the final score with the score modifier to currency
        _currency = _finalScore / _scoreModifier;
        // add the currency to the player
        _initialPlayerStats.coins += _currency;
    }

    public void UpdateUI()
    {
        ConvertScore();
        
        if(currencyText == null) return;
        currencyText.text = _currency.ToString();
    }

    private void Update()
    {
        UpdateUI();
    }
}
