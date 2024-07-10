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

        [Header("Block Type Randomizer References")]
        [SerializeField][Range(1, 100)] private int mainBlockSpawnRate;
        
        [Header("Spawn Validation References. To be private")]
        [SerializeField] private bool canSpawn;
        
        private GameObject _repeatingMainBlock;
        
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
            //Will only be true if the spawner tries to spawn while there is a sudden block on the topmost cell. 
            if (GridCells[0].CurrentBlock != null)
            {
                canSpawn = false; 
                ValidateBlockSpawn();
                return;
            }
            var block = Instantiate(BlockToSpawn(), GridCells[0].gameObject.transform.position, quaternion.identity);
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
            
            var convertedSpawnRate = tempMaxValue * (mainBlockSpawnRate / 100f);

            //return default block type
            if (blockRangePass1 <= (int)convertedSpawnRate) return RandomizedMainBlock();
            //return heavy block
            if (blockRangePass1 > (int)convertedSpawnRate) return RandomizedSpecialBlock();
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