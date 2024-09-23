using System;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeShop
{
    //Handles the data of each upgrade item
    [Serializable]
    public class UpgradeItem
    {
        [Header("String Values")]
        public string identifier;
        public string upgradeName;
        public string description;
        
        [Header("Upgrade Sprite")]
        public Sprite upgradeSprite;

        [Header("Stat to be Upgraded")]
        public Upgradables affectedStat;
        
        [Header("Stat Sign")] 
        public StatSign statSign;

        [Header("Level Data")]
        public int maxLevelCount;
        public int currentLevel;
        
        [Tooltip("Should have a + 1 count from the maxLevelCount value.")]
        public List<int> costPerLevel;
        [Tooltip("Should have a + 1 count from the maxLevelCount value.")]
        public List<float> valuePerLevel;
    }
}