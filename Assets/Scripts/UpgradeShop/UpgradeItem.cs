using System;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeShop
{
    [Serializable]
    public class UpgradeItem
    {
        public string identifier;
        public string upgradeName;
        public string description;

        public Upgradables affectedStat;
        
        public int maxLevelCount;

        public int currentLevel;
        public float currentIncrease;

        [Tooltip("Should have a + 1 count from the maxLevelCount value.")]
        public List<int> costPerLevel;
        [Tooltip("Should have a + 1 count from the maxLevelCount value.")]
        public List<float> valuePerLevel;
    }
}