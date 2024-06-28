using PlayerScripts;
using System.Collections;
using UnityEngine;

/// <summary>
/// attach to row manager
/// </summary>
public class Multiplier : PlayerPowerUps
{
    [Header("Multiplier Power-Up")]
    [SerializeField] private int _multiplierAmount = 2;
    [SerializeField] private bool _hasPowerUp;

    protected override void OnPowerUpReady(PowerTypes powerType)
    {
        base.OnPowerUpReady(PowerTypes.Multiplier);
    }

    protected override void OnPowerUpActive()
    {
        base.OnPowerUpActive();
        _hasPowerUp = true;
        IsInEffect = true;
    }

    protected override void OnPowerUpDeactivate()
    {
        base.OnPowerUpDeactivate();
        _hasPowerUp = false;
        IsInEffect = false;
        GameEvents.ON_SCORE_CHANGES?.Invoke(_playerScore.PointsToAdd, _playerScore.Multiplier, _hasPowerUp);
    }

    protected override void ActivateMultiplier()
    {
        base.ActivateMultiplier();
        GameEvents.ON_SCORE_CHANGES?.Invoke(_playerScore.PointsToAdd, _multiplierAmount, _hasPowerUp);
    }
}
