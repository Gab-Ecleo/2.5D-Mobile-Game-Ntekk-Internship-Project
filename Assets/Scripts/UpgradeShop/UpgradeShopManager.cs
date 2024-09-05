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
        
        [Header("Prefab references")]
        [SerializeField] private GameObject itemPrefab;
        
        [Header("Content Container")]
        [SerializeField] private GameObject contentList;

        private void Start()
        {
            GenerateItems();
        }

        public void GenerateItems()
        {
            //destroy existing child
            foreach (Transform child in contentList.transform)
            {
                Destroy(child.gameObject);
            }
            
            //generate new items and provide their proper references
            foreach (var item in itemList.items)
            {
                foreach (var id in itemIdentifiers)
                {
                    if (item.identifier != id.identifier)
                    {
                        continue;
                    }
                    var newItem = Instantiate(itemPrefab, contentList.transform);

                    var newItemComponent = newItem.GetComponent<ItemGenerator>();

                    newItem.name = item.upgradeName;
                
                    newItemComponent.UpdateItemValues(item, id);
                }
            }
        }
    }
}