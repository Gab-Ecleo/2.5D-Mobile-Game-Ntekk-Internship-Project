using System;
using System.Collections;
using PlayerScripts;
using ScriptableData;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PowerState
{
    ready,
    active,
    cooldown,
    deactivate
}

public enum PowerTypes
{
    None,
    Multiplier,
    Spring,
    TimeSlow
}

public class PlayerPowerUps : MonoBehaviour
{
    [Header("Permissions")]
    public bool IsInEffect = true;

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

    protected GameObject _playerGo;
    [SerializeField] protected PlayerBehavior _playerBehaviour;
    protected bool _powerUpInitialized = false;
    protected Animator _animator;
    protected Mesh _mesh;
    protected PowerState _powerState;

    protected PowerTypes _currentPowerUpType = PowerTypes.None;

    #region Initializations
    protected virtual void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        _playerGo = GameObject.FindGameObjectWithTag("Player");
        _animator = _playerGo.GetComponent<Animator>();
        _mesh = _playerGo.GetComponent<Mesh>();

        _playerBehaviour = _playerGo.GetComponent<PlayerBehavior>();   
        if (_playerBehaviour == null){Debug.LogError("There's no player behaviour attached");return;}

        if (_currPlayerStatsSO == null){Debug.LogError("There's no player stats referenced");return;}

        _powerUpInitialized = true;
    }

    #endregion

    #region Power States
    // always ready state if it is initialized properly
    protected virtual void OnPowerUpReady(PowerTypes powerTypes)
    {
        if (_powerState != PowerState.ready){Debug.Log("Player Power-up is not yet ready to use");return;}
        _currentPowerUpType = powerTypes;

        switch (_currentPowerUpType)
        {
            case PowerTypes.Multiplier:
                ActivateMultiplier();
                Debug.Log("Multiplier Power-up");
                break;
            case PowerTypes.Spring:
                ActivateSpring();
                Debug.Log("Spring Power-up");
                break;
            case PowerTypes.TimeSlow:
                ActivateTimeSlow();
                Debug.Log("Time Slow Power-up");
                break;
            default:
                Debug.LogError("Unhandled power-up type: " + _currentPowerUpType);
                break;
        }
        StartCoroutine(DelayBeforeActivation());
    }

    protected virtual void OnPowerUpActive()
    {
        if (_powerState != PowerState.active) { Debug.Log("Player Power-up is not yet active"); return;}
        Debug.Log("Power-up activated!");

        StartCoroutine(PowerUpDuration());
    }


    /// <summary>
    /// Deactivate power-ups and bring back to normal gameplay
    /// </summary>
    protected virtual void OnPowerUpDeactivate()
    {
        if (_powerState != PowerState.deactivate) { Debug.Log("Player Power-up is not yet deactivated"); return; }

        Debug.Log("Power-up deactivated!");
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
        yield return new WaitForSeconds(powerUpActivateDuration);
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

    #region Additionals
    protected virtual void OnHit() { }

    protected virtual void OnDeath() { }

    protected virtual void OnRespawn() { }

    protected virtual void OnReset() { }
    #endregion

    #region Power Ups Activation Methods
    protected virtual void ActivateMultiplier() 
    { 
        _currPlayerStatsSO.hasMultiplier = true;
        _currPlayerStatsSO.springJump = false;
        _currPlayerStatsSO.timeSlow = false;
    }

    protected virtual void ActivateSpring()
    {
        _currPlayerStatsSO.springJump = true;
        _currPlayerStatsSO.hasMultiplier = false;
        _currPlayerStatsSO.timeSlow = false;
    }

    protected virtual void ActivateTimeSlow()
    {
        _currPlayerStatsSO.timeSlow = true;
        _currPlayerStatsSO.springJump = false;
        _currPlayerStatsSO.hasMultiplier = false;
    }
    #endregion

    public virtual void PermitUseOfPowerUp(bool isPowerUpInEffect)
    {
        IsInEffect = isPowerUpInEffect;
    }

    // handles input
    public void PowerUp(InputAction.CallbackContext context)
    {
        if (context.performed && _powerUpInitialized)
        {
            if (_currPlayerStatsSO.hasMultiplier) { OnPowerUpReady(PowerTypes.Multiplier); }

            if (_currPlayerStatsSO.springJump) { OnPowerUpReady(PowerTypes.Spring); }

            if (_currPlayerStatsSO.timeSlow) { OnPowerUpReady(PowerTypes.TimeSlow); }
        }
    }

}
