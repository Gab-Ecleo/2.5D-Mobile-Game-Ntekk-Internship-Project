using System;
using EventScripts;
using Player_Statistics;
using SaveSystem.Storage;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class SaveDataManager : MonoBehaviour
    {
        private static SaveDataManager _instance;
        public static SaveDataManager Instance => _instance;

        [SerializeField] private PlayerStatsSO initialPlayerStats;
        [SerializeField] private CurrencySO playerCurrency;
        [SerializeField] private UpgradeItemsList itemList;
        [SerializeField] private AudioSettingsSO audioSettings;
        
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
            new AudioStorage().CreateAudioData();
        }

        //called at the beginning of the scene
        private void InitializeData(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 0)
            {
                LoadUpgrade();
                LoadPlayerStats();
                LoadCurrencyData();
                UpgradeShopEvents.OnUpdateCurrency?.Invoke();
            }

            if (scene.buildIndex != 0)
            {
                
            }
        }

        #region PLAYER_STATS
        public void LoadPlayerStats()
        {
            if (new StatStorages().GetStatData() == null) return;
            Debug.Log("Loading Player Stats");
            initialPlayerStats.stats = new StatStorages().GetStatData().stats;
        }

        public void SavePlayerStats()
        {
            Debug.Log("Saving Player Stats");
            var tempStats = new StatData() { stats = initialPlayerStats.stats};
            new StatStorages().SaveStatData(tempStats);
        }
        #endregion

        #region UPGRADES
        public void LoadUpgrade()
        {
            //called to update in-game data from the local storage
            if (new UpgradeStorage().GetUpgradeData() == null) return;
            Debug.Log("Loading Upgrade Progress");
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
        
        public void SaveUpgrade()
        {
            Debug.Log("Saving Upgrade Progress");
            var tempUpgrades = new UpgradeData
            {
                items = itemList.items
            };
            new UpgradeStorage().SaveUpgradeData(tempUpgrades);
        }
        #endregion

        #region CURRENCY
        public void LoadCurrencyData()
        {
            if (new CurrencyStorage().GetCurrencyData() == null) return;
            Debug.Log("Loading Currency");
            playerCurrency.coins = new CurrencyStorage().GetCurrencyData().coins;
        }
        
        public void SaveCurrencyData()
        {
            Debug.Log("Saving Currency");
            var tempCurrency = new CurrencyData { coins = playerCurrency.coins};
            new CurrencyStorage().SaveCurrencyData(tempCurrency);
        }
        #endregion

        #region AUDIO_SETTINGS

        public void LoadAudioSettingsData()
        {
            if (new AudioStorage().GetAudioData() == null) return;
            Debug.Log("Loading Audio Data");
            audioSettings.bgmVolume = new AudioStorage().GetAudioData().bgmVolume;
            audioSettings.sfxVolume = new AudioStorage().GetAudioData().sfxVolume;
        }

        public void SaveAudioSettingsData()
        {
            Debug.Log("Saving Audio Data");
            var tempData = new AudioData
            {
                bgmVolume = audioSettings.bgmVolume,
                sfxVolume = audioSettings.sfxVolume
            };
            new AudioStorage().SaveAudioData(tempData);
        }
        #endregion
        
        //called when a scene has loaded out
        private void SaveAll(Scene current)
        {
            if (current.buildIndex == 0)
            {
                Debug.Log("Unloading Scene. Saving Data");
                SavePlayerStats();
                SaveCurrencyData();
                SaveUpgrade();
                SaveAudioSettingsData();
            }
            
            if (current.buildIndex != 0)
            {
                Debug.Log("Unloading Scene. Saving Data");
                SaveAudioSettingsData();
            }
        }
        
        private void OnApplicationQuit()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Debug.Log("Quitting Application. Saving Data");
                SavePlayerStats();
                SaveCurrencyData();
                SaveUpgrade();
                SaveAudioSettingsData();
            }

            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                Debug.Log("Quitting Application. Saving Data");
                SaveAudioSettingsData();
            }
        }

        private void OnEnable()
        {
            LocalStorageEvents.OnLoadPlayerStats += LoadPlayerStats;
            LocalStorageEvents.OnSavePlayerStats += SavePlayerStats;
            
            LocalStorageEvents.OnSaveUpgradesData += SaveUpgrade;
            LocalStorageEvents.OnLoadUpgradeData += LoadUpgrade;

            LocalStorageEvents.OnLoadCurrencyData += LoadCurrencyData;
            LocalStorageEvents.OnSaveCurrencyData += SaveCurrencyData;

            LocalStorageEvents.OnLoadAudioSettingsData += LoadAudioSettingsData;
            LocalStorageEvents.OnSaveAudioSettingsData += SaveAudioSettingsData;
            
            SceneManager.sceneUnloaded += SaveAll;
            SceneManager.sceneLoaded += InitializeData;
        }

        private void OnDisable()
        {
            LocalStorageEvents.OnLoadPlayerStats -= LoadPlayerStats;
            LocalStorageEvents.OnSavePlayerStats -= SavePlayerStats;
            
            LocalStorageEvents.OnSaveUpgradesData -= SaveUpgrade;
            LocalStorageEvents.OnLoadUpgradeData -= LoadUpgrade;
            
            LocalStorageEvents.OnLoadCurrencyData -= LoadCurrencyData;
            LocalStorageEvents.OnSaveCurrencyData -= SaveCurrencyData;
            
            LocalStorageEvents.OnLoadAudioSettingsData -= LoadAudioSettingsData;
            LocalStorageEvents.OnSaveAudioSettingsData -= SaveAudioSettingsData;
            
            SceneManager.sceneUnloaded -= SaveAll;
            SceneManager.sceneLoaded -= InitializeData;
        }
    }
}