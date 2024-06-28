using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Revive : MonoBehaviour
{
    //player's local stats
    // CHANGE currentPlayerStats to initialPlayerStats to make upgrades permanent
    [SerializeField] private PlayerStatsSO initialPlayerStats;
    [SerializeField] private PlayerStatsSO currentPlayerStats;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Button button;
    [SerializeField] private Button reviveDeathButton;
    
    private void Start()
    {
        //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
        currentPlayerStats.canRez = initialPlayerStats.canRez;
    }

    public void UpgradeRez()
    {
        // Activate rez passive
        Debug.Log("Unlocked Revive ! You may now revive the player once per game.");
        currentPlayerStats.rezUpgrade++;
        currentPlayerStats.canRez = true;
        button.interactable = false;
        reviveDeathButton.interactable = true;
    }

    public void RevivePlayer()
    {
        if (!currentPlayerStats.canRez) return;
        deathScreen.SetActive(false);
        currentPlayerStats.canRez = false;
        Time.timeScale = 1;
    }
}
