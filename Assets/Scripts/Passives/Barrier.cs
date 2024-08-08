using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableData;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Barrier : MonoBehaviour
{
    //player's local stats
    // CHANGE currentPlayerStats to initialPlayerStats to make upgrades permanent
    [SerializeField] private PlayerStatsSO initialPlayerStats;
    [SerializeField] private PlayerStatsSO currentPlayerStats;
    [SerializeField] private int durabModif = 1;
    [SerializeField] private UnityEngine.UI.Button button;
    [SerializeField] private UnityEngine.UI.Button rezButton;

    // private void Start()
    // {
    //     //initializes the data. UPDATE THIS ONCE MORE DATA IS USED.
    //     currentPlayerStats.stats.barrierUpgrade = initialPlayerStats.stats.barrierUpgrade;
    //     currentPlayerStats.stats.barrierDurability = initialPlayerStats.stats.barrierDurability;
    //     currentPlayerStats.stats.canRez = initialPlayerStats.stats.canRez;
    // }
    //
    // public void UpgradeBarrier()
    // {
    //     currentPlayerStats.stats.barrierUpgrade++;
    //     switch (currentPlayerStats.stats.barrierUpgrade)
    //     {
    //         //Upgrade Tree
    //         case 1:
    //             // Add durability to the barrier using the modifier
    //             currentPlayerStats.stats.barrierDurability += durabModif;
    //             Debug.Log("Barrier Upgraded to Level 2 ! Added more barrier hit points!");
    //             break;
    //         case 2:
    //             // Add duration to the barrier using the modifier
    //             currentPlayerStats.stats.barrierDurability += durabModif;
    //             Debug.Log("Barrier Upgraded to Level 3 ! Added more barrier hit points!");
    //             break;
    //         case 3:
    //             // Add durability to the barrier using the modifier
    //             currentPlayerStats.stats.barrierDurability += durabModif;
    //             Debug.Log("Barrier Upgraded to Level 4 ! Added more barrier hit points!");
    //             break;
    //         case 4:
    //             // Add duration to the barrier using the modifier
    //             currentPlayerStats.stats.barrierDurability += durabModif;
    //             //Activate Revive Upgrade Node
    //             button.interactable = false;
    //             rezButton.interactable = true;
    //             Debug.Log("Barrier Upgraded to Max Level ! Added more barrier hit points!  Revive Passive Unlocked!");
    //             break;
    //     }
    // }
}
