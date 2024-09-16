using System;
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
        private PlacementScoreState _placementScoreState;

        [Header("Block Fall Timer")]
        private BlockFallTimer _fallTimer;

        [Header("Test References. To be private")]
        [SerializeField] private GridCell currentCell;
        [SerializeField] private BlockSpawner blockSpawner;

        public BlockType BlockType => blockType;
        public BlockState BlockState => _blockState;
        public PlacementScoreState PlacementScoreState => _placementScoreState;
        public GridCell CurrentCell => currentCell;

        #endregion

        private void Awake()
        {
            if (gameObject.GetComponent<BlockFallTimer>() == null)
            {
                gameObject.AddComponent<BlockFallTimer>();
            }
            _fallTimer = GetComponent<BlockFallTimer>();
            _placementScoreState = PlacementScoreState.ScoreNotYetTriggered;
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
                CheckNeighbors();
                return;
            }
            //Signals the block above that it can fall down
            SignalTopBlock();
            
            //checks if nextCell has a value
            if (currentCell.NextYCell != null)
            {
                currentCell.EmptyCellSlot();
                currentCell = currentCell.NextYCell;
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
            if (currentCell.NextYCell == null)
            {
                _blockState = BlockState.Landed;
                return _blockState;
            }
            
            if (currentCell.NextYCell.CurrentBlock != null)
            {
                //if the next cell's current block is a powerUp, destroy it and replace its position
                if (currentCell.NextYCell.CurrentBlock.BlockType == BlockType.PowerUp)
                {
                    currentCell.NextYCell.DestroyBlock();
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
            if (currentCell.PreviousYCell == null)
            {
                return null;
            }
            if (currentCell.PreviousYCell.CurrentBlock == null)
            {
                return null;
            }
            return currentCell.PreviousYCell.CurrentBlock;
        }

        #endregion

        #region BLOCK EVENTS
        //Checks if the left and right cells have the same color as this block
        private void CheckNeighbors()
        {
            if (_placementScoreState == PlacementScoreState.ScoreTriggered) return;

            //Left Cell checker
            if (currentCell.PreviousXCell != null)
            {
                if (currentCell.PreviousXCell.CurrentBlock != null)
                {
                    var previousXBlock = currentCell.PreviousXCell.CurrentBlock;
                    if (previousXBlock.BlockState == BlockState.CanPickUp && previousXBlock.BlockType == blockType)
                    {
                        Debug.Log("Previous Block has the same color. Add Points!");
                        _placementScoreState = PlacementScoreState.ScoreTriggered;
                    }
                }
            }
            
            //Right Cell checker
            if (currentCell.NextXCell != null)
            {
                if (currentCell.NextXCell.CurrentBlock != null)
                {
                    var nextXBlock = currentCell.NextXCell.CurrentBlock;
                    if (nextXBlock.BlockState == BlockState.CanPickUp && nextXBlock.BlockType == blockType)
                    {
                        Debug.Log("Next Block has the same color. Add Points!");
                        _placementScoreState = PlacementScoreState.ScoreTriggered;
                    }
                }
            }
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