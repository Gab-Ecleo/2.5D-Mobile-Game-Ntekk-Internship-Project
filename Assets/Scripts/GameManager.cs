using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    #region Script Instance

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    #endregion
    
    [Header("Scriptable")]
    [SerializeField] private PlayerStatsSO playerCurrentStat;
    [SerializeField] private PlayerStatsSO initialPlayerStats;
    [SerializeField] private ScoresSO scoreSO;
    [SerializeField] private CurrencySO currencySo;
    [SerializeField] private HazardSO hazardSo;

    [Header("Game Objects")] 
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject canvas;
    
    [Header("UI Objects")]
    [SerializeField] private GameObject gameOverText;
    
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
        gameOverText.SetActive(false);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        gameOverText.SetActive(true);
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
    
    #region Object Fetching

    public PlayerStatsSO FetchCurrentPlayerStat()
    {
        return playerCurrentStat;
    }

    public PlayerStatsSO FetchInitialPlayerStat()
    {
        return initialPlayerStats;
    }

    public ScoresSO FetchScores()
    {
        return scoreSO;
    }
    
    public CurrencySO FetchCurrency()
    {
        return currencySo;
    }

    public HazardSO FetchHazardData()
    {
        return hazardSo;
    }

    public GameObject FetchPlayer()
    {
        return playerObj;
    }

    public GameObject FetchCanvas()
    {
        return canvas;
    }

    #endregion


}
