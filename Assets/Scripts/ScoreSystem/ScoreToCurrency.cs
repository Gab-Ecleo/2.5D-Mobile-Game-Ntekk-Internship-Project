using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

public class ScoreToCurrency : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO _initialPlayerStats;
    
    [SerializeField] private ScoresSO _scores;
    
    [SerializeField] private int _currency;
    [SerializeField] private int _scoreModifier;
    [SerializeField] private int _finalScore;
    
    private void ConvertScore()
    {
        // get the final score
        _finalScore = _scores.Points ;
        // convert the final score with the score modifier to currency
        _currency = _finalScore / _scoreModifier;
        // add the currency to the player
        _initialPlayerStats.coins += _currency;
    }
}
