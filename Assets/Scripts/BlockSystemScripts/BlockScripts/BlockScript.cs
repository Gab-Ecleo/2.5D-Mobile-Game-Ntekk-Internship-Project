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

        [Header("Bottom Raycast References")] 
        [SerializeField] private Vector3 directionBot = -Vector3.up;
        [SerializeField] private float maxDistanceBot = 0.6f;
        [SerializeField] private LayerMask landedLayerBot;
        
        [Header("Top Raycast References")] 
        [SerializeField] private Vector3 directionTop = Vector3.up;
        [SerializeField] private float maxDistanceTop = 0.6f;
        [SerializeField] private LayerMask landedLayerTop;
        private RaycastHit _hit1;
        
        [Header("Test References. To be private")]
        [SerializeField] private GridCell currentCell, nextCell;
        [SerializeField] private BlockSpawner blockSpawner;

        private bool _canPickUp;

        public bool CanPickUp => _canPickUp;

        public GridCell CurrentCell => currentCell;

        #endregion
        
        //draws a ray for the raycast. CAN DELETE AFTER TESTING
        private void FixedUpdate()
        {
            Debug.DrawRay(transform.position, directionBot * maxDistanceBot, Color.red);
            Debug.DrawRay(transform.position, directionTop * maxDistanceTop, Color.green);
        }

        #region BLOCK_MANIPULATION
        //called by the block spawner or the player to initialize references for this block 
        public void InitializeReferences(GridCell cell, BlockSpawner spawnerReference)
        {
            currentCell = cell;
            currentCell.FillCellSlot(this);
            fallTimer.StartTimer();
            nextCell = currentCell.AssignedColumn.GridCells[currentCell.RowIndex + 1].GetComponent<GridCell>();
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
            if (nextCell != null)
            {
                currentCell.EmptyCellSlot();
                currentCell = nextCell;
                currentCell.FillCellSlot(this);
                transform.position = currentCell.transform.position;
                fallTimer.StartTimer();

                TogglePickUpState(false);
            }
            
            //checks if the current cell is not yet the last one. If not, give nextCell a value. If it is, return null. 
            if (currentCell.RowIndex + 1 != currentCell.AssignedColumn.GridCells.Count)
            {
                nextCell = currentCell.AssignedColumn.GridCells[currentCell.RowIndex + 1].GetComponent<GridCell>();
                return;
            }
            nextCell = null;
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
        //casts a raycast, determining if there is a block or a platform below
        public bool IsLanded()
        {
            return Physics.Raycast(transform.position, directionBot, maxDistanceBot, landedLayerBot);
        }
        
        //casts a raycast, determining if there is a block above
        public BlockScript TopBlockDetection()
        {
            Physics.Raycast(transform.position, directionTop, out _hit1,maxDistanceTop,  landedLayerTop);
            if (_hit1.collider == null)
            {
                return null;
            }
            return _hit1.collider.gameObject.GetComponent<BlockScript>();
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