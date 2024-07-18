using ScriptableData;
using TMPro;
using UnityEngine;
using UpgradeShop.ItemLevels;

namespace UpgradeShop
{
    public class ItemGenerator : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        
        [Header("Script References")]
        [SerializeField] private ItemLevelGenerator upgradeLevelGenerator;

        [Header("Upgrade Items. To be private")]
        [SerializeField] private UpgradeItem currentItem;
        [SerializeField] private UpgradeItemIdentifier idData;

        
        public void UpdateItemValues(UpgradeItem item, UpgradeItemIdentifier id)
        {
            currentItem = item;
            idData = id;
            
            title.text = currentItem.upgradeName;
            description.text = currentItem.description;
            
            upgradeLevelGenerator.GenerateUpgradeCells(currentItem,idData);
            
        }
    }
}