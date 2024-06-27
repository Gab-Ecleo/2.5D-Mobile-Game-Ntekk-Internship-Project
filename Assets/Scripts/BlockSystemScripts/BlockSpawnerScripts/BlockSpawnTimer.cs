using EventScripts;
using UnityEngine;
using Random = UnityEngine.Random;


namespace BlockSystemScripts.BlockSpawnerScripts
{
    public class BlockSpawnTimer : MonoBehaviour
    {
        #region VARIABLES
        [Header("Spawn Timer Data")] 
        [SerializeField] private float startingSpawnTime = 3f;
        [SerializeField] private float timeMinValue;
        [SerializeField] private float timeMaxValue;
        
        private bool _spawnTimerActive;
        
        [Header("Difficulty Timer Data")]
        [SerializeField] private float initialDifficultyCountdown;
        [SerializeField] private float maxSpawnTimerDecrement;
        [SerializeField] private float minSpawnTimerDecrement;
        
        private bool _difficultyTimerActive;
        
        [Header("Active Timers. To be private")]
        [SerializeField] private float spawnTimeLeft;
        [SerializeField] private float difficultyTimeLeft;
        #endregion

        private void Start()
        {
            //sets the timers by the determined starting values
            spawnTimeLeft = startingSpawnTime;
            _spawnTimerActive = true;

            difficultyTimeLeft = initialDifficultyCountdown;
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
            //If timer is not activated, ignore everything els
            if (!_spawnTimerActive) return;
            //If timer is more than zero, reduce timer. 
            if (spawnTimeLeft > 0)
            {
                spawnTimeLeft -= Time.deltaTime;
            }
            //If timer is equal to or less than zero, deactivate the timer, and trigger a block spawn
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
            if (maxSpawnTimerDecrement <= 0 && minSpawnTimerDecrement <= 0) return;
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
        
        //Decrements the spawn timer values
        private void DecrementSpawnTimer()
        {
            if (minSpawnTimerDecrement >= 0)
            {
                if (timeMinValue > 1)
                {
                    timeMinValue -= minSpawnTimerDecrement;
                    //checks if the value is lower than the minimum threshold, then force it to the minimum desired value
                    if (timeMinValue < 1)
                    {
                        timeMinValue = 1;
                    }
                }
            }
            
            if (maxSpawnTimerDecrement >= 0)
            {
                if (timeMaxValue > timeMinValue + 1)
                {
                    timeMaxValue -= maxSpawnTimerDecrement;
                    //checks if the value is lower than the minimum threshold, then force it to the minimum desired value
                    if (timeMaxValue < timeMinValue + 1)
                    {
                        timeMaxValue = timeMinValue + 1;
                    }
                }
            }
            
            difficultyTimeLeft = initialDifficultyCountdown;
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