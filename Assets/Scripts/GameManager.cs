using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Script Instance

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    #endregion

    [SerializeField] private PlayerStatsSO _playerCurrentStat;
    [SerializeField] private PlayerStatsSO _initialPlayerStats;
    [SerializeField] private ScoresSO _scores;
    
    
    private bool _gameEnd;
    
    private void Awake()
    {
        // Initialization
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);

        //Event subscription
        GameEvents.IS_GAME_OVER += SetGameState;
    }

    private void Start()
    {
        _gameEnd = false;
    }

    private void OnDestroy()
    {
        GameEvents.IS_GAME_OVER -= SetGameState;
    }

    private void Update()
    {
        GameOver();
    }

    private void GameOver()
    {
        if (!IsGameOver()) return;

        if (Input.GetKeyDown(KeyCode.Escape)) //This is a tentative feature, it can be remove if not needed
        {
            //load main menu scene here
        }
        
        //Do game over stuff
    }

    private void SetGameState(bool _state)
    {
        _gameEnd = _state;
    }

    //Fetch game state
    public bool IsGameOver()
    {
        return _gameEnd;
    }

    public PlayerStatsSO FetchCurrentPlayerStat()
    {
        return _playerCurrentStat;
    }

    public PlayerStatsSO FetchInitialPlayerStat()
    {
        return _initialPlayerStats;
    }

    public ScoresSO FetchScores()
    {
        return _scores;
    }
}
