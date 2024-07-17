using System;
using System.Collections.Generic;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace UpgradeShop
{
    public class UpgradeShopManager : MonoBehaviour
    {
        [SerializeField] private UpgradeItemsList itemList;
        [SerializeField] private List<UpgradeItemIdentifier> itemIdentifiers;

        [Header("Prefab references")]
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject containerPrefab;

        private void Start()
        {
            foreach (Transform child in containerPrefab.transform)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var item in itemList.Items)
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