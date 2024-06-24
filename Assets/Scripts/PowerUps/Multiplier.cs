using PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : PlayerPowerUps
{
    [SerializeField] private int _multiplierAmount = 2;
    [SerializeField] private bool _hasPowerUp = true;

    protected override void OnPowerUpActive()
    {
        base.OnPowerUpActive();

        _playerScore.Multiplier = _multiplierAmount;
        GameEvents.ON_SCORE_CHANGES?.Invoke(_playerScore.PointsToAdd, _playerScore.Multiplier, _playerScore.PowerUpDuration, true);
    }

    protected override void OnPowerUpDeactivate()
    {
        base.OnPowerUpDeactivate();

        _playerScore.Multiplier = 1;
        GameEvents.ON_SCORE_CHANGES?.Invoke(_playerScore.PointsToAdd, _playerScore.Multiplier, _playerScore.PowerUpDuration, false);
    }
}
