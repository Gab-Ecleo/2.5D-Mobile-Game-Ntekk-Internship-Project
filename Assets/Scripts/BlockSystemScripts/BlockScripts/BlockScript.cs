using System;
using BlockSystemScripts.BlockSpawnerScripts;
using UnityEngine;

namespace BlockSystemScripts.BlockScripts
{
    public class BlockScript : MonoBehaviour
    {
        [SerializeField] private GridCell currentCell, nextCell;
        [SerializeField] private BlockSpawner blockSpawner;
        
        [Header("Block Fall Timer")]
        [SerializeField] private BlockFallTimer fallTimer;

        [Header("Raycast References")] 
        [SerializeField] private Vector3 direction = -Vector3.up;
        [SerializeField] private float maxDistance = 0.6f;
        [SerializeField] private LayerMask landedLayer;
        
        private void FixedUpdate()
        {
            //draws a ray for the groundCheck raycast
            Debug.DrawRay(transform.position, direction * maxDistance, Color.yellow);
        }

        //moves the block down if the conditions are met
        [ContextMenu("Drop Block")]
        public void TransferCell()
        {
            //checks if the block has already landed
            if (IsLanded())
            {
                return;
            }

            //checks if nextCell has a value
            if (nextCell != null)
            {
                currentCell.EmptyCellSlot();
                currentCell = nextCell;
                currentCell.FillCellSlot(this);
                transform.position = currentCell.transform.position;
                //fallTimer.StartTimer();
            }

            //If the block has already landed, stops the fall timer 
            //Tell block spawner that it can spawn a block again
            //Then call for row validation
            if (IsLanded())
            {
                //fallTimer.StopTimer();
                blockSpawner.TriggerCanSpawn();
                currentCell.AssignedRow.ValidateRow();
            }
            
            //checks if the current cell is not yet the last one. If not, give nextCell a value. If it is, return null. 
            if (currentCell.RowNumber + 1 != currentCell.AssignedColumn.GridCells.Count)
            {
                nextCell = currentCell.AssignedColumn.GridCells[currentCell.RowNumber + 1].GetComponent<GridCell>();
                return;
            }
            nextCell = null;
        }

        public void InitializeReferences(GridCell cell, BlockSpawner spawnerReference)
        {
            currentCell = cell;
            currentCell.FillCellSlot(this);
            fallTimer.StartTimer();
            nextCell = currentCell.AssignedColumn.GridCells[currentCell.RowNumber + 1].GetComponent<GridCell>();
            blockSpawner = spawnerReference;
        }

        public bool IsLanded()
        {
            return Physics.Raycast(transform.position, direction, maxDistance, landedLayer);
        }
        
        private void OnDestroy()
        {
            blockSpawner.TriggerCanSpawn();
        }
    }
}