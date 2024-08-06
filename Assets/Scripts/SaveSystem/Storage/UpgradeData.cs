using System;
using System.Collections.Generic;
using UpgradeShop;

namespace SaveSystem.Storage
{
    [Serializable]
    public class UpgradeData
    {
        public List<UpgradeItem> items = new List<UpgradeItem>();
    }
}