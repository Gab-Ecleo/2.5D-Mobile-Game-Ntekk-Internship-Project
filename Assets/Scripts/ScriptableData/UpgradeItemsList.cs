using System.Collections.Generic;
using UnityEngine;
using UpgradeShop;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Upgrade Items Data", menuName = "Upgrade Shop/Upgrade Items Data", order = 0)]
    public class UpgradeItemsList : ScriptableObject
    {
        [SerializeField] private List<UpgradeItem> items;
        public List<UpgradeItem> Items => items;
    }
}