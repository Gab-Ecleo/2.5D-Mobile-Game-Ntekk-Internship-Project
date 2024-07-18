using System;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

namespace UpgradeShop.ItemLevels
{
    public class ItemLevelManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO initialStats;
        
        [SerializeField]private List<ItemLevel> levelSlots;

        private Upgradables _statToUpgrade;
        
        private UpgradeItem _currentItem;
        private UpgradeItemIdentifier _id;
        
        private int _levelCount;

        public void AddLevelSlotToList(ItemLevel itemLevel)
        {
            levelSlots.Add(itemLevel);
        }

        public void RenderList(UpgradeItem item, UpgradeItemIdentifier identifier)
        {
            _currentItem = item;
            _id = identifier;

            _statToUpgrade = _currentItem.affectedStat;

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

        #region LEVEL_MANIPULATION
        public void AddLevel()
        {
            for (var index = 0; index < levelSlots.Count; index++)
            {
                var level = levelSlots[index];
                switch (level.ItemLevelState)
                {
                    case LevelState.Upgraded:
                        continue;
                    
                    case LevelState.NotUpgraded:
                        level.UpgradeSlot();
                        _currentItem.currentLevel++;
                        UpdateStats();
                        return;
                        
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void ReduceLevel()
        {
            for (var index = levelSlots.Count - 1; index > -1; index--)
            {
                var level = levelSlots[index];
                switch (level.ItemLevelState)
                {
                    case LevelState.NotUpgraded:
                        continue;
                    
                    case LevelState.Upgraded:
                        level.DegradeSlot();
                        _currentItem.currentLevel--;
                        UpdateStats();
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
        }
        #endregion

        #region SPECIAL_UPRADE_BEHAVIORS
        private void ValidateForResurrectionUpgrade()
        {
            Debug.Log(_currentItem.currentLevel >= 6 ? "RESURRECTION UPGRADE UNLOCKED!" : "NO RESURRECTION YET!");
        }
        #endregion
        
    }
}