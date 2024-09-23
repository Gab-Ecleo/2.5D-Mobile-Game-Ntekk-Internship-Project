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
        
        [Header("Prefab references")]
        [SerializeField] private GameObject itemPrefab;
        
        [Header("Content Container")]
        [SerializeField] private GameObject contentList;

        [Header("TO BE PRIVATE")]
        [SerializeField]private int _currentPage = 0;

        private void Start()
        {
            GenerateItems(0);
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
        }

        public void GenerateItems(int currentPage)
        {
            //destroy existing child
            // foreach (Transform child in contentList.transform)
            // {
            //     Destroy(child.gameObject);
            // }

            foreach (var id in itemIdentifiers)
            {
                if (itemList.items[currentPage].identifier != id.identifier)
                {
                    continue;
                }

                //var newItem = Instantiate(itemPrefab, contentList.transform);
                //var newItemComponent = newItem.GetComponent<ItemGenerator>();
                itemGenerator.name = itemList.items[currentPage].upgradeName;
                itemGenerator.UpdateItemValues(itemList.items[currentPage], id);
            }
        }
    }
}