using System;
using System.Collections;
using ScriptableData;
using UnityEngine;
using UnityEngine.Serialization;

public class RainEffect : MonoBehaviour
{
    [SerializeField] private float _hazardDuration = 5f;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _hazardModifier = 2;

    private PlayerStatsSO _playerStat;
    private GameManager _gameManager;
    private bool _isCorActive;

    #region UNITY METHODS

    private void Awake()
    {
        GameEvents.TRIGGER_RAIN_HAZARD += TriggerRainHazard;
    }

    private void OnDestroy()
    {
        GameEvents.TRIGGER_RAIN_HAZARD -= TriggerRainHazard;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _playerStat = _gameManager.FetchCurrentPlayerStat();
        _isCorActive = false;
        
        //Set current speed from the player stat speed value
        _currentSpeed = _playerStat.movementSpeed;
    }

    #endregion
    
    private void TriggerRainHazard()
    {
        if (_isCorActive || _gameManager.IsGameOver()) return;

        StartCoroutine(SlowPlayerMovement());
    }
    
    //Halves the player's speed for [Hazard Duration], then returns to the original value
    IEnumerator SlowPlayerMovement()
    {
        _isCorActive = true;
        
        _currentSpeed /= _hazardModifier;
        _playerStat.movementSpeed = _currentSpeed;

        yield return new WaitForSeconds(_hazardDuration);
        Debug.Log("End of Hazard Duration");
        _currentSpeed *= _hazardModifier;
        _playerStat.movementSpeed = _currentSpeed;

        _isCorActive = false;
    }
}
