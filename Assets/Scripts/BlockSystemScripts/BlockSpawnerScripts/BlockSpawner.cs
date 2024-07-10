using System;
using System.Collections.Generic;
using BlockSystemScripts.BlockScripts;
using BlockSystemScripts.RowAndColumnScripts;
using EventScripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlockSystemScripts.BlockSpawnerScripts
{
    /// <summary>
    /// Inheritor of AlignmentManager
    /// Handles the behavior of the block spawner
    /// </summary>
    public class BlockSpawner : AlignmentManager
    {
        [SerializeField] private List<GameObject> mainBlockList;
        [SerializeField] private List<GameObject> specialBlockList;
        [SerializeField] private List<GameObject> powerUpBlockList;

        [Header("Block Type Randomizer References")]
        [Tooltip("Value should be LOWER than heavy block & power up block")]
        [SerializeField][Range(1, 100)] private int defaultRate;
        [Tooltip("Value should be HIGHER than default block & LOWER than power up block. Set to 0 to keep heavy blocks from spawning")]
        [SerializeField][Range(0, 100)] private int heavyRate;
        [Tooltip("Value should be HIGHER than heavy block, unless heavy block is set to 0. Set to 0 to keep power up blocks from spawning")]
        [SerializeField][Range(0, 100)] private int powerUpRate;
        
        [Header("Spawn Validation References. To be private")]
        [SerializeField] private bool canSpawn;
        
        private GameObject _repeatingMainBlock;

        private void Awake()
        {
            InitializeSpawnRatesValues();
        }

        #region INITIALIZATION_METHODS
        private void InitializeSpawnRatesValues()
        {
            if (defaultRate <= 98)
            {
                if (heavyRate > 0 && heavyRate <= defaultRate)
                {
                    heavyRate = defaultRate + 1;
                }

                if (powerUpRate > 0 && heavyRate <= 99)
                {
                    if (heavyRate <= 0 && powerUpRate <= defaultRate)
                    {
                        powerUpRate = defaultRate + 1;
                        return;
                    }

                    if (powerUpRate <= heavyRate)
                    {
                        powerUpRate = heavyRate + 1;
                    }
                }
            }
        }
        

        #endregion

        //Called to validate the block spawn
        public void ValidateBlockSpawn()
        {
            //if there's still a moving block, find another spawner. Else, spawn a block and reset timer. 
            if (!canSpawn)
            {
                SpawnEvents.OnSpawnTrigger?.Invoke(true);
                SpawnEvents.onSpawnerListShuffle?.Invoke();
                return;
            }
            SpawnBlock();
            SpawnEvents.onSpawnerListShuffle?.Invoke();
            SpawnEvents.OnSpawnTimerReset?.Invoke();
        }

        //spawn a block and give it the proper references
        [ContextMenu("SPAWN BLOCK")]
        private void SpawnBlock()
        {
            GameObject block;
            //Will only be true if the spawner tries to spawn while there is a sudden block on the topmost cell. 
            if (GridCells[0].CurrentBlock != null)
            {
                if (GridCells[0].CurrentBlock.BlockType == BlockType.PowerUp)
                {
                    GridCells[0].DestroyBlock();
                    block = Instantiate(BlockToSpawn(), GridCells[0].gameObject.transform.position, quaternion.identity);
                    block.GetComponent<BlockScript>().InitializeReferences(GridCells[0], this);
                    TriggerCannotSpawn();
                }
                canSpawn = false; 
                ValidateBlockSpawn();
                return;
            }
            block = Instantiate(BlockToSpawn(), GridCells[0].gameObject.transform.position, quaternion.identity);
            block.GetComponent<BlockScript>().InitializeReferences(GridCells[0], this);
            TriggerCannotSpawn();
        }

        #region BLOCK_RANDOMIZERS
        //returns a block prefab with the "type" depending on the random number generated. 
        private GameObject BlockToSpawn()
        {
            //checks if there is less than or equal to one type of block in the list
            if (specialBlockList.Count <= 0) return RandomizedMainBlock();
            
            var tempMaxValue = 100;
            var blockRangePass1 = Random.Range(1, tempMaxValue + 1);
            blockRangePass1 = Random.Range(1, tempMaxValue + 1);
            blockRangePass1 = Random.Range(1, tempMaxValue + 1);
            

            //return default block type
            if (blockRangePass1 <= defaultRate) return RandomizedMainBlock();
            //return heavy block
            if (blockRangePass1 > defaultRate && blockRangePass1 <= heavyRate) return RandomizedSpecialBlock();
            //return PU block
            if (blockRangePass1 > heavyRate && blockRangePass1 <= powerUpRate) return RandomizedPowerUpBlock();
            //returns default block if no conditions are met
            return RandomizedMainBlock();
        }
        
        //Randomizer for Main Block
        private GameObject RandomizedMainBlock()
        {
            if (mainBlockList.Count <= 1)
            {
                return mainBlockList[0];
            }
            var blockNumber = Random.Range(0, mainBlockList.Count);
            
            if (_repeatingMainBlock == null) // checks if the _repeatingMainBlock is null
            {
                _repeatingMainBlock = mainBlockList[blockNumber];
                return mainBlockList[blockNumber];
            }

            if (_repeatingMainBlock == mainBlockList[blockNumber]) // checks if the block to be spawned is the same as the last one
            {
                mainBlockList.Remove(_repeatingMainBlock);
                blockNumber = Random.Range(0, mainBlockList.Count);
                mainBlockList.Add(_repeatingMainBlock);
                _repeatingMainBlock = mainBlockList[blockNumber];
                return mainBlockList[blockNumber];
            }
            
            // proceeds here if the repeating block is not null and if it is not the same as the last one
            _repeatingMainBlock = mainBlockList[blockNumber];
            return mainBlockList[blockNumber];
        }
        
        //Randomizer for Special Block
        private GameObject RandomizedSpecialBlock()
        {
            if (specialBlockList.Count <= 1)
            {
                return specialBlockList[0];
            }
            var blockNumber = Random.Range(0, specialBlockList.Count);
            blockNumber = Random.Range(0, specialBlockList.Count);
            blockNumber = Random.Range(0, specialBlockList.Count);
            return specialBlockList[blockNumber];
        }
        
        private GameObject RandomizedPowerUpBlock()
        {
            if (powerUpBlockList.Count <= 1)
            {
                return powerUpBlockList[0];
            }
            var blockNumber = Random.Range(0, powerUpBlockList.Count);
            blockNumber = Random.Range(0, powerUpBlockList.Count);
            blockNumber = Random.Range(0, powerUpBlockList.Count);
            return powerUpBlockList[blockNumber];
        }
        #endregion
        
        public void TriggerCanSpawn()
        {
            canSpawn = true;
        }

        public void TriggerCannotSpawn()
        {
            canSpawn = false;
        }
    }
}