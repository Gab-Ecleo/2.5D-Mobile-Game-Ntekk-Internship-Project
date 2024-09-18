using System.Collections;
using AudioScripts;
using EventScripts;
using ScriptableData;
using UnityEngine;

public class RainEffect : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float _hazardDuration = 5f;
    [SerializeField] private float _hazardModifier = 2;
    
    [Header("Dependency")]
    [SerializeField] public GameObject _rainParticles;

    private float _movSpeed;
    private PlayerStatsSO _playerStat;
    private GameManager _gameManager;
    private bool _isHazardActive;

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
        SetParameters();
    }

    #endregion
    
    private void SetParameters()
    {
        _isHazardActive = false;
        _gameManager = GameManager.Instance;
        _playerStat = _gameManager.FetchCurrentPlayerStat();
    }
    
    public void TriggerRainHazard()
    {
        if (_isHazardActive || _gameManager.IsGameOver()) return;

        StartCoroutine(SlowPlayerMovement());
    }
    
    //Halves the player's speed for [Hazard Duration], then returns to the original value
    IEnumerator SlowPlayerMovement()
    {
        SetHazardCondition(true);
        
        //Set Visual Effects
        var rainParticles = Instantiate(_rainParticles);
        AudioEvents.ON_HAZARD_TRIGGER?.Invoke("rain");
        
        ModifyPlayerMovement("activate");

        yield return new WaitForSeconds(_hazardDuration);
        
        ModifyPlayerMovement("deactivate");

        //Remove Visual Effects
        rainParticles.SetActive(false); //vfx
        SfxScript.Instance.StopSFX();   //sfx
        
        
        SetHazardCondition(false);
    }

    private void SetHazardCondition(bool condition)
    {
        _isHazardActive = condition;
        _gameManager.FetchHazardData().IsRainActive = condition;
    }

    private void ModifyPlayerMovement(string condition)
    {
        switch (condition)
        {
            case "activate":
                _playerStat.stats.movementSpeed /= _hazardModifier;
                break;
            
            case "deactivate":
                _playerStat.stats.movementSpeed *= _hazardModifier;
                break;
        }
    }
}
