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
            #region Singleton
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            #endregion
            
            InitializeFiles();
        }

        private void InitializeFiles()
        {
            new UpgradeStorage().CreateUpgradeData();
            new StatStorages().CreateStatData();
            new CurrencyStorage().CreateCurrencyData();
        }

        private void SaveAll(Scene current, Scene next)
        {
            SavePlayerStats();
            SaveCurrencyData();
            SaveUpgrade();
        }
        

        #region PLAYER_STATS
        private void LoadPlayerStats()
        {
            if (new StatStorages().GetStatData() == null) return;
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
                movementSpeed = initialPlayerStats.movementSpeed,
                aerialSpdReducer = initialPlayerStats.aerialSpdReducer,
                jumpHeight = initialPlayerStats.jumpHeight,
                barrierDurability = initialPlayerStats.barrierDurability,
                canRez = initialPlayerStats.canRez
            };
            new StatStorages().SaveStatData(tempStats);
        }
        #endregion

        #region UPGRADES
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
        #endregion

        #region CURRENCY
        private void LoadCurrencyData()
        {
            if (new CurrencyStorage().GetCurrencyData() == null) return;
            initialPlayerStats.coins = new CurrencyStorage().GetCurrencyData().coins;
        }
        
        private void SaveCurrencyData()
        {
            var tempCurrency = new CurrencyData { coins = initialPlayerStats.coins};
            new CurrencyStorage().SaveCurrencyData(tempCurrency);
        }
        #endregion

        private void OnEnable()
        {
            LocalStorageEvents.OnLoadPlayerStats += LoadPlayerStats;
            LocalStorageEvents.OnSavePlayerStats += SavePlayerStats;
            
            LocalStorageEvents.OnSaveUpgradesData += SaveUpgrade;
            LocalStorageEvents.OnLoadUpgradeData += LoadUpgrade;

            LocalStorageEvents.OnLoadCurrencyData += LoadCurrencyData;
            LocalStorageEvents.OnSaveCurrencyData += SaveCurrencyData;

            SceneManager.activeSceneChanged += SaveAll;
        }

        private void OnDisable()
        {
            LocalStorageEvents.OnLoadPlayerStats -= LoadPlayerStats;
            LocalStorageEvents.OnSavePlayerStats -= SavePlayerStats;
            
            LocalStorageEvents.OnSaveUpgradesData -= SaveUpgrade;
            LocalStorageEvents.OnLoadUpgradeData -= LoadUpgrade;
            
            LocalStorageEvents.OnLoadCurrencyData -= LoadCurrencyData;
            LocalStorageEvents.OnSaveCurrencyData -= SaveCurrencyData;
            SceneManager.activeSceneChanged += SaveAll;
        }
        
        private void OnApplicationQuit()
        {
            SavePlayerStats();
            SaveCurrencyData();
            SaveUpgrade();
        }
    }
}