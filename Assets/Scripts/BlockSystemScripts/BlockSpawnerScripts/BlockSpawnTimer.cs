using EventScripts;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BlockSystemScripts.BlockSpawnerScripts
{
    public class BlockSpawnTimer : MonoBehaviour
    {
        [Header("Spawn Timer Data")] 
        [SerializeField] private float startingSpawnTime = 3f;
        [SerializeField] private float timeMinValue;
        [SerializeField] private float timeMaxValue;
        
        private bool _spawnTimerActive;
        
        [Header("Difficulty Timer Data")]
        [SerializeField] private float startingDifficultyTime;
        [SerializeField] private float spawnTimerDecrement;
        private bool _difficultyTimerActive;
        
        [Header("Active Timers")]
        [SerializeField] private float spawnTimeLeft;
        [SerializeField] private float difficultyTimeLeft;

        private void Start()
        {
            //sets the timers by the determined starting values
            spawnTimeLeft = startingSpawnTime;
            _spawnTimerActive = true;

            difficultyTimeLeft = startingDifficultyTime;
            _difficultyTimerActive = true;
        }

        private void Update()
        {
            UpdateSpawnTimer();
            UpdateDifficultyTimer();
        }

        #region SPAWN_TIMER
        //countdown of the spawn timer unless it's inactive. Reaching zero triggers the block spawn event
        private void UpdateSpawnTimer()
        {
            if (!_spawnTimerActive) return;
            if (spawnTimeLeft > 0)
            {
                spawnTimeLeft -= Time.deltaTime;
            }
            else
            {
                _spawnTimerActive = false;
                SpawnEvents.OnSpawnTrigger?.Invoke();
            }
        }

        //resets the spawn timer, giving it a random value and activating it again
        private void ResetSpawnTimer()
        {
            var randomTime = Random.Range(timeMinValue, timeMaxValue + 1);
            spawnTimeLeft = randomTime;
            _spawnTimerActive = true;
            
        }
        #endregion

        #region DECREMENT_TIMER
        //countdown of the difficulty decrement unless it's inactive. Triggers the decrement of the spawn max timer
        private void UpdateDifficultyTimer()
        {
            if (!_difficultyTimerActive) return;
            if (difficultyTimeLeft > 0)
            {
                difficultyTimeLeft -= Time.deltaTime;
            }
            else
            {
                _difficultyTimerActive = false;
                DecrementSpawnTimer();
            }
        }
        
        //Decrements the spawn timer
        private void DecrementSpawnTimer()
        {
            timeMaxValue -= spawnTimerDecrement;
            difficultyTimeLeft = startingDifficultyTime;
            _difficultyTimerActive = true;
        }
        

        #endregion
        
        private void OnEnable()
        {
            SpawnEvents.OnSpawnTimerReset += ResetSpawnTimer;
        }

        private void OnDisable()
        {
            SpawnEvents.OnSpawnTimerReset -= ResetSpawnTimer;
        }
    }
}