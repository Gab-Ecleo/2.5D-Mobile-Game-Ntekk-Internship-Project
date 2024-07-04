using System;
using BlockSystemScripts.BlockSpawnerScripts;
using UnityEngine;

namespace BlockSystemScripts.BlockScripts
{
    public class BlockScript : MonoBehaviour
    {
        #region VARIABLES
        [Header("Block Type")]
        [SerializeField] private bool isHeavy;
        
        [Header("Block Fall Timer")]
        [SerializeField] private BlockFallTimer fallTimer;

        [Header("Test References. To be private")]
        [SerializeField] private GridCell currentCell;
        [SerializeField] private BlockSpawner blockSpawner;

        private bool _canPickUp;

        public bool CanPickUp => _canPickUp;

        public GridCell CurrentCell => currentCell;

        #endregion

        #region BLOCK_MANIPULATION
        //called by the block spawner or the player to initialize references for this block 
        public void InitializeReferences(GridCell cell, BlockSpawner spawnerReference)
        {
            currentCell = cell;
            currentCell.FillCellSlot(this);
            fallTimer.StartTimer();
            blockSpawner = spawnerReference;

            TogglePickUpState(false);
        }

        //moves the block down if the conditions are met
        public void TransferCell()
        {
            //If the block has already landed, stops the fall timer 
            //Tell block spawner that it can spawn a block again
            //Then call for row validation and column validation
            if (IsLanded())
            {
                fallTimer.StopTimer();
                blockSpawner.TriggerCanSpawn();
                //checks if the vertical/row position of this cell is the last row
                if (currentCell.RowIndex + 1 >= currentCell.AssignedColumn.GridCells.Count)
                {
                    currentCell.AssignedRow.ValidateRow();
                }
                //checks if this cell is the topmost cell
                if (currentCell.RowIndex == 0)
                {
                    currentCell.AssignedColumn.ValidateColumn();
                }
                TogglePickUpState(true);
                return;
            }
            //Signals the block above that it can fall down
            SignalTopBlock();
            
            //checks if nextCell has a value
            if (currentCell.NextCell != null)
            {
                currentCell.EmptyCellSlot();
                currentCell = currentCell.NextCell;
                currentCell.FillCellSlot(this);
                transform.position = currentCell.transform.position;
                fallTimer.StartTimer();
                TogglePickUpState(false);
            }
        }
        
        private void TogglePickUpState(bool nextState)
        {
            if (isHeavy) return;
            _canPickUp = nextState;
        }
        
        //method for checking if there is block on top of this block, then trigger it's transfer timer.
        private void SignalTopBlock()
        {
            if (TopBlockDetection() == null) return;
            TopBlockDetection().TogglePickUpState(false);
            TopBlockDetection().fallTimer.StartTimer();
        }
        #endregion
        
        #region RAYCAST_METHODS
        //Determines if there is a block or a platform below
        public bool IsLanded()
        {
            if (currentCell.NextCell == null)
            {
                return true;
            }
            if (currentCell.NextCell.CurrentBlock != null)
            {
                return true;
            }
            return false;
        }
        
        //Determines if there is a block above
        public BlockScript TopBlockDetection()
        {
            if (currentCell.PreviousCell == null)
            {
                return null;
            }
            if (currentCell.PreviousCell.CurrentBlock == null)
            {
                return null;
            }
            return currentCell.PreviousCell.CurrentBlock;
        }

        #endregion

        private void OnDestroy()
        {
            currentCell.EmptyCellSlot();
            SignalTopBlock();
            blockSpawner.TriggerCanSpawn();
        }

        private void OnDisable()
        {
            currentCell.EmptyCellSlot();
            blockSpawner.TriggerCanSpawn();
        }
    }
}