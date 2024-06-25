using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;
using UnityEngine.UI;

public class Revive : MonoBehaviour
{
    //player's local stats
    [SerializeField] private PlayerStatsSO _initialPlayerStats;
    [SerializeField] private PlayerStatsSO _currentPlayerStats;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private Button _button;
    [SerializeField] private Button _reviveDeathButton;
    
    private void Start()
    {
        //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
        _currentPlayerStats._barrierUpgrade = _initialPlayerStats._barrierUpgrade;
        _currentPlayerStats._barrierDurability = _initialPlayerStats._barrierDurability;
        _currentPlayerStats._barrierDuration = _initialPlayerStats._barrierDuration;
        _currentPlayerStats._canRez = _initialPlayerStats._canRez;
    }

    public void upgradeRez()
    {
        // Activate rez passive
        Debug.Log("Unlocked Revive ! You may now revive the player once per game."); 
        _currentPlayerStats._canRez = true;
        _button.interactable = false;
        _reviveDeathButton.interactable = true;
    }

    public void revivePlayer()
    {
        if (_currentPlayerStats._canRez)
        {
            _deathScreen.SetActive(false);
            _currentPlayerStats._canRez = false;
            Time.timeScale = 1;
        }
    }
}
