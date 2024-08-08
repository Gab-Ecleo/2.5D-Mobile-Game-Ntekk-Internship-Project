using System;
using System.Collections;
using PlayerScripts;
using ScriptableData;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPowerUps : MonoBehaviour, PowerUpsBaseMethods
{
    [Tooltip("Go To PowerBase Script to add another power up")]

    // default floats
    [Header("Timers")]
    public float powerUpReadyDelay = 0.1f;
    public float powerUpActivateDuration = 3.0f;
    public float powerUpDeactivateDelay = 0.5f;
    public float powerUpCooldown = 5.0f;

    [Header("References")]
    [SerializeField] protected PlayerStatsSO _initialPlayerStatsSO;
    [SerializeField] protected PlayerStatsSO _currPlayerStatsSO;
    [SerializeField] protected ScoresSO _playerScore;
    public bool PowerUpInitialized => _powerUpInitialized;

    [Header("Time Slow Power-up")]
    [SerializeField] private float _slowMotionFactor = 0.1f;
    [SerializeField] private float _originalTimeScale;

    [Header("Spring Power-Up")]
    [SerializeField] private float _currentJumpHeight;
    [SerializeField] private float _jumpBoost = 3;

    [Header("Multiplier Power-Up")]
    [SerializeField] private int _multiplierAmount = 2;

    protected GameObject _playerGo;
    protected PlayerBehavior _playerBehaviour;
    protected bool _powerUpInitialized = false;
    protected Animator _animator;
    protected MeshRenderer _mesh;
    protected PowerState _powerState;

    protected PowerTypes _currentPowerUpType = PowerTypes.None;

    #region Initializations
    protected virtual void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        try
        {
            _playerGo = GameObject.FindGameObjectWithTag("Player");
            if (_playerGo == null) {throw new Exception("Player not referenced");}

            _animator = _playerGo.GetComponent<Animator>();
            if (_animator == null) {throw new Exception("Animator not referenced");}

            _mesh = _playerGo.GetComponent<MeshRenderer>();
            if (_mesh == null) {throw new Exception("Mesh component not referenced");}

            _playerBehaviour = _playerGo.GetComponent<PlayerBehavior>();
            if (_playerBehaviour == null) {throw new Exception("Player Script not referenced");}

            if (_currPlayerStatsSO == null) {throw new Exception("PlayerStatsSO reference is not assigned.");}

            _powerUpInitialized = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Initialization failed: " + e.Message);
        }
    }

    #endregion

    #region Power States
    // always ready state if it is initialized properly
    protected virtual void OnPowerUpReady(PowerTypes powerTypes)
    {
        if (_powerState != PowerState.ready){Debug.Log("Player Power-up is not yet ready to use");return;}
        _currentPowerUpType = powerTypes;
        Debug.Log(_currentPowerUpType + " Is Ready");
        StartCoroutine(DelayBeforeActivation());
    }

    protected virtual void OnPowerUpActive()
    {
        if (_powerState != PowerState.active) { Debug.Log("Player Power-up is not yet active"); return;}
        Debug.Log(_currentPowerUpType + " Is Activated");
        StartCoroutine(PowerUpDuration());
    }

    /// <summary>
    /// Deactivate power-ups and bring back to normal gameplay
    /// </summary>
    protected virtual void OnPowerUpDeactivate()
    {
        if (_powerState != PowerState.deactivate) { Debug.Log("Player Power-up is not yet deactivated"); return; }
        Debug.Log("Power-up deactivated!");

        // turn on whatever power-up that is activated
        switch (_currentPowerUpType)
        {
            case PowerTypes.Multiplier:
                _currPlayerStatsSO.stats.hasMultiplier = false;
                OnMultiplierDeactivate();
                Debug.Log("Multiplier Power-up finished");
                break;
            case PowerTypes.Spring:
                _currPlayerStatsSO.stats.springJump = false;
                OnSpringDeactivate();
                Debug.Log("Spring Power-up finished");
                break;
            case PowerTypes.TimeSlow:
                _currPlayerStatsSO.stats.timeSlow = false;
                OnTimeSlowDeactivate();
                Debug.Log("Time Slow Power-up finished");
                break;
            case PowerTypes.SingleClear:
                _currPlayerStatsSO.stats.singleBlockRemover = false;
                OnSingleClearDeactivate();
                Debug.Log("Single Clear Finished");
                break;
            case PowerTypes.RowClear:
                _currPlayerStatsSO.stats.expressDelivery = false;
                OnRowClearDeactivate();
                Debug.Log("Row Clear Finished");
                break;
            default:
                Debug.LogError("Unhandled power-up type: " + _currentPowerUpType);
                break;
        }

        StartCoroutine(DelayBeforeCooldown());
    }

    /// <summary>
    /// Cooldown before player can use power-ups again
    /// </summary>
    protected virtual void OnPowerUpCooldown()
    {
        if (_powerState != PowerState.cooldown) { Debug.Log("Player Power-up is not on cooldown"); return; }
        _powerState = PowerState.cooldown;
        StartCoroutine(CooldownDuration());
    }
    #endregion

    #region IEnumerators
    private IEnumerator DelayBeforeActivation()
    {
        yield return new WaitForSeconds(powerUpReadyDelay);
        _powerState = PowerState.active;
        // check what power up is active
        
        OnPowerUpActive();
    }
    private IEnumerator PowerUpDuration()
    {
        switch (_currentPowerUpType)
        {
            case PowerTypes.Multiplier:
                OnMultiplierActivate();
                Debug.Log("Multiplier Power-up");
                break;
            case PowerTypes.Spring:
                OnSpringActivate();
                Debug.Log("Spring Power-up");
                break;
            case PowerTypes.TimeSlow:
                OnTimeSlowActivate();
                Debug.Log("Time Slow Power-up");
                break;
            case PowerTypes.SingleClear:
                OnSingleClearActivate();
                Debug.Log("Single Clear");
                break;
            case PowerTypes.RowClear:
                OnRowClearActivate();
                Debug.Log("Row Clear");
                break;
            default:
                Debug.LogError("Unhandled power-up type: " + _currentPowerUpType);
                break;
        }

        yield return new WaitForSecondsRealtime(powerUpActivateDuration);
        _powerState = PowerState.deactivate;
        OnPowerUpDeactivate();
    }

    private IEnumerator DelayBeforeCooldown()
    {
        yield return new WaitForSeconds(powerUpDeactivateDelay);
        _powerState = PowerState.cooldown;
        OnPowerUpCooldown();
    }
    private IEnumerator CooldownDuration()
    {
        Debug.Log("Cooldown Power-up");
        yield return new WaitForSeconds(powerUpCooldown);
        Debug.Log("Power-up is ready to use again");
        _powerState = PowerState.ready;
        _currentPowerUpType = PowerTypes.None;
    }
    #endregion

    #region PowerUpsBaseMethods
    public void OnMultiplierActivate()
    {
        GameEvents.ON_SCORE_CHANGES?.Invoke(_playerScore.PointsToAdd, _multiplierAmount, true);
    }

    public void OnMultiplierDeactivate()
    {
        GameEvents.ON_SCORE_CHANGES?.Invoke(_playerScore.PointsToAdd, _multiplierAmount, false);
    }

    public void OnSpringActivate()
    {
        _currentJumpHeight += _jumpBoost;
        _currPlayerStatsSO.stats.jumpHeight = _currentJumpHeight;
    }
    public void OnSpringDeactivate()
    {
        _currentJumpHeight -= _jumpBoost;
        _currPlayerStatsSO.stats.jumpHeight = _initialPlayerStatsSO.stats.jumpHeight;
    }
    public void OnTimeSlowActivate()
    {
        _originalTimeScale = Time.timeScale;
        Time.timeScale = _slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
    }
    public void OnTimeSlowDeactivate()
    {
        Time.timeScale = _originalTimeScale;
    }

    public void OnSingleClearActivate()
    {
        //yes
    }
    public void OnSingleClearDeactivate()
    {
        //yes
    }
    public void OnRowClearActivate()
    {
        //yes
    }
    public void OnRowClearDeactivate()
    {
        //yes
    }

    #endregion

    // handles input
    public void PowerUp(/*InputAction.CallbackContext context*/)
    {
        if (/*context.performed &&*/ _powerUpInitialized)
        {
            if (_currPlayerStatsSO.stats.hasMultiplier) 
            { 
                OnPowerUpReady(PowerTypes.Multiplier); 
            }
            if (_currPlayerStatsSO.stats.springJump) 
            {
                OnPowerUpReady(PowerTypes.Spring); 
            }
            if (_currPlayerStatsSO.stats.timeSlow) 
            {
                OnPowerUpReady(PowerTypes.TimeSlow); 
            }
            if (_currPlayerStatsSO.stats.singleBlockRemover) 
            {
                OnPowerUpReady(PowerTypes.SingleClear); 
            }
            if (_currPlayerStatsSO.stats.expressDelivery) 
            {
                OnPowerUpReady(PowerTypes.RowClear); 
            }
        }
    }
}
