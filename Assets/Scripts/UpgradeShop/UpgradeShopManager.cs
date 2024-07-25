using System;
using System.Collections.Generic;
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
        [SerializeField] private GameObject containerPrefab;

        private void Start()
        {
            //destroy existing child
            foreach (Transform child in containerPrefab.transform)
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
                    var newItem = Instantiate(itemPrefab, containerPrefab.transform);

                    var newItemComponent = newItem.GetComponent<ItemGenerator>();

                    newItem.name = item.upgradeName;
                
                    newItemComponent.UpdateItemValues(item, id);
                }
            }
        }
    }
}