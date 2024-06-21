using System;
using BlockSystemScripts.BlockScripts;
using BlockSystemScripts.RowAndColumnScripts;
using EventScripts;
using Unity.Mathematics;
using UnityEngine;

namespace BlockSystemScripts.BlockSpawnerScripts
{
    /// <summary>
    /// Inheritor of AlignmentManager
    /// Handles the behavior of the block spawner
    /// </summary>
    public class BlockSpawner : AlignmentManager
    {
        [SerializeField] private BlockSpawnersManager assignedSpawnersManager;

        [Header("Spawn Validation References")]
        [SerializeField] private int failCounter;
        [SerializeField] private bool canSpawn;

        [SerializeField] private GameObject blockPrefab;
        
        //assigns the spawn manager to this spawner.
        public void SetSpawnManager()
        {
            assignedSpawnersManager = transform.GetComponentInParent<BlockSpawnersManager>();
        }

        //Called to validate the block spawn
        public void ValidateBlockSpawn()
        {
            //if there's still a moving block, find another spawner. Else, spawn a block and reset timer. 
            if (!canSpawn)
            {
                failCounter++;
                assignedSpawnersManager.TriggerBlockSpawn(failCounter);
                // Debug.Log($"{gameObject.name} failed");
                return;
            }
            SpawnBlock();
            failCounter = 0;
            SpawnEvents.OnSpawnTimerReset?.Invoke();
        }

        //spawn a block and give it the proper references
        [ContextMenu("SPAWN BLOCK")]
        private void SpawnBlock()
        {
            var block = Instantiate(blockPrefab, GridCells[0].gameObject.transform.position, quaternion.identity);
            block.GetComponent<BlockScript>().InitializeReferences(GridCells[0], this);
            canSpawn = false;
        }

        public void TriggerCanSpawn()
        {
            canSpawn = true;
        }
    }
}