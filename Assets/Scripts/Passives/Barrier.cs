using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Barrier : MonoBehaviour
{
    //player's local stats
    [SerializeField] private PlayerStatsSO _initialPlayerStats;
    [SerializeField] private PlayerStatsSO _currentPlayerStats;
    [SerializeField] private int _durabModif = 1;
    [SerializeField] private float _durationModif = 1;
    [SerializeField] private Button _button;
    [SerializeField] private Button _rezButton;

    private void Start()
    {
        //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
        _currentPlayerStats._barrierUpgrade = _initialPlayerStats._barrierUpgrade;
        _currentPlayerStats._barrierDurability = _initialPlayerStats._barrierDurability;
        _currentPlayerStats._barrierDuration = _initialPlayerStats._barrierDuration;
        _currentPlayerStats._canRez = _initialPlayerStats._canRez;
    }

    public void upgradeBarrier()
    {
        //Upgrade Tree
        if (_currentPlayerStats._barrierUpgrade == 1)
        {
            // Activate Barrier effect with starting duration and durability
            _currentPlayerStats._barrierUpgrade++;
            Debug.Log("Unlocked Barrier !");
        }
        else if (_currentPlayerStats._barrierUpgrade == 2)
        {
            // Add durability to the barrier using the modifier
            _currentPlayerStats._barrierDurability += _durabModif;
            _currentPlayerStats._barrierUpgrade++;
            Debug.Log("Barrier Upgraded to Level 2 !");

        }
        else if (_currentPlayerStats._barrierUpgrade == 3)
        {
            // Add duration to the barrier using the modifier
            _currentPlayerStats._barrierDuration += _durationModif;
            _currentPlayerStats._barrierUpgrade++;
            Debug.Log("Barrier Upgraded to Level 3 !");
        }
        else if (_currentPlayerStats._barrierUpgrade == 4)
        {
            // Add durability to the barrier using the modifier
            _currentPlayerStats._barrierDurability += _durabModif;
            _currentPlayerStats._barrierUpgrade++;
            Debug.Log("Barrier Upgraded to Level 4 !");
        }
        else if (_currentPlayerStats._barrierUpgrade == 5)
        {
            // Add duration to the barrier using the modifier
            _currentPlayerStats._barrierDuration += _durationModif;
            //Activate Revive Upgrade Node
            _button.interactable = false;
            _rezButton.interactable = true;
            Debug.Log("Barrier Upgraded to Max Level ! Revive Passive Unlocked!");
        }
        
    }
}
