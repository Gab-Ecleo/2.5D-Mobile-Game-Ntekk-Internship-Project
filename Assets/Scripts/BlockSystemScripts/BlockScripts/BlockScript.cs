﻿using System;
using BlockSystemScripts.BlockSpawnerScripts;
using UnityEngine;

namespace BlockSystemScripts.BlockScripts
{
    public class BlockScript : MonoBehaviour
    {
        #region VARIABLES
        [Header("Block States")]
        [SerializeField] private BlockType blockType;
        private BlockState _blockState;

        [Header("Block Fall Timer")]
        private BlockFallTimer _fallTimer;

        [Header("Test References. To be private")]
        [SerializeField] private GridCell currentCell;
        [SerializeField] private BlockSpawner blockSpawner;

        public BlockType BlockType => blockType;
        public BlockState BlockState => _blockState;
        public GridCell CurrentCell => currentCell;

        #endregion

        private void Awake()
        {
            if (gameObject.GetComponent<BlockFallTimer>() == null)
            {
                gameObject.AddComponent<BlockFallTimer>();
            }
            _fallTimer = GetComponent<BlockFallTimer>();
        }

        #region BLOCK_MANIPULATION
        //called by the block spawner or the player to initialize references for this block 
        public void InitializeReferences(GridCell cell, BlockSpawner spawnerReference)
        {
            currentCell = cell;
            currentCell.FillCellSlot(this);
            _fallTimer.StartTimer();
            blockSpawner = spawnerReference;

            _blockState = BlockState.Falling;
        }

        //moves the block down if the conditions are met
        public void TransferCell()
        {
            //If the block has already landed, stops the fall timer 
            //Tell block spawner that it can spawn a block again
            //Then call for row validation and column validation
            if (BlockStateChecker() == BlockState.Landed)
            {
                _fallTimer.StopTimer();
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
                _blockState = BlockState.CanPickUp;
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
                _fallTimer.StartTimer();
                _blockState = BlockState.Falling;
            }
        }
        
        //method for checking if there is block on top of this block, then trigger it's transfer timer.
        private void SignalTopBlock()
        {
            if (TopBlockDetection() == null) return;
            TopBlockDetection()._blockState = BlockState.Falling;
            TopBlockDetection()._fallTimer.StartTimer();
        }
        #endregion
        
        #region STATE_CHECKER
        //Determines if there is a block or a platform below
        private BlockState BlockStateChecker()
        {
            if (currentCell.NextCell == null)
            {
                _blockState = BlockState.Landed;
                return _blockState;
            }
            
            if (currentCell.NextCell.CurrentBlock != null)
            {
                //if the next cell's current block is a powerUp, destroy it and replace its position
                if (currentCell.NextCell.CurrentBlock.BlockType == BlockType.PowerUp)
                {
                    currentCell.NextCell.DestroyBlock();
                    _blockState = BlockState.Falling;
                    return _blockState;
                }
                
                _blockState = BlockState.Landed;
                return _blockState;
            }
            _blockState = BlockState.Falling;
            return _blockState;
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

        private void OnDisable()
        {
            currentCell.EmptyCellSlot();
            SignalTopBlock();
            blockSpawner.TriggerCanSpawn();
        }
    }
}