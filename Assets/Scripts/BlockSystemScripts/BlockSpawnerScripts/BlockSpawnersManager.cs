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

        private int _failCounter;
        private BlockSpawner _repeatingSpawner;
        
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
        private void TriggerBlockSpawn(bool isSpawnFailed)
        {
            if (isSpawnFailed)
            {
                _failCounter++;
                if (_failCounter >= blockSpawners.Count)
                {
                    Debug.Log("TOO MANY FAILURES LAH");
                    SpawnEvents.OnSpawnTimerReset?.Invoke();
                    _failCounter = 0;
                    return;
                }
            }
            var spawnerRandomValue = Random.Range(0, blockSpawners.Count);
            _repeatingSpawner = blockSpawners[spawnerRandomValue];
            blockSpawners[spawnerRandomValue].ValidateBlockSpawn();
        }

        private void ShuffleSpawnList()
        {
            blockSpawners.Remove(_repeatingSpawner);
            blockSpawners.Add(_repeatingSpawner);
        }

        #region OVERLOAD FOR SPECIFIC BLOCK SPAWNS
        //overload for specific block testing
        public void TriggerBlockSpawn(GameObject objToSpawn)
        {
            // if (isSpawnFailed)
            // {
            //     _failCounter++;
            //     if (_failCounter >= blockSpawners.Count)
            //     {
            //         Debug.Log("TOO MANY FAILURES LAH");
            //         SpawnEvents.OnSpawnTimerReset?.Invoke();
            //         _failCounter = 0;
            //         return;
            //     }
            // }
            var spawnerRandomValue = Random.Range(0, blockSpawners.Count);
            _repeatingSpawner = blockSpawners[spawnerRandomValue];
            blockSpawners[spawnerRandomValue].ValidateBlockSpawn(objToSpawn);
        }
        #endregion

        private void OnEnable()
        {
            SpawnEvents.OnSpawnTrigger += TriggerBlockSpawn;
            SpawnEvents.onSpawnerListShuffle += ShuffleSpawnList;
        }
        
        private void OnDisable()
        {
            SpawnEvents.OnSpawnTrigger -= TriggerBlockSpawn;
            SpawnEvents.onSpawnerListShuffle -= ShuffleSpawnList;
        }

        #region FOR DEBUG ONLY. TBD
        public void DebugTriggerBlockSpawn(bool isSpawnFailed)
        {
            if (isSpawnFailed)
            {
                _failCounter++;
                if (_failCounter >= blockSpawners.Count)
                {
                    Debug.Log("TOO MANY FAILURES LAH");
                    SpawnEvents.OnSpawnTimerReset?.Invoke();
                    return;
                }
            }
            var spawnerRandomValue = Random.Range(0, blockSpawners.Count);
            _repeatingSpawner = blockSpawners[spawnerRandomValue];
            blockSpawners[spawnerRandomValue].ValidateBlockSpawn();
        }
        #endregion
    }
}