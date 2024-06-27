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
    Freeze
}

public class PlayerPowerUps : MonoBehaviour
{
    [Header("Permissions")]
    public bool IsInEffect = true;

    public bool PowerUpInitialized => _powerUpInitialized;

    protected GameObject _playerGo;
    protected PlayerBehavior _playerBehaviour;
    protected bool _powerUpInitialized = false;
    protected PlayerStatsSO _playerStats;
    protected ScoresSO _playerScore;
    protected Animator _animator;
    protected Mesh _mesh;
    protected PowerState _powerState = PowerState.ready;
    protected float _timer = 0;
    protected float _jumpHeight;
    protected float _movementSpeed;
    protected float _jumpFallOff;
    
    // defaults
    protected float _powerUpDurations = 5.0f;
    protected float _cooldownTime = 5.0f;
    protected float _delayTime = 0.5f;

    #region Initializations
    protected virtual void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        _playerGo = this.gameObject;
        _animator = _playerGo.GetComponent<Animator>();
        _mesh = _playerGo.GetComponent<Mesh>();

        _playerBehaviour = _playerGo.GetComponent<PlayerBehavior>();
        if (_playerBehaviour == null){Debug.LogError("There's no player behaviour attached");return;}

        _playerStats = _playerBehaviour.GetComponent<PlayerStatsSO>();
        _playerScore = _playerBehaviour.GetComponent<ScoresSO>();

        if (_playerStats == null){Debug.LogError("There's no player stats referenced");return;}

        _movementSpeed = _playerStats.movementSpeed;
        _jumpHeight = _playerStats.jumpHeight;
        _jumpFallOff = _playerStats.jumpFallOff;

        _timer = 0;

        _powerUpInitialized = true;
    }

    public void InputHandler(InputAction.CallbackContext interact)
    {
        if (interact.phase == InputActionPhase.Performed)
        {
            OnPowerUpReady();
        }
    }
    #endregion

    #region Power States
    protected virtual void OnPowerUpReady()
    {
        if (_powerState != PowerState.ready)
        {
            Debug.Log("Player Power-up is not yet ready to use");
            return;
        }

        StartCoroutine(DelayBeforeActivation());
    }

    /// <summary>
    /// Holds unique function for specific power-up
    /// </summary>
    protected virtual void OnPowerUpActive()
    {
        Debug.Log("Power-up activated!");

        StartCoroutine(PowerUpDuration(_powerUpDurations));
    }

    /// <summary>
    /// Deactivate power-ups and bring back to normal gameplay
    /// </summary>
    protected virtual void OnPowerUpDeactivate()
    {
        Debug.Log("Power-up deactivated!");
        OnPowerUpCooldown();
    }

    /// <summary>
    /// Cooldown before player can use power-ups again
    /// </summary>
    protected virtual void OnPowerUpCooldown()
    {
        _powerState = PowerState.cooldown;
        _timer = 0;
        StartCoroutine(CooldownDuration(_cooldownTime));
    }
    #endregion

    #region IEnumerators
    private IEnumerator DelayBeforeActivation()
    {
        _timer = 0;
        while (_timer < _delayTime)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        _powerState = PowerState.active;
        OnPowerUpActive();
    }

    private IEnumerator CooldownDuration(float cooldown)
    {
        while (_timer < cooldown)
        {
            _timer += Time.deltaTime;
            yield return null;
        }

        _powerState = PowerState.ready;
    }

    private IEnumerator PowerUpDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _powerState = PowerState.deactivate;
        OnPowerUpDeactivate();
    }
    #endregion

    #region Additionals
    protected virtual void OnHit()
    {

    }

    protected virtual void OnDeath()
    {

    }

    protected virtual void OnRespawn()
    {

    }

    protected virtual void OnReset()
    {

    }
    #endregion

    public virtual void PermitUseOfPowerUp(bool isPowerUpInEffect)
    {
        IsInEffect = isPowerUpInEffect;
    }

}
