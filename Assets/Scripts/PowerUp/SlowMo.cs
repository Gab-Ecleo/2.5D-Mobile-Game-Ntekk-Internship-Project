using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

public class SlowMo : MonoBehaviour
{
    /// <summary>
    /// Slowmo breaks player controller still need fix. RIOT PLS FIX
    /// </summary>
    
    private PlayerStatsSO _playerStatsSo;
    private PlayerPowerUps _powerUps;
    void Start()
    {
        _playerStatsSo = Resources.Load("PlayerData/CurrentPlayerStats") as PlayerStatsSO;
        _powerUps = GameObject.FindWithTag("Player").GetComponent<PlayerPowerUps>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerStatsSo.timeSlow = true;
            _powerUps.PowerUp();
            Destroy(gameObject);
        }
    }
}
