using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

public class Revive : MonoBehaviour
{
    //player's local stats
    [SerializeField] private PlayerStatsSO _initialPlayerStats;
    [SerializeField] private PlayerStatsSO _currentPlayerStats;
    [SerializeField] private int _upgradeCount;
    
    private void Start()
    {
        //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
        _currentPlayerStats._barrierUpgrade = _initialPlayerStats._barrierUpgrade;
        _currentPlayerStats._barrierDurability = _initialPlayerStats._barrierDurability;
        _currentPlayerStats._barrierDuration = _initialPlayerStats._barrierDuration;
        _currentPlayerStats._canRez = _initialPlayerStats._canRez;
        _upgradeCount = _currentPlayerStats._rezUpgrade;
    }

    private void Update()
    {
        if (_upgradeCount == 1)
        {
            // Activate rez passive
            _currentPlayerStats._canRez = true;
        }
    }
}
