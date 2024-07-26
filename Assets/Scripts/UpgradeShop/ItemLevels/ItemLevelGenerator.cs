using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

namespace UpgradeShop.ItemLevels
{
    //Generates and initializes the values of each cell/level for an item
    public class ItemLevelGenerator : MonoBehaviour
    {
        [Header("Grid Cell Data")]
        [SerializeField] private GameObject gridCellParent; //Parent game object for the grid cells
        [SerializeField] private GameObject gridCellPrefab; //Prefab to be referenced for each grid cells

        [Header("Script References")]
        [SerializeField]private ItemLevelManager itemLevelManager;

        public void GenerateUpgradeCells(UpgradeItem item, UpgradeItemIdentifier identifier)
        {
            //destroy existing child
            foreach (Transform child in gridCellParent.transform)
            {
                Destroy(child.gameObject);
            }
            
            //generate new level slots/cells for this item
            for (var currentCellCount = 0; currentCellCount < item.maxLevelCount; currentCellCount++)
            {
                var cell = Instantiate(gridCellPrefab, gridCellParent.transform);
                
                var cellScript = cell.GetComponent<ItemLevel>();
                
                itemLevelManager.AddLevelSlotToList(cellScript);
            }
            
            itemLevelManager.RenderList(item, identifier);
            
        }
    }
}