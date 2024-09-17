using EventScripts;
using ScriptableData;
using UnityEngine;
using Random = UnityEngine.Random;
using UI;
using UnityEngine.UI;
using TMPro;


namespace BlockSystemScripts.BlockSpawnerScripts
{
    public class BlockSpawnTimer : MonoBehaviour
    {
        #region VARIABLES
        [Header("Spawn Timer Data")] 
        [SerializeField][Range(3,20)] private float startingSpawnTime = 3f;
        [SerializeField][Range(1,20)] private float timeMinValue;
        [SerializeField][Range(2,20)] private float timeMaxValue;
        
        private bool _spawnTimerActive;
        
        [Header("Difficulty Timer Data")]
        [SerializeField][Range(0,50)] private float difficultyCountdown;
        [SerializeField][Range(0,10)] private float minDecrement;
        [SerializeField][Range(0,10)] private float maxDecrement;
        
        private bool _difficultyTimerActive;

        [Header("Pause Data")] 
        [SerializeField] private PauseSO pauseSo;
        
        [Header("Active Timers. To be private")]
        [SerializeField] private float spawnTimeLeft;
        [SerializeField] private float difficultyTimeLeft;
        #endregion

        private void Awake()
        {
            //sets the timers by the determined starting values
            spawnTimeLeft = startingSpawnTime;
            _spawnTimerActive = true;

            difficultyTimeLeft = difficultyCountdown;
            _difficultyTimerActive = true;
        }

        private void Update()
        {
            if (pauseSo.isPaused) return;
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
                SpawnEvents.OnSpawnTrigger?.Invoke(false);
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
            if (difficultyCountdown <= 0) return;
            if (maxDecrement <= 0 && minDecrement <= 0) return;
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
            if (minDecrement >= 0)
            {
                if (timeMinValue > 1)
                {
                    timeMinValue -= minDecrement;
                    //checks if the value is lower than the minimum threshold, then force it to the minimum desired value
                    if (timeMinValue < 1)
                    {
                        timeMinValue = 1;
                    }
                }
            }
            
            if (maxDecrement >= 0)
            {
                if (timeMaxValue > timeMinValue + 1)
                {
                    timeMaxValue -= maxDecrement;
                    //checks if the value is lower than the minimum threshold, then force it to the minimum desired value
                    if (timeMaxValue < timeMinValue + 1)
                    {
                        timeMaxValue = timeMinValue + 1;
                    }
                }
            }
            
            difficultyTimeLeft = difficultyCountdown;
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

        #region FOR DEBUG ONLY. TBD
        public void DebugSpawnTimer(Slider spawnTimer)
        {
            timeMinValue = spawnTimer.value;
            timeMaxValue = spawnTimer.value;
            ResetSpawnTimer();
        }

        public void DebugSpawnTimerNumber(TMP_Text tmpText)
        {
            tmpText.text = timeMaxValue.ToString();
        }
        #endregion
    }
}