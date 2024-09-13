using System;
using System.Collections;
using AudioScripts;
using AudioScripts.AudioSettings;
using EventScripts;
using ScriptableData;
using UnityEngine;
using UnityEngine.Serialization;

public class RainEffect : MonoBehaviour
{
    [SerializeField] private float _hazardDuration = 5f;
    [SerializeField] private float _hazardModifier = 2;
    [SerializeField] public GameObject _rainParticles;

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
        _gameManager.FetchHazardData().IsRainActive = true;
        _isCorActive = true;
        var rainParticles = Instantiate(_rainParticles);
        
        AudioEvents.ON_HAZARD_TRIGGER?.Invoke("rain");
        
        _playerStat.stats.movementSpeed /= _hazardModifier;

        yield return new WaitForSeconds(_hazardDuration);
        Debug.Log("End of Hazard Duration");
        
        SfxScript.Instance.StopSFX();
        rainParticles.SetActive(false);
        
        _playerStat.stats.movementSpeed *= _hazardModifier;

        _isCorActive = false;
        
        _gameManager.FetchHazardData().IsRainActive = false;
    }
}
