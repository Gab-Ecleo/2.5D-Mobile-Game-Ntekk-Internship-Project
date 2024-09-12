using System;
using BlockSystemScripts.BlockSpawnerScripts;
using EventScripts;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace BlockSystemScripts.BlockScripts
{
    /// <summary>
    /// Timer for block falling
    /// </summary>
    public class BlockFallTimer : MonoBehaviour
    {
        [Header("Script References")]
        private BlockScript _blockScript;
        
        [Header("Timer Data")] 
        [SerializeField] private BlockTimerSO blockSpawnTimerData;
        private bool _isTimerActivated;
        
        [Header("Active Timers. To be private")]
        [SerializeField] private float timeLeft;

        private void Awake()
        {
            _blockScript = GetComponent<BlockScript>();
        }

        private void Start()
        {
            timeLeft = blockSpawnTimerData.blockTimerState == BlockTimerState.Normal ? blockSpawnTimerData.initialTimer : blockSpawnTimerData.slowedTimer;
        }

        private void Update()
        {
            UpdateTimer();
        }

        #region TIMER_METHODS
        private void UpdateTimer()
        {
            //If timer is not activated, ignore everything els
            if (!_isTimerActivated) return;
            //If timer is more than zero, reduce timer. 

            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            //If timer is equal to or less than zero, deactivate the timer, and move down the block
            else
            {
                StopTimer();
                _blockScript.TransferCell();
            }
        }
        
        //Method to activate the timer
        public void StartTimer()
        {
            timeLeft = blockSpawnTimerData.blockTimerState == BlockTimerState.Normal ? blockSpawnTimerData.initialTimer : blockSpawnTimerData.slowedTimer;
            _isTimerActivated = true;
        }
        
        //method to deactivate the timer
        public void StopTimer()
        {
            _isTimerActivated = false;
        }

        private void SyncTimer()
        {
            timeLeft = blockSpawnTimerData.blockTimerState == BlockTimerState.Normal ? blockSpawnTimerData.initialTimer : blockSpawnTimerData.slowedTimer;
        }
        #endregion

        private void OnEnable()
        {
            BlockEvents.OnSyncBlockFallTimers += SyncTimer;
        }

        private void OnDisable()
        {
            BlockEvents.OnSyncBlockFallTimers -= SyncTimer;
        }
    }
}