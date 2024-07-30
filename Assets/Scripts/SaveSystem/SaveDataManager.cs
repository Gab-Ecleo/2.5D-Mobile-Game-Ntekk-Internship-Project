using System;
using EventScripts;
using SaveSystem.Storage;
using ScriptableData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class SaveDataManager : MonoBehaviour
    {
        private static SaveDataManager _instance;
        public static SaveDataManager Instance => _instance;

        [SerializeField] private PlayerStatsSO initialPlayerStats;
        [SerializeField] private UpgradeItemsList itemList;
        
        private void Awake()
        {
            Debug.Log("awoken");
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            
            InitializeFiles();
        }
        public void TestSceneTransfer()
        {
            SceneManager.LoadScene("BlockRelatedPowerUpTest");
        }

        private void InitializeFiles()
        {
            new UpgradeStorage().CreateUpgradeData();
            new StatStorages().CreateStatData();
        }

        private void LoadPlayerStats()
        {
            if (new StatStorages().GetStatData() == null) return;
            initialPlayerStats.coins = new StatStorages().GetStatData().coins;

            initialPlayerStats.movementSpeed = new StatStorages().GetStatData().movementSpeed;
            initialPlayerStats.aerialSpdReducer = new StatStorages().GetStatData().aerialSpdReducer;
            
            initialPlayerStats.jumpHeight = new StatStorages().GetStatData().jumpHeight;

            initialPlayerStats.barrierDurability = new StatStorages().GetStatData().barrierDurability;
            initialPlayerStats.canRez = new StatStorages().GetStatData().canRez;
        }

        private void SavePlayerStats()
        {
            var tempStats = new StatData
            {
                coins = initialPlayerStats.coins,
                movementSpeed = initialPlayerStats.movementSpeed,
                aerialSpdReducer = initialPlayerStats.aerialSpdReducer,
                jumpHeight = initialPlayerStats.jumpHeight,
                barrierDurability = initialPlayerStats.barrierDurability,
                canRez = initialPlayerStats.canRez
            };
            new StatStorages().SaveStatData(tempStats);
        }

        private void LoadUpgrade()
        {
            //called to update in-game data from the local storage
            if (new UpgradeStorage().GetUpgradeData() == null) return;
            foreach (var dataItem in new UpgradeStorage().GetUpgradeData().items)
            {
                foreach (var item in itemList.items)
                {
                    if (dataItem.identifier == item.identifier)
                    {
                        item.currentLevel = dataItem.currentLevel;
                    }
                }
            }
        }
        
        private void SaveUpgrade()
        {
            var tempUpgrades = new UpgradeData
            {
                items = itemList.items
            };
            new UpgradeStorage().SaveUpgradeData(tempUpgrades);
        }

        private void OnEnable()
        {
            LocalStorageEvents.OnLoadPlayerStats += LoadPlayerStats;
            LocalStorageEvents.OnSavePlayerStats += SavePlayerStats;
            LocalStorageEvents.OnSaveUpgradesData += SaveUpgrade;
            LocalStorageEvents.OnLoadUpgradeData += LoadUpgrade;
        }

        private void OnDisable()
        {
            LocalStorageEvents.OnLoadPlayerStats -= LoadPlayerStats;
            LocalStorageEvents.OnSavePlayerStats -= SavePlayerStats;
            LocalStorageEvents.OnSaveUpgradesData -= SaveUpgrade;
            LocalStorageEvents.OnLoadUpgradeData -= LoadUpgrade;
        }
    }
}