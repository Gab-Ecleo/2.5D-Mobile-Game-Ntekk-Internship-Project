using System;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

namespace UpgradeShop.ItemLevels
{
    public class ItemLevelManager : MonoBehaviour
    {
        [SerializeField] private List<ItemLevel> itemLevels;
        private UpgradeItem _currentItem;
        private UpgradeItemIdentifier _id;
        private int _levelCount;

        public void AddLevelSlotToList(ItemLevel itemLevel)
        {
            itemLevels.Add(itemLevel);
        }

        public void RenderList(UpgradeItem item, UpgradeItemIdentifier identifier)
        {
            _currentItem = item;
            _id = identifier;

            for (var index = 0; index < itemLevels.Count; index++)
            {
                var level = itemLevels[index];
                if (index <= _currentItem.currentLevel - 1 && _currentItem.currentLevel > 0)
                {
                    level.UpgradeSlot();
                }
                else if (index >= _currentItem.currentLevel - 1)
                {
                    level.DegradeSlot();
                }
            }
        }

        public void AddLevel()
        {
            for (var index = 0; index < itemLevels.Count; index++)
            {
                var level = itemLevels[index];
                switch (level.ItemLevelState)
                {
                    case LevelState.Upgraded:
                        continue;
                    
                    case LevelState.NotUpgraded:
                        level.UpgradeSlot();
                        _currentItem.currentLevel++;
                        return;
                        
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void ReduceLevel()
        {
            for (var index = itemLevels.Count - 1; index > -1; index--)
            {
                var level = itemLevels[index];
                switch (level.ItemLevelState)
                {
                    case LevelState.NotUpgraded:
                        continue;
                    
                    case LevelState.Upgraded:
                        level.DegradeSlot();
                        _currentItem.currentLevel--;
                        return;
                        
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}