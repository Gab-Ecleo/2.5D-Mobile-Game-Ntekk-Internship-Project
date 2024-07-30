using System;
using System.Collections.Generic;
using EventScripts;
using ScriptableData;
using UnityEngine;

namespace UpgradeShop.ItemLevels
{
    //Handles the manipulation of each item's upgrade progress
    public class ItemLevelManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO initialStats;
        [SerializeField] private ItemInfoUI infoUI;
        
        [Header("Level Slots List. DO NOT MODIFY!!!")]
        [SerializeField]private List<ItemLevel> levelSlots;

        private Upgradables _statToUpgrade;
        
        private UpgradeItem _currentItem;
        private UpgradeItemIdentifier _id;
        
        private int _levelCount;

        #region INITIALIZATIONS
        public void AddLevelSlotToList(ItemLevel itemLevel)
        {
            levelSlots.Add(itemLevel);
        }

        public void RenderList(UpgradeItem item, UpgradeItemIdentifier identifier)
        {
            _currentItem = item;
            _id = identifier;
            _statToUpgrade = _currentItem.affectedStat;

            //renders each cell based on the current level found in the current item's data
            for (var index = 0; index < levelSlots.Count; index++)
            {
                var level = levelSlots[index];
                if (index <= _currentItem.currentLevel - 1 && _currentItem.currentLevel > 0)
                {
                    level.UpgradeSlot();
                }
                else if (index > _currentItem.currentLevel - 1)
                {
                    level.DegradeSlot();
                }
            }
            UpdateStats();
        }
        #endregion
        
        #region LEVEL_MANIPULATION
        public void AddLevel()
        {
            //Finds the next un-upgraded slot that could be upgraded/purchased
            for (var index = 0; index < levelSlots.Count; index++)
            {
                var level = levelSlots[index];
                switch (level.ItemLevelState)
                {
                    case LevelState.Upgraded:
                        continue;
                    
                    case LevelState.NotUpgraded:
                        //triggered when the player does not have enough money
                        if (initialStats.coins < _currentItem.costPerLevel[_currentItem.currentLevel + 1])
                        {
                            Debug.Log("YOU ARE POOR");
                            UpgradeShopEvents.OnInsufficientFunds?.Invoke();
                            return;
                        }
                        
                        level.UpgradeSlot();
                        UpgradeShopEvents.OnPurchaseLevel?.Invoke(_currentItem.costPerLevel[_currentItem.currentLevel + 1]);
                        _currentItem.currentLevel++;
                        UpdateStats();
                        UpdateLocalData();
                        return;
                        
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void ReduceLevel()
        {
            //Finds the latest upgraded slot that could be degraded
            for (var index = levelSlots.Count - 1; index > -1; index--)
            {
                var level = levelSlots[index];
                switch (level.ItemLevelState)
                {
                    case LevelState.NotUpgraded:
                        continue;
                    
                    case LevelState.Upgraded:
                        level.DegradeSlot();
                        UpgradeShopEvents.OnSellLevel?.Invoke(_currentItem.costPerLevel[_currentItem.currentLevel]);
                        _currentItem.currentLevel--;
                        UpdateStats();
                        UpdateLocalData();
                        return;
                        
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        #endregion

        //ADD NEW STATS UPDATES HERE
        #region STAT_UPDATE
        private void UpdateStats()
        {
            //Update Actual Stats
            switch (_statToUpgrade)
            {
                case Upgradables.Barrier:
                    initialStats.barrierDurability = (int)_currentItem.valuePerLevel[_currentItem.currentLevel];
                    ValidateForResurrectionUpgrade();
                    break;
                case Upgradables.MovementSpeed:
                    initialStats.movementSpeed = _currentItem.valuePerLevel[_currentItem.currentLevel];
                    break;
                case Upgradables.AerialMovement:
                    initialStats.aerialSpdReducer = _currentItem.valuePerLevel[_currentItem.currentLevel];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            UpdateStatsUI();
            infoUI.UpdateButtonsUI(_currentItem.maxLevelCount, _currentItem.currentLevel);
        }
        #endregion

        //ADD UNIQUE BEHAVIOR HERE FOR SPECIAL UPGRADES
        #region SPECIAL_UPRADE_BEHAVIORS
        private void ValidateForResurrectionUpgrade()
        {
            switch (_currentItem.currentLevel)
            {
                case < 6:
                    if (initialStats.canRez)
                    {
                        initialStats.canRez = false;
                    }
                    return;
                case 6:
                    initialStats.canRez = true;
                    Debug.Log("RESURRECTION UPGRADE UNLOCKED");
                    break;
            }
        }
        #endregion

        #region UI_CALLER
        private void UpdateStatsUI()
        {
            //Update Stats UI
            if (_currentItem.currentLevel == _currentItem.maxLevelCount)
            {
                infoUI.UpdateDetailsUI(null, 
                    _currentItem.valuePerLevel[_currentItem.currentLevel].ToString(), 
                    null, 
                    _currentItem.statSign, 
                    true);
            }
            else
            {
                infoUI.UpdateDetailsUI(_currentItem.costPerLevel[_currentItem.currentLevel + 1].ToString(), 
                    _currentItem.valuePerLevel[_currentItem.currentLevel].ToString(), 
                    _currentItem.valuePerLevel[_currentItem.currentLevel + 1].ToString(), 
                    _currentItem.statSign,
                    false);
            }
        }
        #endregion

        #region UPDATE_DATA
        private void UpdateLocalData()
        {
            LocalStorageEvents.OnSaveUpgradesData?.Invoke();
            LocalStorageEvents.OnSavePlayerStats?.Invoke();
        }
        #endregion
    }
}