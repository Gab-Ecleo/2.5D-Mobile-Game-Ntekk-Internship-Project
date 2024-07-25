using ScriptableData;
using TMPro;
using UnityEngine;
using UpgradeShop.ItemLevels;

namespace UpgradeShop
{
    //Handles the generation and initialization of the generated item's values.
    public class ItemGenerator : MonoBehaviour
    {
        [Header("Script References")] 
        [SerializeField] private ItemInfoUI itemInfoUI;
        [SerializeField] private ItemLevelGenerator upgradeLevelGenerator;

        [Header("Upgrade Items. To be private")]
        [SerializeField] private UpgradeItem currentItem;
        [SerializeField] private UpgradeItemIdentifier idData;
        
        public void UpdateItemValues(UpgradeItem item, UpgradeItemIdentifier id)
        {
            currentItem = item;
            idData = id;
            
            itemInfoUI.UpdateNewItemName(currentItem);
            upgradeLevelGenerator.GenerateUpgradeCells(currentItem,idData);
        }
    }
}