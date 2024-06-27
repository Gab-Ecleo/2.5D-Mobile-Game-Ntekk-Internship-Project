using System.Collections.Generic;
using EventScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlockSystemScripts.BlockSpawnerScripts
{
    /// <summary>
    /// Handles the list of block spawners.
    /// </summary>
    public class BlockSpawnersManager : MonoBehaviour
    {
        [Header("Test References. To be private")]
        [SerializeField] private List<BlockSpawner> blockSpawners;
        private BlockSpawnTimer _spawnTimer;

        //initialize the value of the spawn timer. 
        private void Start()
        {
            _spawnTimer = GetComponent<BlockSpawnTimer>();
        }
        
        #region LIST_INITIALIZATION
        //Called by the GridManager to add a spawner to this manager's list
        public void AddSpawnersToList(BlockSpawner item)
        {
            blockSpawners.Add(item);
        }
        
        //Called by the GridManager to clear this list before adding new items
        public void ClearList()
        {
            blockSpawners.Clear();
        }
        #endregion
        
        //An event triggered by the timer. Triggers the block spawning
        [ContextMenu("SPAWN BLOCK")]
        public void TriggerBlockSpawn()
        {
            var spawnerRandomValue = Random.Range(0, blockSpawners.Count);
            blockSpawners[spawnerRandomValue].ValidateBlockSpawn();
        }
        
        //An overload method that is triggered when the spawner cannot spawn a block
        public void TriggerBlockSpawn(int failCounter)
        {
            if (failCounter >= blockSpawners.Count)
            {
                SpawnEvents.OnSpawnTimerReset?.Invoke();
                return;
            }
            var spawnerRandomValue = Random.Range(0, blockSpawners.Count);
            blockSpawners[spawnerRandomValue].ValidateBlockSpawn();
        }

        private void OnEnable()
        {
            SpawnEvents.OnSpawnTrigger += TriggerBlockSpawn;
        }
        
        private void OnDisable()
        {
            SpawnEvents.OnSpawnTrigger -= TriggerBlockSpawn;
        }
    }
}