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
        _currentPlayerStats._canRez = _initialPlayerStats._canRez;
    }

    private void Update()
    {
        //Updating values
        _currentPlayerStats._barrierUpgrade = _upgradeCount;
        _currentPlayerStats._barrierDurability = _durability;
        _currentPlayerStats._barrierDuration = _duration;
              
        //Upgrade Tree
        if (_upgradeCount == 1)
        {
            // Activate Barrier effect with starting duration and durability
            if (Input.GetKeyDown("p"))
            {
                _upgradeCount++;
                Debug.Log("Barrier Upgraded");
            }
        }
        else if (_upgradeCount == 2)
        {
            // Add durability to the barrier using the modifier
            _durability += _durabModif;
            if (Input.GetKeyDown("p"))
            {
                _upgradeCount++;
                Debug.Log("Barrier Upgraded");
            }
        }
        else if (_upgradeCount == 3)
        {
            // Add duration to the barrier using the modifier
            _duration += _durationModif;
            if (Input.GetKeyDown("p"))
            {
                _upgradeCount++;
                Debug.Log("Barrier Upgraded");
            }
        }
        else if (_upgradeCount == 4)
        {
            // Add durability to the barrier using the modifier
            _durability += _durabModif;
            if (Input.GetKeyDown("p"))
            {
                _upgradeCount++;
                Debug.Log("Barrier Upgraded to Max Level! Revive Passive Unlocked!");
            }
        }
        else if (_upgradeCount == 5)
        {
            // Add duration to the barrier using the modifier
            _duration += _durationModif;
            //Activate Revive Upgrade Node
        }
    }
}
