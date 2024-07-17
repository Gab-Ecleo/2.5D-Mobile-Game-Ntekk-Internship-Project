using System;
using System.Collections.Generic;

namespace UpgradeShop
{
    [Serializable]
    public class UpgradeItem
    {
        public string identifier;
        public string upgradeName;
        public string description;
        
        public int maxLevelCount;

        public int currentLevel;
        public float currentIncrease;

        public List<int> costPerLevel;
        public List<float> increasePerLevel;
    }
}