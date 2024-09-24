using System;
using System.Collections.Generic;
using System.Linq;
using EventScripts;
using SaveSystem.Storage;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace UpgradeShop
{
    //Handles the generation of each item in the Upgrade Shop
    public class UpgradeShopManager : MonoBehaviour
    {
        [SerializeField] private UpgradeItemsList itemList;
        [SerializeField] private List<UpgradeItemIdentifier> itemIdentifiers;
        [SerializeField] private ItemGenerator itemGenerator;

        [Header("Upgrade UI Reference")] 
        [SerializeField] private UpgradeShopUIManager upgradeUI;

        [Header("TO BE PRIVATE")]
        [SerializeField]private int _currentPage = 0;

        private void Start()
        {
            GenerateItems(0);
            upgradeUI.InitializeIntList(itemList);
        }

        public void TurnPage(int addend)
        {
            _currentPage += addend;

            #region PAGE_CALCULATION
            if (_currentPage < 0)
            {
                _currentPage = itemList.items.Count - 1;
            }
            else if (_currentPage > itemList.items.Count - 1)
            {
                _currentPage = 0;
            }
            #endregion
            
            GenerateItems(_currentPage);
            upgradeUI.SyncPagePosition(_currentPage);
        }

        public void GenerateItems(int currentPage)
        {
            foreach (var id in itemIdentifiers)
            {
                if (itemList.items[currentPage].identifier != id.identifier)
                {
                    continue;
                }
                
                itemGenerator.name = itemList.items[currentPage].upgradeName;
                itemGenerator.UpdateItemValues(itemList.items[currentPage], id);
            }
        }
    }
}