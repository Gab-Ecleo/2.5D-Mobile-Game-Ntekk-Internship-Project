using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableData;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //player's local stats
    [SerializeField] private PlayerStatsSO _initialPlayerStats;
    [SerializeField] private PlayerStatsSO _currentPlayerStats;
    [SerializeField] private int _upgradeCount;
    [SerializeField] private int _durability;
    [SerializeField] private float _duration;
    [SerializeField] private int _durabModif = 1;
    [SerializeField] private float _durationModif = 1;
    
    private void Start()
    {
        //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
        _currentPlayerStats._barrierUpgrade = _initialPlayerStats._barrierUpgrade;
        _currentPlayerStats._barrierDurability = _initialPlayerStats._barrierDurability;
        _currentPlayerStats._barrierDuration = _initialPlayerStats._barrierDuration;
        _upgradeCount = _currentPlayerStats._barrierUpgrade;
        _durability = _currentPlayerStats._barrierDurability;
        _duration = _currentPlayerStats._barrierDuration;
    }

    private void Update()
    {
        if (_currentPlayerStats._barrierUpgrade == 1)
        {
            // Activate Barrier effect with starting duration and durability
        }
        if (_currentPlayerStats._barrierUpgrade == 2)
        {
            // Add durability to the barrier using the modifier
            _durabModif += _durability;
        }
        if (_currentPlayerStats._barrierUpgrade == 3)
        {
            // Add duration to the barrier using the modifier
            _durationModif += _duration;
        }
        if (_currentPlayerStats._barrierUpgrade == 4)
        {
            // Add durability to the barrier using the modifier
            _durabModif += _durability;
        }
        if (_currentPlayerStats._barrierUpgrade == 5)
        {
            // Add duration to the barrier using the modifier
            _durationModif += _duration;
            //Activate Revive Upgrade Node
        }
    }
}
