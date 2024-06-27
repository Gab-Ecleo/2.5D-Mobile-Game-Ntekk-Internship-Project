using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : PlayerPowerUps
{
    [Header("Spring Power-Up")]
    [SerializeField] protected float SpringJumpHeight;


    protected override void OnPowerUpActive()
    {
        base.OnPowerUpActive();   
        _currPlayerStatsSO.jumpHeight = SpringJumpHeight;
    }

    protected override void OnPowerUpDeactivate()
    {
        base.OnPowerUpDeactivate();
        _currPlayerStatsSO.jumpHeight = _initialPlayerStatsSO.jumpHeight;
    }
}
