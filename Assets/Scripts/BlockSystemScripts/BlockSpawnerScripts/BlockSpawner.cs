using System;
using System.Collections.Generic;
using BlockSystemScripts.BlockScripts;
using BlockSystemScripts.RowAndColumnScripts;
using EventScripts;
using ScriptableData;
using UI.Hazard_Related;
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
        [Header("UI Elements")]
        [SerializeField] private GameObject imageToSpawn;
        private GameObject _imgObj;
        
        [Header("Serialized Object Data")]
        [SerializeField] private BlockSpawnerSO blockData;
        [SerializeField] private HazardSO hazardData;
        private GameObject _repeatingMainBlock;

        [Header("Spawn Validation References. To be private")]
        [SerializeField] private bool canSpawn;
        
        private void Awake()
        {
            InitializeData();
        }

        #region INITIALIZATIONS
        private void InitializeData()
        {
            //checks if the default rate is equal or more than 99
            if (blockData.defaultRate >= 99)
            {
                blockData.heavyRate = 0;
                //checks if the default rate is equal or more than 100
                if (blockData.defaultRate >=100)
                {
                    blockData.powerUpRate = 0;
                }
            }
            //checks if the heavy rate and block date are equals to 0 
            if (blockData.heavyRate <= 0 && blockData.powerUpRate <= 0)
            {
                blockData.defaultRate = 100;
                return;
            }
            //checks if the heavy rate is more than 0 and is less than the default rate
            if (blockData.heavyRate > 0 && blockData.heavyRate <= blockData.defaultRate)
            {
                blockData.heavyRate = blockData.defaultRate + ((100 - blockData.defaultRate) / 2);
            }
            //checks if the heavy rate is more than 0 and the power up rate is set to 0
            if (blockData.heavyRate > 0 && blockData.powerUpRate <= 0)
            {
                blockData.heavyRate = 100;
            }
            //checks if the power up rate is more than 0 
            if (blockData.powerUpRate > 0)
            {
                blockData.powerUpRate = 100;
                //checks if the heavy rate is already set to 100
                if (blockData.heavyRate >=100)
                {
                    blockData.powerUpRate = 0;
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
            //Will only be true if the spawner tries to spawn while there is a sudden block on the topmost cell. 
            if (GridCells[0].CurrentBlock != null)
            {
                if (GridCells[0].CurrentBlock.BlockType == BlockType.PowerUp)
                {
                    GridCells[0].DestroyBlock();
                    OnSpawnBlock();
                }
                canSpawn = false; 
                ValidateBlockSpawn();
                return;
            }
            OnSpawnBlock();
        }
        
        private void OnSpawnBlock()
        {
            #region Hazard_Checking_Behavior
            if (hazardData.IsBlackOutActive)
            {
                if (_imgObj == null)
                {
                    Camera mainCamera = Camera.main;
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(GridCells[0].transform.position);
                    var obj = Instantiate(imageToSpawn, screenPos, quaternion.identity, GameManager.Instance.FetchCanvas().transform);
                    _imgObj = obj;
                    _imgObj.GetComponent<Animator>().SetTrigger("BlinkTrigger");
                    _imgObj.GetComponent<BlackOutWarningTrigger>().InitializeImage();
                }
                else
                {
                    if (_imgObj.activeSelf)
                    {
                        Debug.Log("Spawner Warning is Already Active");
                    }
                    else
                    {
                        _imgObj.SetActive(true);
                        _imgObj.GetComponent<Animator>().SetTrigger("BlinkTrigger");
                    }
                }
            }
            #endregion
            
            GameObject block;
            block = Instantiate(BlockToSpawn(), GridCells[0].gameObject.transform.position, quaternion.identity);
            block.GetComponent<BlockScript>().InitializeReferences(GridCells[0], this);
            TriggerCannotSpawn();
        }

        #region BLOCK_RANDOMIZERS
        //returns a block prefab with the "type" depending on the random number generated. 
        private GameObject BlockToSpawn()
        {
            //checks if there is less than or equal to one type of block in the list
            if (blockData.specialBlockList.Count <= 0) return RandomizedMainBlock();
            
            var tempMaxValue = 100;
            var blockRangePass1 = Random.Range(1, tempMaxValue + 1);
            blockRangePass1 = Random.Range(1, tempMaxValue + 1);
            blockRangePass1 = Random.Range(1, tempMaxValue + 1);
            

            //return default block type
            if (blockRangePass1 <= blockData.defaultRate) return RandomizedMainBlock();
            //return heavy block
            if (blockRangePass1 > blockData.defaultRate && blockRangePass1 <= blockData.heavyRate) return RandomizedSpecialBlock();
            //return PU block
            if (blockRangePass1 > blockData.heavyRate && blockRangePass1 <= blockData.powerUpRate) return RandomizedPowerUpBlock();
            //returns default block if no conditions are met
            return RandomizedMainBlock();
        }
        
        //Randomizer for Main Block
        private GameObject RandomizedMainBlock()
        {
            if (blockData.mainBlockList.Count <= 1)
            {
                return blockData.mainBlockList[0];
            }
            var blockNumber = Random.Range(0, blockData.mainBlockList.Count);
            
            if (_repeatingMainBlock == null) // checks if the _repeatingMainBlock is null
            {
                _repeatingMainBlock = blockData.mainBlockList[blockNumber];
                return blockData.mainBlockList[blockNumber];
            }

            if (_repeatingMainBlock == blockData.mainBlockList[blockNumber]) // checks if the block to be spawned is the same as the last one
            {
                blockData.mainBlockList.Remove(_repeatingMainBlock);
                blockNumber = Random.Range(0, blockData.mainBlockList.Count);
                blockData.mainBlockList.Add(_repeatingMainBlock);
                _repeatingMainBlock = blockData.mainBlockList[blockNumber];
                return blockData.mainBlockList[blockNumber];
            }
            
            // proceeds here if the repeating block is not null and if it is not the same as the last one
            _repeatingMainBlock = blockData.mainBlockList[blockNumber];
            return blockData.mainBlockList[blockNumber];
        }
        
        //Randomizer for Special Block
        private GameObject RandomizedSpecialBlock()
        {
            if (blockData.specialBlockList.Count <= 1)
            {
                return blockData.specialBlockList[0];
            }
            var blockNumber = Random.Range(0, blockData.specialBlockList.Count);
            blockNumber = Random.Range(0, blockData.specialBlockList.Count);
            blockNumber = Random.Range(0, blockData.specialBlockList.Count);
            return blockData.specialBlockList[blockNumber];
        }
        
        private GameObject RandomizedPowerUpBlock()
        {
            if (blockData.powerUpBlockList.Count <= 1)
            {
                return blockData.powerUpBlockList[0];
            }
            var blockNumber = Random.Range(0, blockData.powerUpBlockList.Count);
            blockNumber = Random.Range(0,blockData.powerUpBlockList.Count);
            blockNumber = Random.Range(0, blockData.powerUpBlockList.Count);
            return blockData.powerUpBlockList[blockNumber];
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